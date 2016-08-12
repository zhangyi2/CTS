using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using System.IO;

namespace ctrip.Framework.ApplicationFx.CTS.CAS
{

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://www.yale.edu/tp/cas")]
    [XmlRoot("serviceResponse", Namespace = "http://www.yale.edu/tp/cas", IsNullable = false)]
    public class ServiceResponse
    {
        internal ServiceResponse() { }

        public static ServiceResponse ParseResponse(string responseXml)
        {
            XmlSerializer xs = new XmlSerializer(typeof(ServiceResponse));
            using (StringReader sr = new StringReader(responseXml))
            {
                return (ServiceResponse)xs.Deserialize(sr);
            }
        }
        //[XmlElement("proxyFailure", typeof(ProxyFailure))]
        //[XmlElement("proxySuccess", typeof(ProxySuccess))]
        [XmlElement("authenticationFailure", typeof(AuthenticationFailure))]
        [XmlElement("authenticationSuccess", typeof(AuthenticationSuccess))]
        public object Item
        {
            get;
            set;
        }

        [XmlIgnore]
        public bool IsAuthenticationFailure
        {
            get
            {
                return (Item != null && Item is AuthenticationFailure);
            }
        }

        [XmlIgnore]
        public bool IsAuthenticationSuccess
        {
            get
            {
                return (Item != null && Item is AuthenticationSuccess);
            }
        }

        //[XmlIgnore]
        //public bool IsProxyFailure
        //{
        //    get
        //    {
        //        return (Item != null && Item is ProxyFailure);
        //    }
        //}

        //[XmlIgnore]
        //public bool IsProxySuccess
        //{
        //    get
        //    {
        //        return (Item != null && Item is ProxySuccess);
        //    }
        //}
    }

}
