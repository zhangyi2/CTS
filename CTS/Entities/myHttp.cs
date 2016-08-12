using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Web;

namespace ctrip.Framework.ApplicationFx.CTS.Entities
{
    public class MyHttp
    {
        public const string DEFAULT_CONTENT_TYPE = "application/x-www-form-urlencoded";//"application/json", "application/x-amf";
        public const string DEFAULT_USER_AGENT = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1) ; EmbeddedWB 14.52 from: http://www.bsalsa.com/ EmbeddedWB 14.52; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; Tablet PC 2.0; .NET4.0C; .NET4.0E; 360SE)";
        public const string DEFAULT_ACCEPT = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
        public const string DEFAULT_ACCEPT_LANGUAGE = "zh-CN,zh;q=0.8";
        public const string DEFAULT_ACCEPT_CHARSET = "GBK,utf-8;q=0.7,*;q=0.3";
        public const string DEFAULT_ACCEPT_ENCODING = "gzip, deflate";
        public const string DEFAULT_CACHE_CONTROL = "no-cache";

        /// <summary>
        /// 根据ContentType获取Charset（默认为uft-8）
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static string GetCharsetByContentType(string contentType)
        {
            string charset = "utf-8";
            int idx = contentType.IndexOf("charset=");
            if (idx > 0) {
                charset = contentType.Substring(idx + 8).Split(';')[0];
            }
            return charset;
        }

        /// <summary>
        /// 设置Headers（特殊header除外，比如Host）
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="headersString"></param>
        public static void SetHeaders(WebHeaderCollection headers, string headersString)
        {
            if (string.IsNullOrEmpty(headersString)) return;

            foreach (string header in headersString.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] kv = header.Split(new string[] { ": " }, StringSplitOptions.None);
                if (kv.Length < 2 || kv[0] == "") continue;
                if (kv[0] != "Host")
                {
                    SetHeaderValue(headers, kv[0], kv[1]);
                }
            }
        }

        /// <summary>
        /// 通过反射设置Header
        /// </summary>
        /// <param name="header"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void SetHeaderValue(WebHeaderCollection header, string name, string value)
        {
            var property = typeof(WebHeaderCollection).GetProperty("InnerCollection",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (property != null)
            {
                var collection = property.GetValue(header, null) as NameValueCollection;
                collection[name] = value;
            }
        }
    }
}