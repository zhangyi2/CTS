using ctrip.Framework.ApplicationFx.CTS.Loghelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Web;
using System.Web.Services;
using ctrip.Framework.ApplicationFx.CTS.Entities;
using System.Collections;

namespace ctrip.Framework.ApplicationFx.CTS
{
    /// <summary>
    /// Summary description for TestManager
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class TestManager : System.Web.Services.WebService
    {
        private const int DEFAULT_TIMEOUT = 60000;//默认间隔过期时间为1s
        private const int WAIT_TIME = 100;//等待时间
        private const int TRY_COUNT = 3;

        [WebMethod]
        public void RunTestCaseByStep(string data, bool isFinished)
        {
            //MetricsHelper.MC_Service_Count(this.GetType().Name, "RunTestCaseByStep");

            MyTestResultByStep stepResult = new MyTestResultByStep();
            BatchInfo batchInfo = null;
            MyTestByStep initInfo = null;
            List<TestResult> resultList = null;
            string result = null;
            bool isBlock = false;//是否中断

            string testCaseName = null;
            string testIp = null;

            try
            {
                //预处理
                PostService postSvc = new PostService();
                MyTestByStep myTest = JsonConvert.DeserializeObject<MyTestByStep>(data);
                //int testCaseId = myTest.testInfo.testCaseId;
                testCaseName = myTest.testInfo.testCaseName;
                testIp = myTest.testInfo.testIp;
                string ip = "";
                if (!string.IsNullOrEmpty(testIp)) ip = testIp.Replace("*", "");

                //更新batch信息
                batchInfo = myTest.batchInfo;
                if (string.IsNullOrEmpty(batchInfo.batchId))
                {
                    batchInfo.batchId = string.Format("RunTestCase_{0}_{1}", testCaseName, DateTime.Now.Ticks);
                }
                if (string.IsNullOrEmpty(batchInfo.stepId))
                {
                    batchInfo.stepId = testIp;
                }
                if (batchInfo.stepCount <= 0)
                {
                    batchInfo.stepCount = 1;
                    isFinished = true;
                }

                //初始resultList，即step1
                initInfo = CacheManager.Get(batchInfo.batchId, "initInfo") as MyTestByStep;
                resultList = CacheManager.Get(batchInfo.batchId, "resultList") as List<TestResult>;
                if (resultList == null)
                {
                    resultList = new List<TestResult>();
                    if (!CacheManager.LockWithWait(batchInfo.batchId, "ALL", WAIT_TIME, TRY_COUNT))//加锁超时
                    {
                        stepResult.stepStatus = new DataStatus("Timeout for conflict reason.", DataStatusCode.Timeout);
                        isBlock = true;
                    }
                    else
                    {
                        CacheManager.Add(batchInfo.batchId, "resultList", resultList);
                        initInfo = myTest;
                        CacheManager.Add(batchInfo.batchId, "initInfo", initInfo);
                        initInfo.batchInfo.finishCount = 0;
                        if (initInfo.batchInfo.timeout <= 0)
                        {
                            initInfo.batchInfo.timeout = DEFAULT_TIMEOUT;
                        }
                        initInfo.batchInfo.startTime = DateTime.Now;
                        CacheManager.Unlock(batchInfo.batchId, "ALL");//解锁
                    }
                }
                initInfo.batchInfo.finishCount++;
                if (initInfo.batchInfo.stepCount == initInfo.batchInfo.finishCount) isFinished = true;

                if (!isBlock)
                {
                    //获取request信息
                    RequestInfo req = initInfo.testInfo.reqInfo;
                    if (req == null)//异常
                    {
                        stepResult.stepStatus = new DataStatus("Test request info is required.", DataStatusCode.ValidationException);
                    }
                    else
                    {
                        MyResponse myResponse = postSvc.SendRequest(req, initInfo.testInfo.reqOptions, ip);
                        stepResult.stepStatus = ConvertToDataStatus(myResponse.status);
                        if (!CacheManager.LockWithWait(batchInfo.batchId, testIp, WAIT_TIME, TRY_COUNT))//加锁超时
                        {
                            stepResult.stepStatus = new DataStatus("Timeout for conflict reason.", DataStatusCode.Timeout);
                        }
                        else
                        {
                            SetResult(testIp, myResponse, stepResult.stepStatus, resultList);
                            CacheManager.Unlock(batchInfo.batchId, testIp);//解锁
                        }
                    }
                    stepResult.batchInfo = initInfo.batchInfo;
                }
            }
            catch (Exception ex)
            {
                stepResult.stepStatus = new DataStatus(ex);
            }

            try
            {
                //合并batch结果
                if (isFinished)
                {
                    MyTestResult myTestResult = new MyTestResult();
                    //myTestResult.testCaseId = testCaseId;
                    myTestResult.testCaseName = testCaseName;
                    myTestResult.lastStepResult = stepResult;
                    myTestResult.resultList = resultList.ToArray();
                    myTestResult.finalStatus = myTestResult.lastStepResult.stepStatus;//new DataStatus(DataStatusCode.Success, "OK");
                    for (int i = 0; i < resultList.Count; i++)
                    {
                        if (resultList[i].stepStatus.statusCode < myTestResult.finalStatus.statusCode)
                        {
                            myTestResult.finalStatus = resultList[i].stepStatus;//取最高级别的状态
                        }
                    }
                    if (initInfo != null && initInfo.batchInfo != null)
                    {
                        initInfo.batchInfo.endTime = DateTime.Now;
                        myTestResult.responseTime = (initInfo.batchInfo.endTime - initInfo.batchInfo.startTime).TotalMilliseconds;
                    }

                    result = JsonConvert.SerializeObject(myTestResult, Formatting.Indented);
                    HttpContext.Current.Response.Write(result);
                    //clear cache
                    CacheManager.RemoveAll(batchInfo.batchId);
                    CacheManager.UnlockAll(batchInfo.batchId, true);
                }
                else
                {
                    result = JsonConvert.SerializeObject(stepResult, Formatting.Indented);
                    HttpContext.Current.Response.Write(result);
                    if (initInfo != null && initInfo.batchInfo != null) initInfo.batchInfo.lastResponseTime = DateTime.Now;
                }
            }
            finally
            {
                if (batchInfo != null)
                {
                    CacheManager.UnlockAll(batchInfo.batchId, false);
                }
            }
        }
        [WebMethod]
        public void RunTestCase(string request, string reqOptions, string bindIps)
        {
            //MetricsHelper.MC_Service_Count(this.GetType().Name, "RunTestCase");

            PostService postSvc = new PostService();
            MyTestResult myTestResult = new MyTestResult();
            RequestInfo reqInfo = JsonConvert.DeserializeObject<RequestInfo>(request);
            Options opt = string.IsNullOrEmpty(reqOptions) ? null : JsonConvert.DeserializeObject<Options>(reqOptions);
            string[] bindIpList = JsonConvert.DeserializeObject<string[]>(bindIps);

            List<TestResult> resultList = new List<TestResult>();
            if (bindIpList.Length > 0)
            {
                DateTime start = DateTime.Now;
                myTestResult.finalStatus = new DataStatus(DataStatusCode.Success, "OK");
                for (int i = 0; i < bindIpList.Length; i++)
                {
                    if (string.IsNullOrEmpty(bindIpList[i])) continue;

                    string bindIp = bindIpList[i].Replace("*", "");
                    MyResponse myResponse = postSvc.SendRequest(reqInfo, opt, bindIp);
                    DataStatus stepStatus = ConvertToDataStatus(myResponse.status);
                    SetResult(bindIpList[i], myResponse, stepStatus, resultList);

                    if (stepStatus.statusCode < myTestResult.finalStatus.statusCode)
                    {
                        myTestResult.finalStatus = resultList[i].stepStatus;//取最高级别的状态
                    }
                }
                DateTime end = DateTime.Now;
                myTestResult.responseTime = (end - start).TotalMilliseconds;
            }
            else
            {
                string bindIp = "";
                MyResponse myResponse = postSvc.SendRequest(reqInfo, opt, bindIp);
                DataStatus stepStatus = ConvertToDataStatus(myResponse.status);
                SetResult("", myResponse, stepStatus, resultList);
                myTestResult.finalStatus = stepStatus;
                myTestResult.responseTime = myResponse.status.responseTime;
            }
            myTestResult.resultList = resultList.ToArray();

            string result = JsonConvert.SerializeObject(myTestResult, Formatting.Indented);
            HttpContext.Current.Response.Write(result);
        }
        private void SetResult(string testIp, MyResponse myResponse, DataStatus status, List<TestResult> resultList)
        {
            bool hit = false;
            if (myResponse != null)
            {
                for (int j = 0; j < resultList.Count; j++)
                {
                    //根据statusCode和responseContent长度粗略判断结果是否一致
                    if (resultList[j].response != null && resultList[j].response.info != null
                        && resultList[j].response.status.statusCode == myResponse.status.statusCode
                        && resultList[j].response.info.responseContent.Length == myResponse.info.responseContent.Length)
                    {
                        resultList[j].bindIps += "," + testIp;
                        hit = true;
                        break;
                    }
                }
            }
            if (!hit)
            {
                resultList.Add(new TestResult()
                {
                    bindIps = testIp,
                    response = myResponse,
                    stepStatus = status
                });
            }
        }
        public DataStatus ConvertToDataStatus(Status status)
        {
            if (status.exception != null)
            {
                return new DataStatus(DataStatusCode.UnkownException, status.exception.message);
            }
            else
            {
                string desc = string.IsNullOrEmpty(status.statusDescription) ? status.statusCode.ToString() : status.statusDescription;
                switch (status.statusCode)
                {
                    case HttpStatusCode.OK:
                        return new DataStatus(DataStatusCode.Success, desc);
                    default:
                        return new DataStatus(DataStatusCode.Failed, desc);
                }
            }
        }
    }
}
