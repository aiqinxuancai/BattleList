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
                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS battlelist(id INTEGER PRIMARY KEY AUTOINCREMENT, Time DATETIME, MapName TEXT, MapPointId int, IsBoss BOOLEAN, MapPointName TEXT, NewShipName TEXT, WinRank TEXT, DeckName TEXT, FullBattleData TEXT)";
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
            cmd.CommandText = "INSERT INTO battlelist(Time,MapName,MapPointId,IsBoss,MapPointName,NewShipName,WinRank,DeckName,FullBattleData) " +
                                              "VALUES(@Time,@MapName,@MapPointId,@IsBoss,@MapPointName,@NewShipName,@WinRank,@DeckName,@FullBattleData)";
            cmd.Parameters.Add("Time", DbType.DateTime).Value = battleListCell.Time;
            cmd.Parameters.Add("MapName", DbType.String).Value = battleListCell.MapName;
            cmd.Parameters.Add("MapPointId", DbType.Int32).Value = battleListCell.MapPointId;
            cmd.Parameters.Add("IsBoss", DbType.Boolean).Value = battleListCell.IsBoos;
            cmd.Parameters.Add("MapPointName", DbType.String).Value = battleListCell.MapPointName;
            cmd.Parameters.Add("NewShipName", DbType.String).Value = battleListCell.NewShipName;
            cmd.Parameters.Add("WinRank", DbType.String).Value = battleListCell.WinRank;
            cmd.Parameters.Add("DeckName", DbType.String).Value = battleListCell.DeckName;
            cmd.Parameters.Add("FullBattleData", DbType.String).Value = battleListCell.FullBattleData;


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
