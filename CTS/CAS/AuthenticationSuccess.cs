using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

namespace ctrip.Framework.ApplicationFx.CTS.CAS
{

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://www.yale.edu/tp/cas")]
    public class AuthenticationSuccess
    {
        internal AuthenticationSuccess() { }

        [XmlElement("user")]
        public string User
        {
            get;
            set;
        }

        [XmlElement("attributes", typeof(AttributeElement))]
        public AttributeElement Attributes
        {
            get;
            set;
        }


        //[XmlElement("proxyGrantingTicket")]
        //public string ProxyGrantingTicket
        //{
        //    get;
        //    set;
        //}

        //[XmlArray("proxies")]
        //[XmlArrayItem("proxy", IsNullable = false)]
        //public string[] Proxies
        //{
        //    get;
        //    set;
        //}
    }
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://www.yale.edu/tp/cas")]
    public class AttributeElement
    {
        [XmlIgnore]
        public string this[string name]
        {
            get
            {
                foreach (var item in Items)
                {
                    if (item.LocalName.Equals(name, StringComparison.OrdinalIgnoreCase))
                    {
                        return item.InnerText;
                    }
                }
                return "";
            }
        }
        [XmlAnyElement]
        public System.Xml.XmlElement[] Items { get; set; }
    }

}
