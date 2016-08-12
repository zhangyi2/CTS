using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;

namespace ctrip.Framework.ApplicationFx.CTS
{
    public static class CommonType
    {
        /// <summary>
        /// 将文本信息[name=value;]转换为名值对列表
        /// </summary>
        /// <param name="strContent">文本信息</param>
        /// <returns></returns>
        public static NameValueCollection NameValueSplit(string strContent)
        {
            return NameValueSplit(strContent, ';', '=');
        }

        /// <summary>
        /// 将文本信息转换为名值对列表
        /// </summary>
        /// <param name="strContent">文本信息</param>
        /// <param name="splitChar">分隔符</param>
        /// <param name="equalChar">名值对操作符</param>
        /// <returns></returns>
        public static NameValueCollection NameValueSplit(string strContent, char splitChar, char equalChar)
        {
            NameValueCollection nvList = new NameValueCollection();
            string[] strList = strContent.Split(new char[] { splitChar }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string nv in strList)
            {
                string[] strNV = nv.Split(new char[] { equalChar }, 2, StringSplitOptions.None);
                if (strNV.Length == 2 && !string.IsNullOrEmpty(strNV[0]))
                    nvList.Add(strNV[0], strNV[1]);
            }
            return nvList;
        }

        /// <summary>
        /// 将文本信息转换为名值对Hashtable
        /// </summary>
        /// <param name="strContent">文本信息</param>
        /// <param name="splitStr">分隔符</param>
        /// <param name="equalStr">名值对操作符</param>
        /// <returns></returns>
        public static Hashtable NameValueSplit(string strContent, string splitStr, string equalStr)
        {
            Hashtable nvList = new Hashtable();
            string[] strList = strContent.Split(new string[] { splitStr }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string nv in strList)
            {
                string[] strNV = nv.Split(new string[] { equalStr }, 2, StringSplitOptions.None);
                if (strNV.Length == 2 && !string.IsNullOrEmpty(strNV[0]))
                    nvList.Add(strNV[0], strNV[1]);
            }
            return nvList;
        }

        /// <summary>
        /// 根据参数列表生成URL参数表示
        /// </summary>
        /// <param name="paramList"></param>
        /// <returns>返回""或以?开头</returns>
        public static string GenUrlParams(Hashtable paramList)
        {
            string url = "";
            foreach (object key in paramList.Keys)
            {
                url += string.Format("&{0}={1}", key, paramList[key]);
            }
            return string.IsNullOrEmpty(url) ? "" : ("?" + url.Substring(1));
        }

        /// <summary>
        /// 生成URL字符串
        /// </summary>
        /// <param name="directUrl">目标地址</param>
        /// <param name="paramList">参数</param>
        /// <returns></returns>
        public static string GenUrl(string directUrl, Hashtable paramList)
        {
            return directUrl + GenUrlParams(paramList);
        }

        /// <summary>
        /// 将字符串数组拼接成以指定字符串连接的文本
        /// </summary>
        /// <param name="linkStr">连接字符串</param>
        /// <param name="withTail">是否以连接字符串结尾</param>
        /// <param name="strList">字符串数组</param>
        /// <returns></returns>
        public static string StringBuild(string linkStr, bool withTail, params string[] strList)
        {
            if (strList == null) return "";

            string text = "";
            foreach (string line in strList)
            {
                text += line + linkStr;
            }
            if (text != "" && !withTail) text = text.Substring(0, text.Length - linkStr.Length);
            return text;
        }

        /// <summary>
        /// 将字符串数组拼接成以指定字符串连接的文本(默认以连接字符串结尾)
        /// </summary>
        /// <param name="linkStr">连接字符串</param>
        /// <param name="strList">字符串数组</param>
        /// <returns></returns>
        public static string StringBuild(string linkStr, params string[] strList)
        {
            return StringBuild(linkStr, false, strList);
        }

        public delegate string ObjectToString<T>(T obj);
        /// <summary>
        /// 将任意数组拼接成以指定字符串连接的文本
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="linkStr">连接字符串</param>
        /// <param name="withTail">是否以连接字符串结尾</param>
        /// <param name="objList">对象数组</param>
        /// <param name="objToString">对象ToString自定义方法</param>
        /// <returns></returns>
        public static string StringBuild<T>(string linkStr, bool withTail, IList<T> objList, ObjectToString<T> objToString)
        {
            if (objList == null) return "";
            string text = "";
            foreach (T obj in objList)
            {
                text += objToString(obj) + linkStr;
            }
            if (text != "" && !withTail) text = text.Substring(0, text.Length - linkStr.Length);
            return text;
        }

        public delegate T StringToObject<T>(string str);
        /// <summary>
        /// 将字符串分解为指定对象的数组形式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="text"></param>
        /// <param name="linkStr">连接字符串</param>
        /// <param name="stringToObject"></param>
        /// <returns></returns>
        public static List<T> Split<T>(string text, string linkStr, StringToObject<T> stringToObject)
        {
            if (stringToObject == null)
            {
                return null;
            }
            else
            {
                List<T> list = new List<T>();
                foreach (string str in text.Split(new string[] { linkStr }, StringSplitOptions.RemoveEmptyEntries))
                {
                    list.Add(stringToObject(str));
                }
                return list;
            }
        }

        /// <summary>
        /// 将字符串数组拼接成以换行符连接的文本
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static string TextLinesBuild(params string[] lines)
        {
            return StringBuild(System.Environment.NewLine, true, lines);
        }

        /// <summary>
        /// 将字符串数组拼接成以Web换行符连接的Html内容流
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static string WebLinesBuild(params string[] lines)
        {
            return StringBuild("<br/>", true, lines);
        }
    }
}