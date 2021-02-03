using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fengj.API;
using FluentAssertions;
using Moq;
using Xunit;
using Fengj.Map;

namespace XUnitTest.Runner
{
    public class BuiderTest
    {
        [Fact]
        public void BuildTest()
        {
            var defs = new List<ITerrainOccur>()
            {
                Mock.Of<ITerrainOccur>(e=>e.key == "TEST1" && e.CalcOccur(It.IsAny<IEnumerable<string>>()) == 20),
                Mock.Of<ITerrainOccur>(e=>e.key == "TEST2" && e.CalcOccur(It.IsAny<IEnumerable<string>>()) == 30),
                Mock.Of<ITerrainOccur>(e=>e.key == "TEST3" && e.CalcOccur(It.IsAny<IEnumerable<string>>()) == 50)
            };

            var map = MapData.Buider.build((100, 100), defs);

            map.size.Should().Be(100 * 100);

            var statisc = map.cells.GroupBy(x => x.terrainKey).ToDictionary(x => x.Key, y => y.Count() * 100 / map.size);

            statisc["TEST1"].Should().BeInRange(19, 21);
            statisc["TEST2"].Should().BeInRange(29, 31);
            statisc["TEST3"].Should().BeInRange(49, 51);
        }
    }
}