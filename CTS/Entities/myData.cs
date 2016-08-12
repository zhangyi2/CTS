using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ctrip.Framework.ApplicationFx.CTS.Entities
{
    [DataContract]
    [Serializable]
    public class MyData
    {
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Object data { get; set; }
        [DataMember]
        public DataStatus status { get; set; }

        public MyData() : base()
        {
            this.status = new DataStatus();
        }
        public MyData(string exceptionMessage) : this()
        {
            this.status = new DataStatus(exceptionMessage);
        }
        public MyData(Exception ex) : this()
        {
            this.status = new DataStatus(ex);
        }
        public MyData(Object data) : this()
        {
            if (data == null) return;
            this.data = data;
        }
    }
}