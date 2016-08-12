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
    public class MyTestCase : TestCase
    {
        [DataMember(Name = "seqno")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int Seqno { get; set; }
    }

    #region entity: TestCase-ReqInfo-PostData
    [DataContract]
    [Serializable]
    public class TestCaseBaseInfo
    {
        [DataMember(Name = "name")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string caseName { get; set; }
        [DataMember(Name = "description")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string caseDescription { get; set; }
        [DataMember(Name = "bindIps")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string[] bindIpList { get; set; }
        [DataMember(Name = "packageIds")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int[] packageIdList { get; set; }
        [DataMember]
        public int testCaseId { get; set; }

        //以;分隔的字符串
        public string bindIpList_str
        {
            get
            {
                return CommonType.StringBuild(";", false, this.bindIpList);
            }
        }
        //以,分隔的字符串
        public string packageIdList_str
        {
            get
            {
                return CommonType.StringBuild<int>(",", false, this.packageIdList, id=>id.ToString());
            }
        }
    }

    [DataContract]
    [Serializable]
    public class TestCaseInfo : TestCaseBaseInfo
    {
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string reqName { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string method { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string requestUri { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string hostName { get; set; }
        [DataMember]
        public int testCaseId { get; set; }
    }

    [DataContract]
    [Serializable]
    public class TestCase
    {
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TestCaseBaseInfo baseInfo { get; set; }
        [DataMember]
        public ReqInfo info { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TestCaseDBInfo dbInfo { get; set; }  //dbInfo
        public TestCase()
            : base()
        {
            this.info = new ReqInfo();
            //this.dbInfo = new TestCaseDBInfo();
        }
    }
    #endregion

    #region dbInfo : TestCase
    [DataContract]
    [Serializable]
    public class TestCaseDBInfo
    {
        [DataMember(Name = "test_case_id")]
        public int Test_case_id { get; set; }
        //[DataMember(Name = "collection_id")]
        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        //public int Collection_id { get; set; }
        //[DataMember(Name = "package_ids")]
        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        //public string Package_ids { get; set; }
        [DataMember(Name = "host_name")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Host_name { get; set; }
        [DataMember(Name = "is_public")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool Is_public { get; set; }
        [DataMember(Name = "is_deleted")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool Is_deleted { get; set; }
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

        public TestCaseDBInfo()
            : base()
        {
            this.Is_public = false;
            this.Is_deleted = false;
            this.Create_time = DateTime.Now;
            this.Dataread_lasttime = DateTime.Now;
            this.Datachange_lasttime = DateTime.Now;
        }
        public TestCaseDBInfo(int testCaseId, int seqno, string hostName)
            : this()
        {
            this.Test_case_id = testCaseId;
            //this.Seqno = seqno;
            this.Host_name = hostName;
        }
    }
    #endregion
}