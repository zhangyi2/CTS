using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace ctrip.Framework.ApplicationFx.CTS
{
    /// <summary>
    /// Summary description for ReadFile
    /// </summary>
    public class ReadFile : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/json";
            string json = readFile().Replace(@"\", @"\\");
            context.Response.Write(json);
            context.Response.End();
        }
        private string readFile()
        {
            string result = "";
            HttpFileCollection files = HttpContext.Current.Request.Files;
            if (files != null && files.Count > 0)
            {
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFile file = files[i];
                    string fileName = System.IO.Path.GetFileName(file.FileName);
                    try
                    {
                        using (StreamReader sr = new StreamReader(file.InputStream))
                        {
                            result = sr.ReadToEnd();
                        }
                    }
                    catch (Exception ex)
                    {
                        result = "{\"error\": \"" + ex.Message + "\"}";
                        if (file.InputStream != null) file.InputStream.Close();
                    }
                }
            }
            else
            {
                return "{\"error\": \"No file\"}";
            }
            return result;
        }
        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}