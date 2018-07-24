using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleListMainWindow.Service
{
    class SQLiteShowList
    {
        private static string SQLITE_PATH_LIST = @".\Data\BattleList\list.sqlite";

        SQLiteConnection m_sqliteConnection;

        public void Init()
        {
            try
            {
                if (File.Exists(@".\list.sqlite")) 
                {
                    m_sqliteConnection = new SQLiteConnection(@"data source=.\list.sqlite");
                }
                else if(File.Exists(SQLITE_PATH_LIST))
                {
                    m_sqliteConnection = new SQLiteConnection("data source=" + SQLITE_PATH_LIST);
                }
                else if(File.Exists(@"..\Data\BattleList\list.sqlite"))
                {
                    m_sqliteConnection = new SQLiteConnection("data source=" + @"..\Data\BattleList\list.sqlite");
                }

                if (m_sqliteConnection.State != System.Data.ConnectionState.Open)
                {
                    m_sqliteConnection.Open();
                }
            }
            catch (System.Exception ex)
            {
                EasyLogOut.Write(ex, LogLevel.Error);
            }
        }

        public DataView LoadData()
        {
            try
            {
	            var con = "SELECT * FROM battlelist ORDER BY time DESC";
	            DataSet dataSet = new DataSet();
	            SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(con, m_sqliteConnection);
	            dataAdapter.Fill(dataSet);
	            var table = dataSet.Tables[0].DefaultView;
	            return table;
            }
            catch (System.Exception ex)
            {
                EasyLogOut.Write(ex, LogLevel.Error);
                return null;
            }
        }



    }
}
