using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BattleListMainWindow
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        App()
        {
            //检查并复制上级目录中的SQLITE文件到当前目录

            if (File.Exists(@"..\x86\SQLite.Interop.dll"))
            {
                if (Directory.Exists(@".\x86") == false)
                {
                    Directory.CreateDirectory(@".\x86");
                }

                File.Copy(@"..\x86\SQLite.Interop.dll", @".\x86\SQLite.Interop.dll");
            }

            if (File.Exists(@"..\x64\SQLite.Interop.dll"))
            {
                if (Directory.Exists(@".\x64") == false)
                {
                    Directory.CreateDirectory(@".\x64");
                }

                File.Copy(@"..\x64\SQLite.Interop.dll", @".\x64\SQLite.Interop.dll");
            }
        }
    }
}
