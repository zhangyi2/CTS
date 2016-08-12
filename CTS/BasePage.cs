using ctrip.Framework.ApplicationFx.CTS.CAS;
using ctrip.Framework.ApplicationFx.CTS.App_Start;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace ctrip.Framework.ApplicationFx.CTS
{
    public class BasePage : System.Web.UI.Page
    {
        #region 属性
        public static string caspath = System.Configuration.ConfigurationManager.AppSettings["SSOUrl"];
        #endregion
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }
        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
            #region sso
            try
            {
               
                SetUrlReferrer();
                if (CurrentUser.CurrentLoginUser() != null)
                {
                    MetricsHelper.MC_Visit_Count(CurrentUser.CurrentLoginUser().Department, Request.Url.AbsolutePath);
                    return;
                }
                else
                {
                    string service = ServiceUrl(Request) + "/Index.aspx";
                    string ticket = Request["ticket"];
                    if (string.IsNullOrEmpty(ticket))
                    {
                        var url = caspath + "/login?&service=" + HttpUtility.UrlEncode(service);

                        var js = "<script type='text/javascript'>window.top.location='" + url + "'</script>";

                        Response.Write(js);
                        Response.End();

                        //Response.Redirect(caspath + "/login?&service=" + HttpUtility.UrlEncode(service), true);
                    }
                    else
                    {
                        string validateurl = caspath + "/serviceValidate?service=" + HttpUtility.UrlEncode(service) + "&ticket=" + ticket;
                        WebClient wc = new WebClient();
                        ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                        StreamReader Reader = new StreamReader(wc.OpenRead(validateurl));
                        string resp = Reader.ReadToEnd();

                        ServiceResponse serviceResponse = ServiceResponse.ParseResponse(resp);
                        if (serviceResponse.IsAuthenticationSuccess)
                        {
                            AuthenticationSuccess authSuccessResponse = (AuthenticationSuccess)serviceResponse.Item;
                            CurrentUser.SetCurrentLoginUser(authSuccessResponse);
                            MetricsHelper.MC_Visit_Count(CurrentUser.CurrentLoginUser().Department, Request.Url.AbsolutePath);
                            string url = service;
                            Response.Redirect(url);
                            return;
                        }
                        else
                        {
                            Response.Redirect(caspath + "/login?&service=" + HttpUtility.UrlEncode(service), false);
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                 
            }
           
            #endregion
        }
        protected void SetUrlReferrer()
        {
            if (Request.UrlReferrer != null && Session["UrlReferrer"] == null && Request.UrlReferrer.Host == Request.Url.Host && Request.UrlReferrer.GetLeftPart(UriPartial.Path) != Request.Url.GetLeftPart(UriPartial.Path))
            {
                Session["UrlReferrer"] = Request.UrlReferrer;
            }
        }
        public string ServiceUrl(HttpRequest request)
        {

            StringBuilder builder = new StringBuilder();
            builder.Append(Request.Url.Scheme);
            builder.Append("://");
            builder.Append(Request.Url.Authority);
         
            builder.Append(request.ApplicationPath);
            return builder.ToString().TrimEnd('/');
        }
        
    }
}