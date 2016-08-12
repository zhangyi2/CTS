using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Runtime.Serialization;
using System.Web;

namespace ctrip.Framework.ApplicationFx.CTS.Entities
{
    [DataContract]
    [Serializable]
    public class BatchInfo
    {
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string batchId { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string stepId { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int stepCount { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int finishCount { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int timeout { get; set; }//两次请求最长间隔毫秒数，超过则视为丢包处理，强制终止
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public DateTime lastResponseTime { get; set; }//最后一次返回响应的时间
    }
    [DataContract]
    [Serializable]
    public class MyTestByStep
    {
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public BatchInfo batchInfo { get; set; }
        [DataMember(Name = "stepInfo")]
        public TestInfo testInfo { get; set; }
        public MyTestByStep()
            : base()
        {
            this.testInfo = new TestInfo();
        }
    }
    [DataContract]
    [Serializable]
    public class TestInfo
    {
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int packageId { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string packageName { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string testSeqMode { get; set; }
        [DataMember]
        public int testCaseId { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string testCaseName { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public RequestInfo reqInfo { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Options reqOptions { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string testIp { get; set; }
        public TestInfo()
            : base()
        {
            //this.request = new MyRequest();
        }
    }
    [DataContract]
    [Serializable]
    public class MyTestResultByStep
    {
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public BatchInfo batchInfo { get; set; }
        [DataMember]
        public DataStatus stepStatus { get; set; }
    }



    [DataContract]
    [Serializable]
    public class MyTestResult
    {
        [DataMember]
        public DataStatus finalStatus { get; set; }
        [DataMember]
        public double responseTime { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TestResult[] resultList { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public MyTestResultByStep lastStepResult { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int testCaseId { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string testCaseName { get; set; }
        public MyTestResult()
            : base()
        {
        }
    }
    [DataContract]
    [Serializable]
    public class TestResult
    {
        [DataMember]
        public MyResponse response { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string bindIps { get; set; }
        [DataMember]
        public DataStatus stepStatus { get; set; }
        public TestResult()
            : base()
        {
            this.response = new MyResponse();
        }
    }
}