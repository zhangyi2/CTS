using Arch.CFramework.AppInternals.Components.MetricComponents;
using Arch.CFramework.AppInternals.Components.MetricComponents.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ctrip.Framework.ApplicationFx.CTS
{
    /// <summary>
    /// 用户页面访问量，调度周期为1分钟
    /// </summary>
    [MetricScheduling(Circle = 1)]
    public class VisitCounter : MetricComponentBase
    {
        [Tag]
        public string Department { get; set; }
        [Tag]
        public string Page { get; set; }
        [CounterMetric("PageVisitCount")]
        public double VisitCount { get; set; }
    }
    /// <summary>
    /// 服务访问量，调度周期为1分钟
    /// </summary>
    [MetricScheduling(Circle = 1)]
    public class ServiceCounter : MetricComponentBase
    {
        [Tag]
        public string Service { get; set; }
        [Tag]
        public string Method { get; set; }
        [CounterMetric("ServiceRequestCount")]
        public double RequestCount { get; set; }
    }
    /// <summary>
    /// Http请求数，调度周期为1分钟
    /// </summary>
    [MetricScheduling(Circle = 1)]
    public class HttpCounter : MetricComponentBase
    {
        [Tag]
        public string Method { get; set; }
        [Tag]
        public string Host { get; set; }
        [Tag]
        public string Path { get; set; }
        [Tag]
        public string ContentType { get; set; }
        /// <summary>
        /// >0成功（参见HttpStatusCode），<0异常（详见ErrorCode）
        /// </summary>
        [Tag]
        public string StatusCode { get; set; }
        [CounterMetric("HttpRequestCount")]
        public double HttpCount { get; set; }
    }
    /// <summary>
    /// Http请求请求报文大小，调度周期为1分钟
    /// </summary>
    [MetricScheduling(Circle = 1)]
    public class HttpPostSize : MetricComponentBase
    {
        [Tag]
        public string Host { get; set; }
        [Tag]
        public string Path { get; set; }
        [Tag]
        public string ContentType { get; set; }
        [HistogramMetric("HttpPostSize")]
        public double PostSize { get; set; }
    }
    /// <summary>
    /// 导入导出数据条数，调度周期为1分钟
    /// </summary>
    [MetricScheduling(Circle = 1)]
    public class InOutCounter : MetricComponentBase
    {
        [Tag]
        public string Department { get; set; }
        /// <summary>
        /// ReqImport, ReqExport, ResExport
        /// </summary>
        [Tag]
        public string About { get; set; }
        /// <summary>
        /// 统计出入记录条数
        /// </summary>
        [CounterMetric("InOutItemsCount")]
        public double ItemsCount { get; set; }
    }
    /// <summary>
    /// 缓存命中次数，调度周期为1分钟
    /// </summary>
    [MetricScheduling(Circle = 1)]
    public class HitCounter : MetricComponentBase
    {
        [Tag]
        public string CacheItemsCount { get; set; }
        /// <summary>
        /// RecentHistory, RecentTestcase, RecentTestPackage
        /// </summary>
        [Tag]
        public string About { get; set; }
        /// <summary>
        /// 统计命中数
        /// </summary>
        [CounterMetric("CacheHitCount")]
        public double HitCount { get; set; }
    }

    /// <summary>
    /// 测试次数，调度周期为1分钟
    /// </summary>
    [MetricScheduling(Circle = 1)]
    public class TestCounter : MetricComponentBase
    {
        [Tag]
        public string Department { get; set; }
        /// <summary>
        /// TestCase, TestPackage
        /// </summary>
        [Tag]
        public string About { get; set; }
        /// <summary>
        /// 提交测试次数
        /// </summary>
        [CounterMetric("TestSubmitCount")]
        public double TestCount { get; set; }
    }
    /// <summary>
    /// 实际用例跑测次数，调度周期为1分钟
    /// </summary>
    [MetricScheduling(Circle = 1)]
    public class CaseRunningCounter : MetricComponentBase
    {
        [Tag]
        public string Department { get; set; }
        /// <summary>
        /// success,abort,timeout,exception,failed
        /// </summary>
        [Tag]
        public string StatusCode { get; set; }
        /// <summary>
        /// 用例跑测次数
        /// </summary>
        [CounterMetric("CaseRunningCount")]
        public double RunningCount { get; set; }
    }
}