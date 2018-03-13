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
        public override string Version { get { return "<BUILD_VERSION>"; } }


        private static string JSON_PATH = @".\Data\BattleList";

        private static string MAINEXE_PATH = @".\Plugins\BattleListMainWindow.exe";

        public Plugin()
        {
            EasyLogOut.Write("Plugin:加载中...");

            if (Directory.Exists(JSON_PATH) == false)
            {
                Directory.CreateDirectory(JSON_PATH);
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
