using System;
using System.Runtime.Serialization;

namespace ctrip.Framework.ApplicationFx.CTS.Biz
{
    [Serializable]
    public class BizException : Exception
    {
        public BizException() :base() {}
        public BizException(string errorMessage) : base(errorMessage) { }
        protected BizException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public BizException(string message, Exception innerException) : base(message, innerException) { }
        public BizException(string msgFormat, params object[] os) : base(string.Format(msgFormat, os)) { }
    }
}