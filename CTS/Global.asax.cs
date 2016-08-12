using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using ctrip.Framework.ApplicationFx.CTS;
using ctrip.Framework.ApplicationFx.CTS.App_Start;
using Arch.CFramework.AppInternals.Components;

namespace ctrip.Framework.ApplicationFx.CTS
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            ComponentManager.Current.Register(new VisitCounter());
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }
    }
}
