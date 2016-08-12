using ctrip.Framework.ApplicationFx.CTS.Loghelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Collections;

namespace ctrip.Framework.ApplicationFx.CTS
{
    public static class CacheManager
    {
        public static Hashtable CacheSet = new Hashtable();
        public static Hashtable CacheSet_Locked = new Hashtable();
        public static object Get(string cacheName, string key)
        {
            if (string.IsNullOrEmpty(key)) key = "Unknown";
            Hashtable cacheList = GetCacheList(cacheName);
            return cacheList[key];
        }
        public static void Add(string cacheName, string key, object val)
        {
            if (string.IsNullOrEmpty(cacheName)) cacheName = "Unknown";
            if (string.IsNullOrEmpty(key)) key = "Unknown";
            Hashtable cacheList = GetCacheList(cacheName);
            cacheList.Add(key, val);
            //TODO: 定期清理
        }
        public static void Remove(string cacheName, string key)
        {
            if (string.IsNullOrEmpty(cacheName)) cacheName = "Unknown";
            if (string.IsNullOrEmpty(key)) key = "Unknown";
            Hashtable cacheList = GetCacheList(cacheName);
            cacheList.Remove(key);
        }
        public static void RemoveAll(string cacheName)
        {
            if (string.IsNullOrEmpty(cacheName)) cacheName = "Unknown";
            Hashtable cacheList = GetCacheList(cacheName);
            cacheList.Clear();
            CacheSet.Remove(cacheName);
        }
        public static Hashtable GetCacheList(string cacheName)
        {
            if (string.IsNullOrEmpty(cacheName)) cacheName = "Unknown";
            Hashtable list = CacheSet[cacheName] as Hashtable;
            if (list == null)
            {
                list = new Hashtable();
                CacheSet.Add(cacheName, list);
            }
            return list;
        }

        public static bool LockWithWait(string cacheName, string key, int waitTime, int tryCount)
        {
            bool isLocked = CacheManager.Lock(cacheName, key);
            while (!isLocked && --tryCount > 0)
            {
                for (int i = 0; i < waitTime; i++) isLocked = false;
                isLocked = CacheManager.Lock(cacheName, key);
            }
            return isLocked;
        }
        public static bool Lock(string cacheName)
        {
            return Lock(cacheName, "ALL");
        }
        public static bool Lock(string cacheName, string key)
        {
            if (string.IsNullOrEmpty(cacheName)) cacheName = "Unknown";
            if (string.IsNullOrEmpty(key)) key = "Unknown";
            if (CacheSet_Locked[cacheName + "__" + key] != null && (bool)CacheSet_Locked[cacheName + "__" + key] == true) return false;
            else{
                CacheSet_Locked[cacheName + "__" + key] = true;
                return true;
            }
        }
        public static bool Unlock(string cacheName)
        {
            return Unlock(cacheName, "ALL");
        }
        public static bool Unlock(string cacheName, string key)
        {
            if (string.IsNullOrEmpty(cacheName)) cacheName = "Unknown";
            if (string.IsNullOrEmpty(key)) key = "Unknown";
            if (CacheSet_Locked[cacheName + "__" + key] == null || (bool)CacheSet_Locked[cacheName + "__" + key] != true) return false;
            else
            {
                CacheSet_Locked[cacheName + "__" + key] = false;
                return true;
            }
        }
        public static bool UnlockAll(string cacheName, bool isClear)
        {
            if (string.IsNullOrEmpty(cacheName)) cacheName = "Unknown";
            IDictionaryEnumerator id = CacheSet_Locked.GetEnumerator();
            List<string> keyList = new List<string>();
            foreach (string key in CacheSet_Locked.Keys)
            {
                if (key == cacheName || key.StartsWith(cacheName + "__"))
                {
                    keyList.Add(key);
                }
            }
            for (int i = keyList.Count - 1; i >= 0; i--)
            {
                if (isClear) CacheSet_Locked.Remove(keyList[i]);
                else CacheSet_Locked[keyList[i]] = false;
            }
            return true;
        }
    }
}