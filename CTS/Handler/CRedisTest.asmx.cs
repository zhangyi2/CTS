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
using CRedis.Third.Redis;

namespace ctrip.Framework.ApplicationFx.CTS
{
    /// <summary>
    /// Summary description for CRedisTest
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class CRedisTest : System.Web.Services.WebService
    {
        static string endline = "\r\n";
        static string testKey = "======test key======";
        static string testValue = "======test value======";
        //ICacheProvider cache = CacheFactory.GetPorvider(" ClusterName ");

        [WebMethod]
        public void TestInstances_json(string data)
        {
            CRedisInstanceInfo[] list = JsonConvert.DeserializeObject<CRedisInstanceInfo[]>(data);
            testInstanceList(list);
        }
        [WebMethod]
        public void TestInstances_list(List<string> instanceList)
        {
            List<CRedisInstanceInfo> list = new List<CRedisInstanceInfo>();
            foreach (string insStr in instanceList)
            {
                string[] ins = insStr.Split(':');
                if (ins.Length == 2)
                {
                    string ip = ins[0];
                    int port = 0;
                    if (Int32.TryParse(ins[1], out port))
                    {
                        bool isMaster = false;
                        if (ip.StartsWith("M"))
                        {
                            isMaster = true;
                            ip = ip.Substring(1);
                        }
                        list.Add(new CRedisInstanceInfo(ip, port, isMaster));
                    }
                }
            }
            testInstanceList(list.ToArray());
        }

        private void testInstanceList(CRedisInstanceInfo[] instanceList)
        {
            List<CRedisInstanceTestResult> resultList = new List<CRedisInstanceTestResult>();
            string result = null;

            bool finalStatus = true;
            foreach (CRedisInstanceInfo info in instanceList)
            {
                CRedisInstanceTestResult insResult = new CRedisInstanceTestResult(info);
                DateTime start = DateTime.Now;
                testInstance(getInstance(info.ip, info.port), info.isMaster, ref insResult);
                if (!insResult.status)
                {
                    finalStatus = false;
                }
                DateTime end = DateTime.Now;
                insResult.responseTime = (end - start).TotalMilliseconds;
                resultList.Add(insResult);
            }

            CRedisInstanceTestResults batchResult = new CRedisInstanceTestResults()
            {
                resultList = resultList.ToArray(),
                status = finalStatus
            };

            result = JsonConvert.SerializeObject(batchResult, Formatting.Indented);
            HttpContext.Current.Response.Write(result);
        }

        [WebMethod]
        public void TestGroups_json(string data)
        {
            CRedisGroupInfo[] list = JsonConvert.DeserializeObject<CRedisGroupInfo[]>(data);
            testGroupList(list);
        }
        [WebMethod]
        public void TestGroup_json(string data)
        {
            CRedisGroupInfo group= JsonConvert.DeserializeObject<CRedisGroupInfo>(data);
            testGroup(group);
        }
        private void testGroupList(CRedisGroupInfo[] groupList)
        {
            List<CRedisInstanceTestResult> resultList = new List<CRedisInstanceTestResult>();
            string result = null;

            bool finalStatus = true;
            foreach (CRedisGroupInfo info in groupList)
            {
                List<CRedisInstanceTestResult> insResultList = testInstanceByGroup(info);
                resultList.AddRange(insResultList);
                if (finalStatus)
                {
                    for (int i = 0; i < insResultList.Count; i++)
                    {
                        if (!insResultList[i].status)
                        {
                            finalStatus = false;
                            break;
                        }
                    }
                }
            }

            CRedisInstanceTestResults batchResult = new CRedisInstanceTestResults()
            {
                resultList = resultList.ToArray(),
                status = finalStatus
            };

            result = JsonConvert.SerializeObject(batchResult, Formatting.Indented);
            HttpContext.Current.Response.Write(result);
        }
        private void testGroup(CRedisGroupInfo group)
        {
            string result = null;
            bool finalStatus = true;
            List<CRedisInstanceTestResult> resultList = testInstanceByGroup(group);
            for (int i = 0; i < resultList.Count; i++)
            {
                if (!resultList[i].status)
                {
                    finalStatus = false;
                    break;
                }
            }

            CRedisInstanceTestResults batchResult = new CRedisInstanceTestResults()
            {
                resultList = resultList.ToArray(),
                status = finalStatus
            };

            result = JsonConvert.SerializeObject(batchResult, Formatting.Indented);
            HttpContext.Current.Response.Write(result);
        }

        private List<CRedisInstanceTestResult> testInstanceByGroup(CRedisGroupInfo group)
        {
            List<CRedisInstanceTestResult> insResultList = new List<CRedisInstanceTestResult>();

            int idxMaster = 0;
            for (int i = 0; i < group.instanceList.Length; i++)
            {
                if (group.instanceList[i].isMaster)
                {
                    idxMaster = i;
                    CRedisInstanceInfo insMaster = group.instanceList[i];
                    RedisClient master = new RedisClient(insMaster.ip, insMaster.port);
                    CRedisInstanceTestResult mResult = new CRedisInstanceTestResult(insMaster);
                    testMaster(master, ref mResult);
                    insResultList.Add(mResult);
                    break;
                }
            }
            for (int j = 0; j < group.instanceList.Length; j++)
            {
                if (j == idxMaster) continue;

                CRedisInstanceInfo ins = group.instanceList[j];
                RedisClient slaver = new RedisClient(ins.ip, ins.port);
                CRedisInstanceTestResult sResult = new CRedisInstanceTestResult(ins);
                testSlaver(slaver, ref sResult);
                insResultList.Add(sResult);
            }
            return insResultList;
        }

        #region private function
        private RedisClient getInstance(string ip, int port)
        {
            RedisClient client = new RedisClient(ip, port);
            return client;
        }

        private void testInstance(RedisClient client, bool isMaster, ref CRedisInstanceTestResult insResult)
        {
            string msg = null;
            DateTime start, end;
            string setValue = null, getValue = null;
            //Set
            if (isMaster)
            {
                start = DateTime.Now;
                setValue = testSet(client, out msg);
                end = DateTime.Now;
                insResult.message = msg;
                insResult.setTime = (end - start).TotalMilliseconds;
            }
            //Get
            start = DateTime.Now;
            getValue = testGet(client, out msg);
            end = DateTime.Now;
            insResult.message += endline + msg;
            insResult.getTime = (end - start).TotalMilliseconds;
            //Del
            //if (isMaster)
            //{
            //    testDel(client, out msg);
            //    insResult.message += endline + msg;
            //}

            if (setValue == getValue && getValue != null)
            {
                insResult.status = true;
            }
            else insResult.status = false;
        }
        private void testMaster(RedisClient client, ref CRedisInstanceTestResult insResult)
        {
            string msg = null;
            DateTime start, end;
            string setValue = null, getValue = null;
            //Set
            start = DateTime.Now;
            setValue = testSet(client, out msg);
            end = DateTime.Now;
            insResult.message = msg;
            insResult.setTime = (end - start).TotalMilliseconds;
            //Get
            start = DateTime.Now;
            getValue = testGet(client, out msg);
            end = DateTime.Now;
            insResult.message += endline + msg;
            insResult.getTime = (end - start).TotalMilliseconds;

            insResult.responseTime = insResult.setTime + insResult.getTime;
            if (setValue == getValue && getValue != null)
            {
                insResult.status = true;
            }
            else
            {
                insResult.status = false;
            }
        }
        private void testSlaver(RedisClient client, ref CRedisInstanceTestResult insResult)
        {
            string msg = null;
            DateTime start, end;
            string getValue = null;
            //Get
            start = DateTime.Now;
            getValue = testGet(client, out msg);
            end = DateTime.Now;
            insResult.message = msg;
            insResult.getTime = (end - start).TotalMilliseconds;

            insResult.responseTime = insResult.setTime + insResult.getTime;
            if (testValue == getValue && getValue != null)
            {
                insResult.status = true;
            }
            else
            {
                insResult.status = false;
            }
        }

        private string testGet(RedisClient client, out string message)
        {
            try
            {
                string getValue = client.Get<string>(testKey);
                message = getValue == null ? "Get null" : "Get success";
                return getValue;
            }
            catch (Exception ex)
            {
                message = "Get failed: " + ex.Message;
                return null;
            }
        }

        private string testSet(RedisClient client, out string message)
        {
            try
            {
                if (client.Set(testKey, testValue))
                {
                    message = "Set success";
                    return testValue;
                }
                else
                {
                    message = "Set failed";
                    return null;
                }
            }
            catch (Exception ex)
            {
                message = "Set failed: " + ex.Message;
                return null;
            }
        }

        private void testDel(RedisClient client, out string message)
        {
            try
            {
                client.Del(testKey);
                message = "Del success";
            }
            catch (Exception ex)
            {
                message = "Del failed: " + ex.Message;
            }
        }
        #endregion
    }
}
