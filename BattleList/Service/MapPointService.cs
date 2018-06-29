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

        static JObject json;


        static MapPointService()
        {
            json = JObject.Parse(System.Text.Encoding.Default.GetString(Properties.Resources.map));
        }

        public static string GetMapPointName(int mapAreaId, int mapInfoId, int point)
        {
            JToken token = json.SelectToken($@"$.data.{mapAreaId}-{mapInfoId}.route.{point}").ToString();
            if (token != null)
            {
                Debug.WriteLine("Find Token");
                if (token.Type == JTokenType.Array)
                {
                    Debug.WriteLine("Token is Array");
                    var tokenArray = (JArray)token;
                    if (tokenArray.Count == 2)
                    {
                        Debug.WriteLine("Token Count 2");
                        return token[1].ToObject<string>();
                    }
                }
            }
            return string.Empty;
        }


    }



}
