using BattleList.Base;
using BattleList.Service;
using ElectronicObserver.Observer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
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
        
        private Plugin plugin;

        private SQLiteSaveList sqliteSaveList;


        private JObject m_lastStart;
        private ArrayList m_battleList;

        public void Initialize(Plugin plugin)
        {
            EasyLogOut.Write("Plugin:初始化BattleList...");

            this.plugin = plugin;

            sqliteSaveList = new SQLiteSaveList();
            sqliteSaveList.Init();

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
            //EasyLogOut.Write(data);
            try
            {
                JObject root = JObject.Parse(json.ToString());
                var next = root;
                if (m_lastStart != null)
                {
                    m_lastStart.Merge(next); //合并点数据
                    //EasyLogOut.Write("合并Next完成");
                    //EasyLogOut.Write(m_lastStart.ToString(Formatting.Indented));
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
                EasyLogOut.Write(ex);
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
                EasyLogOut.Write(root);

                string shipName = root.SelectToken("api_get_ship.api_ship_name")?.ToObject<string>();
                int point = m_lastStart["api_no"].Value<int>();
                string mapId = m_lastStart["api_maparea_id"] + "-" + m_lastStart["api_mapinfo_no"]; //3-2
                bool isBoss = m_lastStart["api_no"].Value<int>() == m_lastStart["api_bosscell_no"].Value<int>();


                

                string mapName = root.SelectToken("api_quest_name")?.ToObject<string>() + $"({mapId})";
                string winRank = root.SelectToken("api_win_rank")?.ToObject<string>();
                string deckName = root.SelectToken("api_enemy_info.api_deck_name")?.ToObject<string>();

                string mapPointName = isBoss ? point + "(Boss)" : point.ToString();

                


                dynamic json = Codeplex.Data.DynamicJson.Parse(root.ToString());
                if ((int)json.api_get_flag[2] != 0)
                {
                    var slotItemId = (int)json.api_get_slotitem.api_slotitem_id;
                    ElectronicObserver.Data.EquipmentDataMaster eq = ElectronicObserver.Data.KCDatabase.Instance.MasterEquipments[slotItemId];
                    shipName = shipName == string.Empty ? eq.Name : shipName + " + " + eq.Name;
                }

                ElectronicObserver.Data.KCDatabase db = ElectronicObserver.Data.KCDatabase.Instance;
                ElectronicObserver.Data.Battle.BattleManager bm = db.Battle;
                string fullBattleData = ElectronicObserver.Data.Battle.Detail.BattleDetailDescriptor.GetBattleDetail(bm);

                BattleListCell data = new BattleListCell()
                {
                    Time = DateTime.Now,
                    MapName = mapName,
                    MapPointId = point,
                    MapPointName = mapPointName,
                    NewShipName = shipName,
                    IsBoos = isBoss,
                    WinRank = winRank,
                    DeckName = deckName,
                    FullBattleData = fullBattleData
                };

                //输出sqlite格式
                sqliteSaveList.AddData(data);

                ////输出Json格式
                //m_battleList.Insert(0, data);
                //SaveData();


                return true;
            }
            catch (System.Exception ex)
            {
                EasyLogOut.Write(ex);
                return false;
            }
        }

        void OnError(object currentObject, Newtonsoft.Json.Serialization.ErrorContext errorContext)
        {

        }

    }
}
