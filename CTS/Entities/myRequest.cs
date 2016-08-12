using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Runtime.Serialization;
using System.Web;

namespace ctrip.Framework.ApplicationFx.CTS.Entities
{
    //var req = {
    //    "name":"",
    //    [bindIp:"",],[ipList:[],],
    //    "info":
    //    {
    //        "requestUri": "",
    //        "requestIp": "",
    //        "method": "GET",
    //        "contentType": DEFAULT_CONTENT_TYPE,
    //        "headers": "",
    //        "urlData":{}  //[protocol]://[host]:[port][path]?[queryString]
    //        "postData":{"dataMode": "params", "data": "", "inputData": "", "dataLanguage": ""}
    //    },
    //    "createTime": new Date()
    //};
    [DataContract]
    [Serializable]
    public class MyRequest
    {
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string name { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string bindIp { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string[] ipList { get; set; }
        [DataMember]
        public RequestInfo info { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Options options { get; set; }

        public MyRequest()
            : base()
        {
            this.info = new RequestInfo();
            this.options = new Options();
        }
        public MyRequest(string method, string url, string headers, string data, string contentType, int timeout)
            : this()
        {
            if (string.IsNullOrEmpty(method) || string.IsNullOrEmpty(url)) return;

            this.info.method = method;
            this.info.requestUri = url;
            this.info.headers = headers;
            if (!string.IsNullOrEmpty(data))
            {
                this.info.postData = new PostData();
                this.info.postData.data = data;
            }
            this.info.contentType = contentType ?? MyHttp.DEFAULT_CONTENT_TYPE;
            this.options.timeout = timeout <= 0 ? 10000 : timeout;
        }
    }
    [DataContract]
    [Serializable]
    public class RequestInfo : ReqInfo
    {
        public string Ip2Host { get; set; }
        //[DataMember]
        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        //public KeyValuePair<string, string> headersKVP { get; set; }
    }
    [DataContract]
    [Serializable]
    public class Options
    {
        [DataMember(Name = "send-no-cache-header")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool sendNoCacheHeader { get; set; }
        [DataMember(Name = "request-timeout")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int timeout { get; set; }
        [DataMember(Name = "keep-alive")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool keepAlive { get; set; }
    }

    #region entity: ReqHistory-ReqInfo-PostData
    public class ReqHistoryInfo
    {
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string name { get; set; }
        [DataMember]
        public string method { get; set; }
        [DataMember]
        public string requestUri { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string bindIp { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string hostName { get; set; }
        [DataMember]
        public int historyId { get; set; }
        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        //public int collectionId { get; set; }
    }
    public class ReqHistory
    {
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string name { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string bindIp { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string[] ipList { get; set; }
        [DataMember]
        public ReqInfo info { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ReqHistoryDBInfo dbInfo { get; set; }  //dbInfo
        public ReqHistory()
            : base()
        {
            this.info = new ReqInfo();
            //this.dbInfo = new ReqHistoryDBInfo();
        }

        //以;分隔的字符串
        public string IpList_Str
        {
            get
            {
                return (this.ipList == null || this.ipList.Length == 0) ? "" : CommonType.StringBuild(";", false, this.ipList);
            }
        }
    }
    [DataContract]
    [Serializable]
    public class ReqInfo
    {
        [DataMember]
        public string method { get; set; }
        [DataMember]
        public string requestUri { get; set; }
        [DataMember]
        public string contentType { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string headers { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public UrlData urlData { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PostData postData { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string author { get; set; }
        public ReqInfo()
            : base()
        {
            this.urlData = new UrlData();
            //this.postData = new PostData();
        }
    }

    [DataContract]
    [Serializable]
    public class UrlData
    {
        [DataMember]
        public string protocol { get; set; }
        [DataMember]
        public string host { get; set; }
        [DataMember]
        public string port { get; set; }
        [DataMember]
        public string path { get; set; }
        [DataMember]
        public string queryString { get; set; }
        [DataMember]
        public string url { get; set; }
    }
    [DataContract]
    [Serializable]
    public class PostData
    {
        [DataMember]
        public string dataMode { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string dataLanguage { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string data { get; set; }
        //[DataMember(Name = "inputData")]
        //public string inputData { get; set; }
    }
    #endregion

    #region dbInfo : ReqHistory
    [DataContract]
    [Serializable]
    public class ReqHistoryDBInfo
    {
        [DataMember(Name = "req_history_id")]
        public int Req_history_id { get; set; }
        //[DataMember(Name = "collection_id")]
        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        //public int Collection_id { get; set; }
        [DataMember(Name = "host_name")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Host_name { get; set; }
        [DataMember(Name = "create_user")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Create_user { get; set; }
        [DataMember(Name = "create_time")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Create_time { get; set; }
        [DataMember(Name = "dataread_lasttime")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Dataread_lasttime { get; set; }
        [DataMember(Name = "datachange_lasttime")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Datachange_lasttime { get; set; }

        public ReqHistoryDBInfo() : base()
        {
            this.Create_time = DateTime.Now;
            this.Dataread_lasttime = DateTime.Now;
            this.Datachange_lasttime = DateTime.Now;
        }
        public ReqHistoryDBInfo(int reqHistoryId, string hostName) : this()
        {
            this.Req_history_id = reqHistoryId;
            this.Host_name = hostName;
        }
    }
    #endregion
}