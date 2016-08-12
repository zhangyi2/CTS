using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ctrip.Framework.ApplicationFx.CTS.App_Start;
using ctrip.Framework.ApplicationFx.CTS.Loghelper;

namespace ctrip.Framework.ApplicationFx.CTS
{
    public partial class Index : BasePage
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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }
    }
}