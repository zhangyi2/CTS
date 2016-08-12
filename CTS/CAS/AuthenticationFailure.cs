using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Globalization;

namespace ctrip.Framework.ApplicationFx.CTS.CAS
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://www.yale.edu/tp/cas")]
    public class AuthenticationFailure
    {
        internal AuthenticationFailure() { }

        [XmlAttribute("code")]
        public string Code
        {
            get;
            set;
        }

        [XmlText]
        public string Message
        {
            get;
            set;
        }

        [XmlIgnore]
        public bool IsInvalidRequester
        {
            get
            {
                return String.Compare(Code, "INVALID_REQUEST", true, CultureInfo.InvariantCulture) == 0;
            }
        }

        [XmlIgnore]
        public bool IsInvalidTicket
        {
            get
            {
                return String.Compare(Code, "INVALID_TICKET", true, CultureInfo.InvariantCulture) == 0;
            }
        }

        [XmlIgnore]
        public bool IsInvalidService
        {
            get
            {
                return String.Compare(Code, "INVALID_SERVICE", true, CultureInfo.InvariantCulture) == 0;
            }
        }

        [XmlIgnore]
        public bool IsInternalError
        {
            get
            {
                return String.Compare(Code, "INTERNAL_ERROR", true, CultureInfo.InvariantCulture) == 0;
            }
        }
    }
}
