using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleList.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleList.Service.Tests
{
    [TestClass()]
    public class MapPointServiceTests
    {
        [TestMethod()]
        public void GetMapPointNameTest()
        {
            var id =  MapPointService.GetMapPointName(4, 5, 13);
            Assert.IsTrue(id == "M");
        }
    }
}