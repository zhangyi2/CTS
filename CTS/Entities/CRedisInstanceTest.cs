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
    public class CRedisInstanceInfo
    {
        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public int port { get; set; }
        [DataMember]
        public bool isMaster { get; set; }
        public CRedisInstanceInfo(string m_ip, int m_port, bool m_isMaster)
            : base()
        {
            ip = m_ip;
            port = m_port;
            isMaster = m_isMaster;
        }

    }

    [DataContract]
    [Serializable]
    public class CRedisGroupInfo
    {
        [DataMember]
        public int groupId { get; set; }
        [DataMember]
        public CRedisInstanceInfo[] instanceList { get; set; }
        public CRedisGroupInfo()
            : base()
        {
        }

    }
    [DataContract]
    [Serializable]
    public class CRedisInstanceTestResults
    {
        [DataMember]
        public bool status { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CRedisInstanceTestResult[] resultList { get; set; }
        public CRedisInstanceTestResults()
            : base()
        {
        }
    }
    [DataContract]
    [Serializable]
    public class CRedisInstanceTestResult
    {
        [DataMember]
        public bool status { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string message { get; set; }
        [DataMember]
        public double responseTime { get; set; }
        [DataMember]
        public double setTime { get; set; }
        [DataMember]
        public double getTime { get; set; }
        [DataMember]
        public CRedisInstanceInfo instance { get; set; }
        public CRedisInstanceTestResult()
            : base()
        {
        }
        public CRedisInstanceTestResult(CRedisInstanceInfo ins)
            : base()
        {
            instance = ins;
        }
    }
}