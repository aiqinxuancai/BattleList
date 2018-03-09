using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleList
{
    public class BattleListCell
    {
        public DateTime Time { get; set; }
        public string MapName { get; set; }
        public int MapPointId { get; set; }

        /// <summary>
        /// 类似 1(Boss) 显示
        /// </summary>
        public string MapPointName { get; set; }
        public string WinRank { get; set; }

        public bool IsBoos { get; set; }

        /// <summary>
        /// 掉落的新船的名字
        /// </summary>
        public string NewShipName { get; set; }
        /// <summary>
        /// 敌方舰队名称
        /// </summary>
        public string DeckName { get; set; }





    }
}
