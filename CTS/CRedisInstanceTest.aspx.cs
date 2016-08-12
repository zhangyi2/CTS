using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Configuration;
using ctrip.Framework.ApplicationFx.CTS.App_Start;
using ctrip.Framework.ApplicationFx.CTS.Loghelper;

namespace ctrip.Framework.ApplicationFx.CTS
{
    public partial class CRedisInstanceTest : System.Web.UI.Page
    {
        public string CurrentUserName
        {
            get
            {
                CurrentUser user = CurrentUser.CurrentLoginUser();
                if (user == null) return "Welcome";
                else return user.Name;
            }
        }
        public string CurrentUserInfo
        {
            get
            {
                CurrentUser user = CurrentUser.CurrentLoginUser();
                if (user == null) return "";
                else return JsonConvert.SerializeObject(user, Formatting.Indented);
            }
        }
        public string GetIncludeUrl(string path)
        {
            return path + "?v=" + ConfigurationManager.AppSettings["IncludeVersion"];
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
            }
        }
    }
}