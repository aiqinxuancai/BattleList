using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleList.Service
{
    public class MapPointService
    {

        static JObject m_jsonPoint;


        static MapPointService()
        {
            m_jsonPoint = JObject.Parse(System.Text.Encoding.Default.GetString(Properties.Resources.map));
        }

        public static string GetMapPointName(int mapAreaId, int mapInfoId, int point)
        {
            JToken token = m_jsonPoint.SelectToken($@"$.data.{mapAreaId}-{mapInfoId}.route.{point}");
            if (token != null)
            {
                Debug.WriteLine("找到point点");
                var tokenArray = (JArray)token;
                if (tokenArray.Count >= 2)
                {
                    return tokenArray[tokenArray.Count - 1].ToObject<string>();
                }
            }
            return string.Empty;
        }



    }



}
