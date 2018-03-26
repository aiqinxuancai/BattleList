using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace BattleListMainWindow.Service
{
    class HttpHelper
    {
        private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 9.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

        public static string PostWebRequest(string postUrl, string paramData)
        {
            string ret = string.Empty;
            try
            {
                CookieContainer cc = new CookieContainer();
                byte[] byteArray = Encoding.UTF8.GetBytes(paramData); //转化

                //System.Diagnostics.Debug.WriteLine("byteArray:" + byteArray.Length);

                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.CookieContainer = cc;
                webReq.Method = "POST";
                webReq.Proxy = null; //new WebProxy();
                webReq.ContentType = "application/x-www-form-urlencoded";
                webReq.UserAgent = DefaultUserAgent;
                webReq.ContentLength = byteArray.Length;
                webReq.Timeout = 30000;

                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                newStream.Close();

                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);

                ret = sr.ReadToEnd();

                sr.Close();
                //webReq.Abort();
                response.Close();
                newStream.Close();
            }
            catch (Exception ex)
            {
                EasyLogOut.Write(ex, LogLevel.Error);
            }
            return ret;
        }
    }
}
