using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Web;

namespace ctrip.Framework.ApplicationFx.CTS.Entities
{
    [DataContract]
    [Serializable]
    public class MyResponse
    {
        [DataMember]
        public ResponseInfo info { get; set; }
        [DataMember]
        public Status status { get; set; }

        public MyResponse()
            : base()
        {
            this.status = new Status();
        }
        public MyResponse(string exceptionMessage)
            : this()
        {
            this.status.exception = new ExceptionInfo(exceptionMessage);
        }
        public MyResponse(HttpWebResponse res)
            : this()
        {
            if (res == null) return;
            this.info = new ResponseInfo();

            this.info.method = res.Method;
            //this.info.ProtocolVersion = res.ProtocolVersion;
            //this.info.CharacterSet = res.CharacterSet;

            this.info.headers = res.Headers.ToString();
            this.info.cookies = res.Headers["Cookies"];

            this.info.contentEncoding = res.ContentEncoding;
            this.info.contentType = res.ContentType;

            this.status.statusCode = res.StatusCode;
            this.status.statusDescription = res.StatusDescription;

            this.info.isFromCache = res.IsFromCache;
            this.info.lastModified = res.LastModified;
            this.info.responseUri = res.ResponseUri.ToString();
            this.info.server = res.Server;
        }
    }
    [DataContract]
    [Serializable]
    public class ResponseInfo
    {
        [DataMember]
        public string method { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string protocolVersion { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string characterSet { get; set; }

        [DataMember]
        public string headers { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string cookies { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string contentEncoding { get; set; }
        [DataMember]
        public string contentType { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool isFromCache { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime lastModified { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string responseUri { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string server { get; set; }

        [DataMember]
        public string responseContent { get; set; }
    }
    [DataContract]
    [Serializable]
    public class Status
    {
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string actionStatus { get; set; }
        [DataMember]
        public HttpStatusCode statusCode { get; set; }
        [DataMember]
        public string statusDescription { get; set; }
        [DataMember]
        public double responseTime { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ExceptionInfo exception { get; set; }
        public Status() : base() { }
        public Status(int statusCode, string desc)
        {
            this.statusCode = (HttpStatusCode)statusCode;
            this.statusDescription = desc;
            this.responseTime = 0;
        }

        public Status(string actionStatus, int statusCode, string desc, double time)
        {
            this.actionStatus = actionStatus;
            this.statusCode = (HttpStatusCode)statusCode;
            this.statusDescription = desc;
            this.responseTime = time;
        }
    }
}