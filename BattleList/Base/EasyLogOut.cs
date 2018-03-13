﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BattleList.Base
{

    /// <summary>
    /// 输出级别
    /// </summary>
    public enum LogLevel { Trace, Debug, Info, Warn, Error }




    static class EasyLogOut
    {
        private static object flag = new object();

        private static string LOG_PATH = @".\Data\BattleList\run.log";

        public static void Write(object obj, LogLevel type = LogLevel.Info, string file = null)
        {
            ThreadPool.QueueUserWorkItem(h =>
            {
                lock (flag)
                {
                    if (file == null)   // 默认输出到应用目录下
                    {
                        file = LOG_PATH;//AppDomain.CurrentDomain.BaseDirectory + DateTime.Now.ToString("yyyy_MM_dd") + ".log";
                    }
                    string head = string.Format(">>>{0}[{1}]", DateTime.Now.ToString("HH:mm:ss.fff"), type.ToString());
                    File.AppendAllText(file, head + obj.ToString() + "\r\n");
                }
            });
        }
    }
}
