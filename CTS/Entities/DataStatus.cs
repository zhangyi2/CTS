using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using Arch.Data;
using ctrip.Framework.ApplicationFx.CTS.Biz;
using System.ComponentModel.DataAnnotations;

namespace ctrip.Framework.ApplicationFx.CTS.Entities
{
    [DataContract]
    [Serializable]
    public class DataStatus
    {
        [DataMember]
        public DataStatusCode statusCode { get; set; }
        [DataMember]
        public string statusDescription { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ExceptionInfo exception { get; set; }
        public DataStatus() : base() { }
        public DataStatus(DataStatusCode statusCode, string desc)
        {
            this.statusCode = (DataStatusCode)statusCode;
            this.statusDescription = desc;
        }

        public DataStatus(string exceptionMessage)
        {
            this.statusCode = DataStatusCode.UnkownException;
            this.exception = new ExceptionInfo(exceptionMessage);
        }

        public DataStatus(string exceptionMessage, DataStatusCode statusCode)
        {
            this.statusCode = statusCode;
            this.exception = new ExceptionInfo(exceptionMessage);
        }
        public DataStatus(Exception ex)
        {
            if (ex is ValidationException)
            {
                this.statusCode = DataStatusCode.ValidationException;
            }
            else if (ex is DalException)
            {
                this.statusCode = DataStatusCode.DalException;
            }
            else if (ex is BizException)
            {
                this.statusCode = DataStatusCode.BizException;
            }
            else
            {
                this.statusCode = DataStatusCode.UnkownException;
            }
            this.exception = new ExceptionInfo(this.statusCode.ToString(), ex.Message, ex.StackTrace);
        }
    }

    public enum DataStatusCode
    {
        Success = 0,
        Abort = -1,
        Failed = -2,
        Error = -3,
        Timeout = -9,
        ValidationException = -10,
        DalException = -20,
        BizException = -30,
        UnkownException = -99
    }

    [DataContract]
    [Serializable]
    public class ExceptionInfo
    {
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string errorCode { get; set; }
        [DataMember]
        public string message { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string trace { get; set; }

        public ExceptionInfo(string msg)
        {
            this.message = msg;
        }
        public ExceptionInfo() : base() { }
        public ExceptionInfo(string code, string msg, string trace)
        {
            this.errorCode = code;
            this.message = msg;
            this.trace = trace;
        }
    }
}