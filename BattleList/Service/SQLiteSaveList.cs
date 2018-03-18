using BattleList.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleList.Service
{
    class SQLiteSaveList
    {

        private static string SQLITE_PATH_LIST = @".\Data\BattleList\list.sqlite";

        SQLiteConnection m_sqliteConnection;


        public void Init()
        {
            try
            {
                m_sqliteConnection = new SQLiteConnection("data source=" + SQLITE_PATH_LIST);

                if (m_sqliteConnection.State != System.Data.ConnectionState.Open)
                {
                    EasyLogOut.Write("Plugin:LoadData:SQLiteConnection");
                    m_sqliteConnection.Open();
                    SQLiteCommand cmd = new SQLiteCommand();
                    cmd.Connection = m_sqliteConnection;
                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS battlelist(id INTEGER PRIMARY KEY AUTOINCREMENT, time DATETIME, mapName TEXT, mapPointId int, isBoss BOOLEAN, mapPointName TEXT, newShipName TEXT, winRank TEXT, deckName TEXT, fullBattleData TEXT)";
                    cmd.ExecuteNonQuery();
                    
                }
            }
            catch (System.Exception ex)
            {
                EasyLogOut.Write(ex);
            }
        }

        public void AddData(BattleListCell battleListCell)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = m_sqliteConnection;
            cmd.CommandText = "INSERT INTO battlelist(time,mapName,mapPointId,isBoss,mapPointName,newShipName,winRank,deckName,fullBattleData) " +
                                              "VALUES(@time,@mapName,@mapPointId,@isBoss,@mapPointName,@newShipName,@winRank,@deckName,@fullBattleData)";
            cmd.Parameters.Add("time", DbType.DateTime).Value = battleListCell.Time;
            cmd.Parameters.Add("mapName", DbType.String).Value = battleListCell.MapName;
            cmd.Parameters.Add("mapPointId", DbType.Int32).Value = battleListCell.MapPointId;
            cmd.Parameters.Add("isBoss", DbType.Boolean).Value = battleListCell.IsBoos;
            cmd.Parameters.Add("mapPointName", DbType.String).Value = battleListCell.MapPointName;
            cmd.Parameters.Add("newShipName", DbType.String).Value = battleListCell.NewShipName;
            cmd.Parameters.Add("winRank", DbType.String).Value = battleListCell.WinRank;
            cmd.Parameters.Add("deckName", DbType.String).Value = battleListCell.DeckName;
            cmd.Parameters.Add("fullBattleData", DbType.String).Value = battleListCell.FullBattleData;


            try
            {
            	cmd.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                EasyLogOut.Write(ex);
            }




        }





    }
}
