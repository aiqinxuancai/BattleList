using BattleList.Base;
using ElectronicObserver.Observer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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


        public void Initialize(Plugin plugin)
        {
            EasyLogOut.Write("Plugin:初始化BattleList...");

            this.plugin = plugin;

            APIObserver o = APIObserver.Instance;
            o["api_req_sortie/battleresult"].ResponseReceived += OnBattleResultCompleted;
            o["api_req_combined_battle/battleresult"].ResponseReceived += OnCombinedBattleResultCompleted;
        }


        private void OnBattleResultCompleted(string apiname, dynamic data)
        {
            EasyLogOut.Write("Plugin:OnBattleResultCompleted");

            Codeplex.Data.DynamicJson json = data;

            EasyLogOut.Write(data.GetType());
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

        private void OnCombinedBattleResultCompleted(string apiname, dynamic data)
        {
            EasyLogOut.Write("Plugin:OnCombinedBattleResultCompleted");

            Codeplex.Data.DynamicJson json = data;

            EasyLogOut.Write(data.GetType());
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

    }
}
