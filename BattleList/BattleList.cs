using ElectronicObserver.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserver.Data;
using ElectronicObserver.Utility;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;

namespace BattleList
{
    public class BattleList : ElectronicObserver.Window.Plugins.DialogPlugin
    {
        public override string MenuTitle => "BattleList";
        public override string Version { get { return "<BUILD_VERSION>"; } }


        private static string JSON_PATH = @".\Data\BattleList";

        public override System.Windows.Forms.Form GetToolWindow()
        {
            throw new NotImplementedException();
        }
        public BattleList()
        {
            if (Directory.Exists(JSON_PATH) == false)
            {
                Directory.CreateDirectory(JSON_PATH);
            }

            APIObserver.Instance["api_req_sortie/battleresult"].RequestReceived += OnBattleResultCompleted;
            APIObserver.Instance["api_req_combined_battle/battleresult"].RequestReceived += OnCombinedBattleResultCompleted;

        }
        private void OnBattleResultCompleted(string apiname, dynamic data)
        {
            

            try
            {
	            JObject root = JsonConvert.SerializeObject(data);
	            File.WriteAllText(JSON_PATH + @"\CombinedBattleResult" + DateTime.Now.Ticks.ToString() + ".json", root.ToString());
            }
            catch (System.Exception ex)
            {
                File.WriteAllText(JSON_PATH + @"\CombinedBattleResult" + DateTime.Now.Ticks.ToString() + ".json", ex.ToString());
            }
           
        }

        private void OnCombinedBattleResultCompleted(string apiname, dynamic data)
        {


            try
            {
	            JObject root = JsonConvert.SerializeObject(data);
	            File.WriteAllText(JSON_PATH + @"\BattleResult" + DateTime.Now.Ticks.ToString() + ".json", root.ToString());
            }
            catch (System.Exception ex)
            {
                File.WriteAllText(JSON_PATH + @"\BattleResult" + DateTime.Now.Ticks.ToString() + ".json", ex.ToString());
            }
        }


    }
}
