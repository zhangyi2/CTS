using ctrip.Framework.ApplicationFx.CTS.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;

namespace ctrip.Framework.ApplicationFx.CTS.Handler
{
    /// <summary>
    /// Summary description for MetricsService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class MetricsService : System.Web.Services.WebService
    {
        [WebMethod]
        public void MC_Count(string mcName, string tagsJson, double setValue)
        {
            MyData data = null;
            try
            {
                Dictionary<string, string> tags = JsonConvert.DeserializeObject<Dictionary<string, string>>(tagsJson);
                MetricsHelper.MC_GetCounter(mcName).Set(tags, setValue);
                data = new MyData();
            }
            catch (Exception ex){
                data = new MyData(ex);
            }
            finally
            {
                string result = JsonConvert.SerializeObject(data, Formatting.Indented);
                HttpContext.Current.Response.Write(result);
            }
        }
    }
}
