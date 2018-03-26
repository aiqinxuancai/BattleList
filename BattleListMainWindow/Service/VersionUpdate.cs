using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BattleListMainWindow.Service
{
    class VersionUpdate
    {
        public static Version m_version;

        public static JObject m_updateCheckJson;

        static VersionUpdate()
        {
            m_version = new Version(1, 0, 0, 0);
        }




        public static bool HasUpdate()
        {
            bool canUpdate = false;
            try
            {
                string data = PostUpdate();
	            JObject root = JObject.Parse(data);
                m_updateCheckJson = root;


                canUpdate = root["canUpdate"].ToObject<bool>();
            }
            catch (System.Exception ex)
            {
                EasyLogOut.Write(ex, LogLevel.Error);
            }


            return canUpdate;
        }




        public static string PostUpdate()
        {
            WebClient webClient = new WebClient();

            string postData = "&version=" + m_version.ToString() + "&eoversion=" + GetElectronicObserverVersion();
            NameValueCollection nameValue = new NameValueCollection();
            nameValue.Set("version", m_version.ToString());
            nameValue.Set("eoversion", GetElectronicObserverVersion());
            var data = webClient.UploadValues("http://ver.moehex.com/battlelist.php", "POST", nameValue);
            var dataString = System.Text.Encoding.UTF8.GetString(data);
            Thread.Sleep(1000 * 5);
            return dataString;
        }

        private static string GetElectronicObserverVersion()
        {
            string version = string.Empty;
            try
            {
                var fileName = @".\ElectronicObserver.exe";
                if (File.Exists(fileName) == false)
                {
                    fileName = @"..\ElectronicObserver.exe";
                }
                FileVersionInfo fileVersion = FileVersionInfo.GetVersionInfo(fileName);
                version = String.Format("{0}.{1}.{2}.{3}", fileVersion.FileMajorPart, fileVersion.FileMinorPart, fileVersion.FileBuildPart, fileVersion.FilePrivatePart);
            }
            catch (Exception ex)
            {
                version = "";
                EasyLogOut.Write(ex, LogLevel.Error);
            }
            return version;
        }
    
    }
}
