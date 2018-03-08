using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleListMainWindow
{
    public class BattleListManager
    {
        private static string JSON_PATH_LIST = @".\list.json";

        public ArrayList m_battleList;


        public BattleListManager ()
        {
            
        }


        public void LoadData()
        {
            m_battleList = new ArrayList();
            if (File.Exists(JSON_PATH_LIST))
            {
                m_battleList = JsonConvert.DeserializeObject<ArrayList>(File.ReadAllText(JSON_PATH_LIST));
            }
        }

    }
}
