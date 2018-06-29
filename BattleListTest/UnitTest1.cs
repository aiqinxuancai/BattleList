using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleList;
using BattleList.Service;
using System.Diagnostics;

namespace BattleListTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Debug.WriteLine("开始测试");

            Debug.WriteLine(MapPointService.GetMapPointName(4, 5, 19));
        }
    }
}
