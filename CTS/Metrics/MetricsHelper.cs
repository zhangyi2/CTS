using Arch.CFramework.AppInternals.Components;
using Arch.CFramework.AppInternals.Components.MetricComponents;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
//using tools.fxproxy;

namespace ctrip.Framework.ApplicationFx.CTS
{
    public class MetricsHelper
    {
        public static MetricComponentBase MC_GetCounter(string mcName)
        {
            return ComponentManager.Current.GetComponent(mcName) as MetricComponentBase;
        }
        public static VisitCounter MC_Visit
        {
            get
            {
                return ComponentManager.Current.GetComponent("VisitCounter") as VisitCounter;
            }
        }
        //public static ServiceCounter MC_Service
        //{
        //    get
        //    {
        //        return ComponentManager.Current.GetComponent("ServiceCounter") as ServiceCounter;
        //    }
        //}
        //public static HttpCounter MC_Http
        //{
        //    get
        //    {
        //        return ComponentManager.Current.GetComponent("HttpCounter") as HttpCounter;
        //    }
        //}
        //public static HttpPostSize MH_HttpPostSize
        //{
        //    get
        //    {
        //        return ComponentManager.Current.GetComponent("HttpPostSize") as HttpPostSize;
        //    }
        //}
        //public static HitCounter MC_Hit
        //{
        //    get
        //    {
        //        return ComponentManager.Current.GetComponent("HitCounter") as HitCounter;
        //    }
        //}
        public static void MC_Visit_Count(string department, string page)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("Department", department);
            dic.Add("Page", page);
            MC_Visit.Set(dic, 1);
        }
        //public static void MC_Service_Count(string service, string method)
        //{
        //    Dictionary<string, string> dic = new Dictionary<string, string>();
        //    dic.Add("Service", service);
        //    dic.Add("Method", method);
        //    MC_Service.Set(dic, 1);
        //}
        //public static void MC_Http_Count(string method, string host, string path, string contentType, int statusCode)
        //{
        //    Dictionary<string, string> dic = new Dictionary<string, string>();
        //    dic.Add("Method", method);
        //    dic.Add("Host", host);
        //    dic.Add("Path", path);
        //    dic.Add("ContentType", contentType);
        //    dic.Add("StatusCode", statusCode.ToString());
        //    MC_Http.Set(dic, 1);
        //}
        //public static void MH_Http_PostSize(string host, string path, string contentType, long postSize)
        //{
        //    Dictionary<string, string> dic = new Dictionary<string, string>();
        //    dic.Add("Host", host);
        //    dic.Add("Path", path);
        //    dic.Add("ContentType", contentType);
        //    MH_HttpPostSize.Set(dic, postSize);
        //}
        //public static void MC_Hit_Count(string cacheItems, string about, double? hitCount = 1)
        //{
        //    Dictionary<string, string> dic = new Dictionary<string, string>();
        //    dic.Add("CacheItems", cacheItems);
        //    dic.Add("About", about);
        //    MC_Hit.Set(dic, hitCount.HasValue ? hitCount.Value : 1);
        //}
    }
}