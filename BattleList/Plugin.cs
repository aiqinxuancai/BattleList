using ElectronicObserver.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserver.Data;
using ElectronicObserver.Utility;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using ElectronicObserver.Window;
using System.Windows.Forms;
using BattleList.Base;
using System.Diagnostics;

namespace BattleList
{
    
    public class Plugin : ElectronicObserver.Window.Plugins.DialogPlugin
    {
        public override string MenuTitle => "BattleList";
        public override string Version { get { return "1.0.0.2"; } }


        private static readonly string JSON_PATH = @".\Data\BattleList";

        private static readonly string MAINEXE_PATH = @".\Plugins\BattleListMainWindow.exe";

        public Plugin()
        {
            EasyLogOut.Write("Plugin:加载中...");

            if (Directory.Exists(JSON_PATH) == false)
            {
                Directory.CreateDirectory(JSON_PATH);
            }

            if (Directory.Exists(@".\x64") == false)
            {
                Directory.CreateDirectory(@".\x64");
            }
            if (Directory.Exists(@".\x86") == false)
            {
                Directory.CreateDirectory(@".\x86");
            }

            if (Directory.Exists(@".\Plugins\x64") == false)
            {
                Directory.CreateDirectory(@".\Plugins\x64");
            }

            if (Directory.Exists(@".\Plugins\x86") == false)
            {
                Directory.CreateDirectory(@".\Plugins\x86");
            }

            EasyLogOut.Write("Plugin:释放SQLite.Interop.dll");

            if (File.Exists(@".\x86\SQLite.Interop.dll") == false)
            {
                File.WriteAllBytes(@".\x86\SQLite.Interop.dll", Properties.Resources.SQLite_x86_Interop);
            }
            if (File.Exists(@".\x64\SQLite.Interop.dll") == false)
            {
                File.WriteAllBytes(@".\x64\SQLite.Interop.dll", Properties.Resources.SQLite_x64_Interop);
            }

            EasyLogOut.Write("Plugin:释放Plugins目录的SQLite.Interop.dll");
            if (File.Exists(@".\Plugins\x86\SQLite.Interop.dll") == false)
            {
                File.WriteAllBytes(@".\Plugins\x86\SQLite.Interop.dll", Properties.Resources.SQLite_x86_Interop);
            }
            if (File.Exists(@".\Plugins\x64\SQLite.Interop.dll") == false)
            {
                File.WriteAllBytes(@".\Plugins\x64\SQLite.Interop.dll", Properties.Resources.SQLite_x64_Interop);
            }

            Task.Factory.StartNew(() => BattleList.Instance.Initialize(this));
        }

        public override bool RunService(FormMain main)
        {
            EasyLogOut.Write("Plugin:RunService调用");
            return true;
        }

        public override Form GetToolWindow()
        {
            EasyLogOut.Write("Plugin:GetToolWindow调用");

            if (Directory.Exists(MAINEXE_PATH) == false)
            {
                Process.Start(MAINEXE_PATH);
            }

            return null;
        }
    }
}
