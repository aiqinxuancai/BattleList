using BattleList.Base;
using ElectronicObserver.Observer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
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
        private static string JSON_PATH_LIST = @".\Data\BattleList\list.json";
        private static string SQLITE_PATH_LIST = @".\Data\BattleList\list.sqlite";
        private Plugin plugin;


        private JObject m_lastStart;
        private ArrayList m_battleList;

        public void Initialize(Plugin plugin)
        {
            EasyLogOut.Write("Plugin:初始化BattleList...");

            this.plugin = plugin;
            
            LoadData();
            APIObserver o = APIObserver.Instance;

            o["api_req_map/start"].ResponseReceived += OnMapStart;
            o["api_req_map/next"].ResponseReceived += OnMapNext;
            o["api_req_sortie/battleresult"].ResponseReceived += OnBattleResult;
            o["api_req_combined_battle/battleresult"].ResponseReceived += OnCombinedBattleResult;
        }

        public void LoadData()
        {
            EasyLogOut.Write("Plugin:LoadData...");
            //try
            //{
	           // SQLiteConnection cn = new SQLiteConnection("data source=" + SQLITE_PATH_LIST);
	
	           // if (cn.State != System.Data.ConnectionState.Open)
	           // {
	           //     EasyLogOut.Write("Plugin:LoadData:SQLiteConnection");
	           //     //Time = DateTime.Now,
	           //     //    MapName = root.SelectToken("api_data.api_quest_name")?.ToObject<string>() + $"({mapId})",
	           //     //    MapPointId = point,
	           //     //    MapPointName = root.SelectToken("api_data.api_enemy_info.api_deck_name")?.ToObject<string>(),
	           //     //    NewShipName = shipName,
	           //     //    WinRankId = root.SelectToken("api_data.api_win_rank")?.ToObject<int>(),
	           //     //    WinRank = root.SelectToken("api_data.api_win_rank")?.ToObject<string>(),
	           //     //    DeckName = root.SelectToken("api_data.api_enemy_info.api_deck_name")?.ToObject<string>()
	
	           //     cn.Open();
	           //     SQLiteCommand cmd = new SQLiteCommand();
	           //     cmd.Connection = cn;
	           //     cmd.CommandText = "CREATE TABLE IF NOT EXISTS battlelist(id int, time DATETIME, score int, mapName TEXT, mapPointId int, mapPointName TEXT, newShipName TEXT, winRankId int, winRank TEXT, deckName TEXT)";
	           //     cmd.ExecuteNonQuery();
	           // }
            //}
            //catch (System.Exception ex)
            //{
            //    EasyLogOut.Write(ex);
            //}

            EasyLogOut.Write("Plugin:LoadData:LoadFromJson");
            m_battleList = new ArrayList();
            if (File.Exists(JSON_PATH_LIST))
            {
                
                m_battleList = JsonConvert.DeserializeObject<ArrayList>(File.ReadAllText(JSON_PATH_LIST));
            }
        }

        public void SaveData()
        {
            File.WriteAllText(JSON_PATH_LIST, JsonConvert.SerializeObject(m_battleList));
        }

        private void OnMapStart(string apiname, dynamic data)
        {
            EasyLogOut.Write("Plugin:OnMapStart");
            Codeplex.Data.DynamicJson json = data;
            //EasyLogOut.Write(data);
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
           // EasyLogOut.Write(data);
            try
            {
                JObject root = JObject.Parse(json.ToString());
                var next = root;
                if (m_lastStart != null)
                {
                    m_lastStart.Merge(next); //合并点数据
                    EasyLogOut.Write("合并Next完成");
                    EasyLogOut.Write(m_lastStart.ToString(Formatting.Indented));
                }
            }
            catch (System.Exception ex)
            {
                EasyLogOut.Write(ex);
            }
        }

        private void OnBattleResult(string apiname, dynamic data)
        {
            EasyLogOut.Write("Plugin:OnBattleResult");
            Codeplex.Data.DynamicJson json = data;
            //EasyLogOut.Write(data);
            try
            {
                JObject root = JObject.Parse(json.ToString());
                SaveBattleResult(root);
            }
            catch (System.Exception ex)
            {
                EasyLogOut.Write(data);
            }
        }

        private void OnCombinedBattleResult(string apiname, dynamic data)
        {
            EasyLogOut.Write("Plugin:OnCombinedBattleResult");
            Codeplex.Data.DynamicJson json = data;
            //EasyLogOut.Write(data);
            try
            {
                JObject root = JObject.Parse(json.ToString());
                SaveBattleResult(root);
            }
            catch (System.Exception ex)
            {
                EasyLogOut.Write(ex);
            }
        }

        private bool SaveBattleResult(JObject root)
        {

            try
            {
                if (m_lastStart == null)
                {
                    return false;
                }

                Debug.WriteLine("战斗结果");
            
                string shipName = root.SelectToken("api_get_ship.api_ship_name")?.ToObject<string>();
                int point = m_lastStart["api_no"].Value<int>();
                string mapId = m_lastStart["api_maparea_id"] + "-" + m_lastStart["api_mapinfo_no"]; //3-2
                bool isBoss = m_lastStart["api_no"].Value<int>() == m_lastStart["api_bosscell_no"].Value<int>();


                string mapName = root.SelectToken("api_quest_name")?.ToObject<string>() + $"({mapId})";
                string winRank = root.SelectToken("api_win_rank")?.ToObject<string>();
                string deckName = root.SelectToken("api_enemy_info.api_deck_name")?.ToObject<string>();

                string mapPointName = isBoss ? point + "(Boss)" : point.ToString();


                BattleListCell data = new BattleListCell()
                {
                    Time = DateTime.Now,
                    MapName = mapName,
                    MapPointId = point,
                    MapPointName = mapPointName,
                    NewShipName = shipName,

                    WinRank = winRank,
                    DeckName = deckName
                };

                m_battleList.Add(data);
                SaveData();
                return true;
            }
            catch (System.Exception ex)
            {
                EasyLogOut.Write(ex);
                return false;
            }
        }
        


    }
}
