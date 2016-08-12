using ctrip.Framework.ApplicationFx.CTS.Loghelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Web;
using System.Web.Services;
using ctrip.Framework.ApplicationFx.CTS.Entities;

namespace ctrip.Framework.ApplicationFx.CTS
{
    /// <summary>
    /// Summary description for PostService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class PostService : System.Web.Services.WebService
    {
        [WebMethod]
        public void Download(string fileName, string content)
        {
            //MetricsHelper.MC_Service_Count(this.GetType().Name, "Download");

            //fileName = HttpUtility.UrlEncode(fileName, Encoding.UTF8);
            long fileSize = content.Length;
            byte[] fileBuffer = System.Text.Encoding.Default.GetBytes(content);

            Context.Response.ContentType = "application/octet-stream";
            Context.Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
            Context.Response.AddHeader("Content-Length", fileSize.ToString());

            //Context.Response.OutputStream.Write(fileBuffer, 0, (int)fileSize);
            Context.Response.BinaryWrite(fileBuffer);
            Context.Response.Flush();
            Context.Response.Close();
        }
        [WebMethod]
        public void HttpRequest(string request)
        {
            //MetricsHelper.MC_Service_Count(this.GetType().Name, "HttpRequest");
            MyRequest myRequest = JsonConvert.DeserializeObject<MyRequest>(request);
            MyResponse myResponse = SendRequest(myRequest);
            string result = JsonConvert.SerializeObject(myResponse, Formatting.Indented);
            HttpContext.Current.Response.Write(result);
        }
        public MyResponse SendRequest(MyRequest myRequest)
        {
            return SendRequest(myRequest.info, myRequest.options, myRequest.bindIp);
        }
        public MyResponse SendRequest(RequestInfo req, Options opt, string bindIp)
        {
            #region 读取request
            if (req.requestUri != "" && req.requestUri.IndexOf("://") < 0)
            {
                req.requestUri = "http://" + req.requestUri;
            }
            UrlData urlData = req.urlData;
            PostData postData = req.postData;
            if (postData != null && postData.dataMode == "raw")
                postData.data = System.Web.HttpUtility.HtmlDecode(postData.data);

            if (!string.IsNullOrEmpty(bindIp))
            {
                string host = urlData.host;
                req.requestUri = req.requestUri.Replace("://" + host, "://" + bindIp);
                req.Ip2Host = host;
            }
            #endregion

            //提交请求
            DateTime start = DateTime.Now;
            MyResponse myResponse = DoHttpRequest(req, opt);
            DateTime end = DateTime.Now;
            double responseTime = (end - start).TotalMilliseconds;
            myResponse.status.responseTime = responseTime;

            if (myResponse.info == null)
            {
                //请求失败
                myResponse.status.actionStatus = "response_none";
                myResponse.status.statusCode = 0;
                myResponse.status.statusDescription = "There is no response.";
            }
            else
            {
                if (myResponse.status.statusCode == HttpStatusCode.OK)
                {
                    myResponse.status.actionStatus = "response_success";
                }
                else
                {
                    myResponse.status.actionStatus = "response_failed";
                }
            }
            return myResponse;
        }
        private MyResponse DoHttpRequest(RequestInfo req, Options opt)
        {
            MyResponse res = null;
            string exceptionInfo = string.Empty;
            if (opt == null)
            {
                opt = new Options()
                {
                    sendNoCacheHeader = true,
                    timeout = 0,
                    keepAlive = false
                };
            }

            HttpWebRequest httpWebRequest = null;
            HttpWebResponse httpWebResponse = null;
            StreamReader streamReader = null;

            if (string.IsNullOrEmpty(req.method) || req.requestUri == null)
            {
                res = new MyResponse("Request error!<br/>The request url and method should not be empty.");
                //LogHelper.Info("Request error", "The request url and method should not be empty.", GetInfo(req));
                return res;
            }
            try
            {
                httpWebRequest = (HttpWebRequest)WebRequest.Create(req.requestUri);
                //httpWebRequest.Proxy = WebRequest.DefaultWebProxy;
                //httpWebRequest.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;

                //设置必要header
                httpWebRequest.ContentType = req.contentType ?? MyHttp.DEFAULT_CONTENT_TYPE; //"application/x-www-form-urlencoded", "application/json", "multipart/form-data", "text/plain", "application/x-amf";
                httpWebRequest.Method = req.method;
                if (opt.timeout > 0) httpWebRequest.Timeout = opt.timeout;
                httpWebRequest.ContentLength = 0;

                //设置默认header
                httpWebRequest.Accept = MyHttp.DEFAULT_ACCEPT;
                httpWebRequest.Headers["Accept-Language"] = MyHttp.DEFAULT_ACCEPT_LANGUAGE;
                httpWebRequest.Headers["Accept-Charset"] = MyHttp.DEFAULT_ACCEPT_CHARSET;
                //httpWebRequest.Headers["x-flash-version"] = "10,1,53,64";
                httpWebRequest.Headers["Accept-Encoding"] = MyHttp.DEFAULT_ACCEPT_ENCODING;
                httpWebRequest.UserAgent = MyHttp.DEFAULT_USER_AGENT;
                if (opt.sendNoCacheHeader)
                    httpWebRequest.Headers["Cache-Control"] = MyHttp.DEFAULT_CACHE_CONTROL;
                httpWebRequest.KeepAlive = opt.keepAlive;//测试避免长连接
                //httpWebRequest.AllowAutoRedirect = false;//避免自动跳转

                //设置自定义header
                SetHeaders(httpWebRequest, req.headers);
                if (!string.IsNullOrEmpty(req.Ip2Host))//显性绑定IP（非header输入）
                {
                    httpWebRequest.Host = req.Ip2Host;
                }

                string charset = MyHttp.GetCharsetByContentType(httpWebRequest.ContentType);
                Encoding encoding = Encoding.GetEncoding(charset);
                if (req.method == "POST" && req.postData != null && !string.IsNullOrEmpty(req.postData.data))
                {
                    byte[] btBodys = encoding.GetBytes(req.postData.data);
                    httpWebRequest.ContentLength = btBodys.Length;
                    httpWebRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);

                    //MetricsHelper.MH_Http_PostSize(req.urlData.host, req.urlData.path, req.contentType, btBodys.Length);
                }

                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                res = new MyResponse(httpWebResponse);

                Stream responseStream = httpWebResponse.GetResponseStream();
                //如果http头中接受gzip，则需要解压缩
                if (httpWebResponse.ContentEncoding.Contains("gzip"))
                {
                    responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
                }

                string contentType = httpWebResponse.ContentType;
                if (contentType.StartsWith("image/"))
                {
                    //res.ResponseContent = ToBase64(responseStream);
                }
                else
                {
                    charset = MyHttp.GetCharsetByContentType(contentType);
                    encoding = Encoding.GetEncoding(charset);
                    streamReader = new StreamReader(responseStream, encoding);
                    res.info.responseContent = streamReader.ReadToEnd();

                    streamReader.Close();
                }

                httpWebResponse.Close();
                httpWebRequest.Abort();

                LogHelper.Info("Request success", req.requestUri, GetInfo(req));
                //MetricsHelper.MC_Http_Count(req.method, req.urlData.host, req.urlData.path, req.contentType, (int)res.status.statusCode);
                return res;
            }
            catch (Exception e)
            {
                res = new MyResponse("There is an exception occured:<br/>" + e.Message);
                LogHelper.Error("HttpRequest exception", e, GetInfo(req));
                //MetricsHelper.MC_Http_Count(req.method, req.urlData.host, req.urlData.path, req.contentType, (int)DataStatusCode.UnkownException);
                return res;
            }
            finally
            {
                if (streamReader != null)
                {
                    streamReader.Close();
                }
                if (httpWebResponse != null)
                {
                    httpWebResponse.Close();
                }
                if (httpWebRequest != null)
                {
                    httpWebRequest.Abort();
                }
            }
        }
        private void SetHeaders(HttpWebRequest request, string headersString)
        {
            if (string.IsNullOrEmpty(headersString)) return;

            foreach (string header in headersString.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] kv = header.Split(new string[] { ": " }, StringSplitOptions.None);
                if (kv.Length < 2 || kv[0] == "") continue;
                if (kv[0].ToLower() == "host")
                {
                    if (!string.IsNullOrWhiteSpace(kv[1])) request.Host = kv[1];
                    //httpWebRequest.Proxy = new WebProxy(kv[1]);//不支持https
                }
                else
                {
                    MyHttp.SetHeaderValue(request.Headers, kv[0], kv[1]);
                    //request.Headers.Set(kv[0], kv[1]);
                }
            }
        }
        private Dictionary<string, string> GetInfo(MyRequest req)
        {
            return GetInfo(req.info);
        }
        private Dictionary<string, string> GetInfo(ReqInfo curRequest)
        {
            Dictionary<string, string> tags = new Dictionary<string, string>();
            tags["Method"] = curRequest.method;
            tags["RequestUri"] = curRequest.requestUri;
            tags["Headers"] = curRequest.headers;
            tags["RequestData"] = curRequest.postData != null ? curRequest.postData.data : "";
            tags["ContentType"] = curRequest.contentType;
            return tags;
        }
        private string ToBase64(Stream stream)
        {
            System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
            BinaryFormatter binFormatter = new BinaryFormatter();
            MemoryStream memStream = new MemoryStream();
            binFormatter.Serialize(memStream, img);
            byte[] bytes = memStream.GetBuffer();
            string base64 = Convert.ToBase64String(bytes);
            return base64;
        }
    }
}
