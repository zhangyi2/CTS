using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web;
using DataModel = ctrip.Framework.ApplicationFx.CTS.DAL.Entity.DataModel;

namespace ctrip.Framework.ApplicationFx.CTS.Entities
{
    public class ConvertEntity
    {
        public static List<T2> Convert<T1, T2>(List<T1> list)
        {
            try
            {
                List<T2> objlist = new List<T2>();
                foreach (T1 entity1 in list)
                {
                    T2 entity2 = Convert<T1, T2>(entity1);
                    objlist.Add(entity2);
                }
                return objlist;
            }
            catch (Exception ex)
            {
                throw new Exception("调用ConvertEntity时，访问Convert<T1, T2>(List<T1>)时出错", ex);
            }
        }
        public static T2 Convert<T1, T2>(T1 entity1)
        {
            try
            {
                Object obj = null;
                if (typeof(T1) == typeof(ReqHistory))
                {
                    DataModel.ReqHistory target = null;
                    Convert(entity1 as ReqHistory, out target);
                    obj = target;
                }
                else if (typeof(T1) == typeof(DataModel.ReqHistory))
                {
                    if (typeof(T2) == typeof(ReqHistory))
                    {
                        ReqHistory target = null;
                        Convert(entity1 as DataModel.ReqHistory, out target);
                        obj = target;
                    }
                    else if (typeof(T2) == typeof(ReqHistoryInfo))
                    {
                        ReqHistoryInfo target = null;
                        Convert(entity1 as DataModel.ReqHistory, out target);
                        obj = target;
                    }
                }
                else if (typeof(T1) == typeof(TestCase))
                {
                    DataModel.TestCase target = null;
                    Convert(entity1 as TestCase, out target);
                    obj = target;
                }
                else if (typeof(T1) == typeof(DataModel.TestCase))
                {
                    if (typeof(T2) == typeof(TestCase))
                    {
                        TestCase target = null;
                        Convert(entity1 as DataModel.TestCase, out target);
                        obj = target;
                    }
                    else if (typeof(T2) == typeof(TestCaseInfo))
                    {
                        TestCaseInfo target = null;
                        Convert(entity1 as DataModel.TestCase, out target);
                        obj = target;
                    }
                }
                else if (typeof(T1) == typeof(TestPackage))
                {
                    DataModel.TestPackage target = null;
                    Convert(entity1 as TestPackage, out target);
                    obj = target;
                }
                else if (typeof(T1) == typeof(DataModel.TestPackage))
                {
                    if (typeof(T2) == typeof(TestPackage))
                    {
                        TestPackage target = null;
                        Convert(entity1 as DataModel.TestPackage, out target);
                        obj = target;
                    }
                    else if (typeof(T2) == typeof(TestPackageInfo))
                    {
                        TestPackageInfo target = null;
                        Convert(entity1 as DataModel.TestPackage, out target);
                        obj = target;
                    }
                }
                return (T2)obj;
            }
            catch (Exception ex)
            {
                throw new Exception("调用ConvertEntity时，访问Convert<T1, T2>(T1)时出错", ex);
            }
        }

        #region Convert entity to data modal: ReqHistory-ReqInfo-PostData
        private static void Convert(ReqHistory source, out DataModel.ReqHistory target)
        {
            target = new DataModel.ReqHistory();

            if (source.info != null)
            {
                target.Method = source.info.method;
                target.Request_uri = source.info.requestUri;
                target.Content_type = source.info.contentType;
                target.Headers = source.info.headers;
                target.Author = source.info.author;

                if (source.info.postData != null)
                {
                    target.Data_mode = source.info.postData.dataMode;
                    target.Data_language = source.info.postData.dataLanguage;
                    target.Post_data = source.info.postData.data;//这里不做Decode
                }

                target.Host_name = source.info.urlData.host;
            }

            target.Bind_ip = source.bindIp;
            target.Ip_list = source.IpList_Str;
            if (source.dbInfo != null)
            {
                target.Req_history_id = source.dbInfo.Req_history_id;
                target.Create_user = source.dbInfo.Create_user;
                target.Create_time = source.dbInfo.Create_time;
                target.Dataread_lasttime = source.dbInfo.Dataread_lasttime;
                target.Datachange_lasttime = source.dbInfo.Datachange_lasttime;
            }
        }
        #endregion
        #region Convert data modal to entity: ReqHistory-ReqInfo-PostData, ReqHistoryInfo
        private static void Convert(DataModel.ReqHistory source, out ReqHistory target)
        {
            target = new ReqHistory();

            target.info = new ReqInfo()
            {
                method = source.Method,
                requestUri = source.Request_uri,
                contentType = source.Content_type,
                headers = source.Headers,
                author = source.Author
            };
            if (source.Method == "POST")
            {
                target.info.postData = new PostData()
                {
                    dataMode = source.Data_mode,
                    dataLanguage = source.Data_language,
                    data = source.Post_data//这里不做Encode
                };
            }

            target.bindIp = source.Bind_ip;
            target.ipList = string.IsNullOrEmpty(source.Ip_list) ? null : source.Ip_list.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            target.dbInfo = new ReqHistoryDBInfo()
            {
                Req_history_id = source.Req_history_id,
                Create_user = source.Create_user,
                Create_time = source.Create_time,
                Dataread_lasttime = source.Dataread_lasttime,
                Datachange_lasttime = source.Datachange_lasttime
            };
        }
        private static void Convert(DataModel.ReqHistory source, out ReqHistoryInfo target)
        {
            target = new ReqHistoryInfo()
            {
                method = source.Method,
                requestUri = source.Request_uri,
                bindIp = source.Bind_ip,
                hostName = source.Host_name,
                historyId = source.Req_history_id,
                //collectionId = source.Collection_id
            };
        }
        #endregion

        #region Convert entity to data modal: TestCase-ReqInfo-PostData
        private static void Convert(TestCase source, out DataModel.TestCase target)
        {
            target = new DataModel.TestCase();

            if (source.info != null)
            {
                target.Method = source.info.method;
                target.Request_uri = source.info.requestUri;
                target.Content_type = source.info.contentType;
                target.Headers = source.info.headers;
                target.Author = source.info.author;

                if (source.info.postData != null)
                {
                    target.Data_mode = source.info.postData.dataMode;
                    target.Data_language = source.info.postData.dataLanguage;
                    target.Post_data = source.info.postData.data;//这里不做Decode
                }

                target.Host_name = source.info.urlData.host;
            }

            if (source.baseInfo != null)
            {
                target.Test_case_id = source.baseInfo.testCaseId;
                target.Case_name = source.baseInfo.caseName;
                target.Case_description = source.baseInfo.caseDescription;
                target.Bind_ips = source.baseInfo.bindIpList_str;
                target.Package_ids = source.baseInfo.packageIdList_str;
            }
            if (source.dbInfo != null)
            {
                target.Is_public = source.dbInfo.Is_public;
                target.Is_deleted = source.dbInfo.Is_deleted;
                target.Create_user = source.dbInfo.Create_user;
                target.Create_time = source.dbInfo.Create_time;
                target.Dataread_lasttime = source.dbInfo.Dataread_lasttime;
                target.Datachange_lasttime = source.dbInfo.Datachange_lasttime;
            }
        }
        #endregion
        #region Convert data modal to entity: TestCase-ReqInfo-PostData, TestCaseInfo
        private static void Convert(DataModel.TestCase source, out TestCase target)
        {
            target = new TestCase();

            target.info = new ReqInfo()
            {
                method = source.Method,
                requestUri = source.Request_uri,
                contentType = source.Content_type,
                headers = source.Headers,
                author = source.Author
            };
            if (source.Method == "POST")
            {
                target.info.postData = new PostData()
                {
                    dataMode = source.Data_mode,
                    dataLanguage = source.Data_language,
                    data = source.Post_data//这里不做Encode
                };
            }

            target.baseInfo = new TestCaseBaseInfo()
            {
                testCaseId = source.Test_case_id,
                caseName = source.Case_name,
                caseDescription = source.Case_description,
                bindIpList = string.IsNullOrEmpty(source.Bind_ips) ? null : source.Bind_ips.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries),
                packageIdList = string.IsNullOrEmpty(source.Package_ids) ? null : CommonType.Split<int>(source.Package_ids, ",", id => int.Parse(id)).ToArray()
            };
            target.dbInfo = new TestCaseDBInfo()
            {
                Test_case_id = source.Test_case_id,
                Is_public = source.Is_public,
                Is_deleted = source.Is_deleted,
                Create_user = source.Create_user,
                Create_time = source.Create_time,
                Dataread_lasttime = source.Dataread_lasttime,
                Datachange_lasttime = source.Datachange_lasttime
            };
        }
        private static void Convert(DataModel.TestCase source, out TestCaseInfo target)
        {
            target = new TestCaseInfo()
            {
                method = source.Method,
                requestUri = source.Request_uri,
                hostName = source.Host_name,
                testCaseId = source.Test_case_id,
                caseName = source.Case_name,
                caseDescription = source.Case_description,
                bindIpList = string.IsNullOrEmpty(source.Bind_ips) ? null : source.Bind_ips.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries),
                packageIdList = string.IsNullOrEmpty(source.Package_ids) ? null : CommonType.Split<int>(source.Package_ids, ",", id=>int.Parse(id)).ToArray(),
                //collectionId = source.Collection_id
            };
        }
        #endregion

        #region Convert entity to data modal: TestPackage
        private static void Convert(TestPackage source, out DataModel.TestPackage target)
        {
            target = new DataModel.TestPackage();

            if (source.baseInfo != null)
            {
                target.Package_id = source.baseInfo.packageId;
                target.Package_name = source.baseInfo.packageName;
                target.Package_description = source.baseInfo.packageDescription;
                target.Test_seq_mode = source.baseInfo.testSeqMode;
                target.Test_case_ids = source.baseInfo.testCaseIdList_str;
            }
            if (source.dbInfo != null)
            {
                target.Is_public = source.dbInfo.Is_public;
                target.Is_deleted = source.dbInfo.Is_deleted;
                target.Create_user = source.dbInfo.Create_user;
                target.Create_time = source.dbInfo.Create_time;
                target.Datachange_lasttime = source.dbInfo.Datachange_lasttime;
            }
        }
        #endregion
        #region Convert data modal to entity: TestPackage, TestPackageInfo
        private static void Convert(DataModel.TestPackage source, out TestPackage target)
        {
            target = new TestPackage();
            TestPackageInfo baseInfo = null;
            Convert(source, out baseInfo);
            target.baseInfo = baseInfo;
            target.dbInfo = new TestPackageDBInfo()
            {
                Package_id = source.Package_id,
                Is_public = source.Is_public,
                Is_deleted = source.Is_deleted,
                Create_user = source.Create_user,
                Create_time = source.Create_time,
                Datachange_lasttime = source.Datachange_lasttime
            };
        }
        private static void Convert(DataModel.TestPackage source, out TestPackageInfo target)
        {
            target = new TestPackageInfo()
            {
                packageName = source.Package_name,
                packageDescription = source.Package_description,
                testSeqMode = source.Test_seq_mode,
                testCaseIdList = string.IsNullOrEmpty(source.Test_case_ids) ? null : CommonType.Split<int>(source.Test_case_ids, ",", id => int.Parse(id)).ToArray(),
                packageId = source.Package_id,
                //collectionId = source.Collection_id
            };
        }
        #endregion

    }
}