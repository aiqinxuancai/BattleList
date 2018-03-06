using BattleList.Base;
using ElectronicObserver.Observer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleList
{



    class BattleList
    {
        private static readonly BattleList instance = new BattleList();
        public static BattleList Instance
        {
            get { return instance; }
        }
        private static string JSON_PATH = @".\Data\BattleList";
        private Plugin plugin;


        private JObject m_lastStart, m_lastBattleResult;


        public void Initialize(Plugin plugin)
        {
            EasyLogOut.Write("Plugin:初始化BattleList...");

            this.plugin = plugin;

            APIObserver o = APIObserver.Instance;

            o["api_req_map/start"].ResponseReceived += OnMapStart;
            o["api_req_map/next"].ResponseReceived += OnMapNext;
            o["api_req_sortie/battleresult"].ResponseReceived += OnBattleResult;
            o["api_req_combined_battle/battleresult"].ResponseReceived += OnCombinedBattleResult;
        }


        private void OnMapStart(string apiname, dynamic data)
        {
            EasyLogOut.Write("Plugin:OnMapStart");
            Codeplex.Data.DynamicJson json = data;
            EasyLogOut.Write(data);
            try
            {
                JObject root = JObject.Parse(json.ToString());
                m_lastStart = root;
            }
            catch (System.Exception ex)
            {
                EasyLogOut.Write(ex);
            }
        }

        private void OnMapNext(string apiname, dynamic data)
        {
            EasyLogOut.Write("Plugin:OnMapNext");
            Codeplex.Data.DynamicJson json = data;
            EasyLogOut.Write(data);
            try
            {
                JObject root = JObject.Parse(json.ToString());
                var next = root;
                if (m_lastStart != null)
                {
                    m_lastStart.Merge(next);//合并点数据
                }
            }
            catch (System.Exception ex)
            {
                EasyLogOut.Write(ex);
            }
        }

        private void OnBattleResult(string apiname, dynamic data)
        {
            EasyLogOut.Write("Plugin:OnBattleResultCompleted");
            Codeplex.Data.DynamicJson json = data;
            EasyLogOut.Write(data);
            try
            {
                JObject root = JObject.Parse(json.ToString());
                File.WriteAllText(JSON_PATH + @"\BattleResult" + DateTime.Now.Ticks.ToString() + ".json", root.ToString());
            }
            catch (System.Exception ex)
            {
                File.WriteAllText(JSON_PATH + @"\BattleResult" + DateTime.Now.Ticks.ToString() + ".json", ex.ToString());
            }
        }

        private void OnCombinedBattleResult(string apiname, dynamic data)
        {
            EasyLogOut.Write("Plugin:OnCombinedBattleResultCompleted");
            Codeplex.Data.DynamicJson json = data;
            EasyLogOut.Write(data);
            try
            {
                JObject root = JObject.Parse(json.ToString());
                File.WriteAllText(JSON_PATH + @"\CombinedBattleResult" + DateTime.Now.Ticks.ToString() + ".json", root.ToString());
            }
            catch (System.Exception ex)
            {
                File.WriteAllText(JSON_PATH + @"\CombinedBattleResult" + DateTime.Now.Ticks.ToString() + ".json", ex.ToString());
            }
        }

        private void SaveBattleResult(JObject root)
        {

        }
        


    }
}
