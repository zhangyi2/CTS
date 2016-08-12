using ctrip.Framework.ApplicationFx.CTS.CAS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ctrip.Framework.ApplicationFx.CTS.App_Start
{
    [Serializable]
    public class CurrentUser
    {
        public CurrentUser()
        {

        }
        /// <summary>
        /// 域帐号
        /// </summary>
        public string SN { get; set; }
        /// <summary>
        /// 中文显示名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public string Department { get; set; }
        /// <summary>
        /// 公司: 例如IT
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// 工号: s*****
        /// </summary>
        public string Employee { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Mail { get; set; }

        private bool isAuthorized;
        public bool IsAuthorized
        {
            get { return isAuthorized; }
        }
        public static CurrentUser CurrentLoginUser()
        {
            CurrentUser currentUser = System.Web.HttpContext.Current.Session["CurrentUser"] as CurrentUser;

            return currentUser;
        }
        public static void SetCurrentLoginUser(AuthenticationSuccess authinfo)
        {
            if (authinfo == null)
            {
                System.Web.HttpContext.Current.Session["CurrentUser"] = null;
            }
            else
            {
                CurrentUser user = new CurrentUser() { 
                    SN = authinfo.User, 
                    Name = authinfo.Attributes["sn"], 
                    Department = authinfo.Attributes["department"],
                    Company = authinfo.Attributes["company"],
                    Employee = authinfo.Attributes["employee"],
                    Mail = authinfo.Attributes["mail"], 
                    isAuthorized = true 
                };
                System.Web.HttpContext.Current.Session["CurrentUser"] = user;
                //(new Task(() => UpdateUser(authinfo))).Start();
            }
        }
    }
}