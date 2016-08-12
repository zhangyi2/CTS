using ctrip.Framework.ApplicationFx.CTS.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ctrip.Framework.ApplicationFx.CTS.Loghelper
{
    public class LogHelper
    {
        static Freeway.Logging.ILog _log;

        static LogHelper()
        {
            try
            {
                _log = Freeway.Logging.LogManager.GetLogger(typeof(LogHelper));
            }
            catch { }
        }

        public static void Info(string title, string message, Dictionary<string, string> tags = null)
        {
            if (_log == null) return;
            try
            {
                if (tags == null)
                    tags = new Dictionary<string, string>();
                tags["Operator"] = CurrentUser.CurrentLoginUser().Name;
                tags["OperTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                _log.Info(title, message, tags);
            }
            catch { }
        }

        public static void Error(string title, Exception ex, Dictionary<string, string> tags = null)
        {
            if (_log == null) return;
            try
            {
                if (tags == null)
                    tags = new Dictionary<string, string>();
                tags["Operator"] = CurrentUser.CurrentLoginUser().Name;
                tags["OperTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                _log.Error(title, ex, tags);
            }
            catch { }
        }


    }
}