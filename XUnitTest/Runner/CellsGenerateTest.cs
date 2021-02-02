using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fengj.API;
using FluentAssertions;
using Moq;

using Xunit;

using static Fengj.Map;

namespace XUnitTest.Runner
{
    public class CellsGenerateTest
    {
        [Fact]
        public void GenerateTest()
        {
            CellsGenerator.defs = new List<ITerrainOccur>()
            {
                Mock.Of<ITerrainOccur>(e=>e.key == "TEST1" && e.CalcOccur(It.IsAny<IEnumerable<string>>()) == 20),
                Mock.Of<ITerrainOccur>(e=>e.key == "TEST2" && e.CalcOccur(It.IsAny<IEnumerable<string>>()) == 30),
                Mock.Of<ITerrainOccur>(e=>e.key == "TEST3" && e.CalcOccur(It.IsAny<IEnumerable<string>>()) == 50)
            };

            var generator = new CellsGenerator();
            var cells = generator.generate(100, 100);

            cells.Count.Should().Be(100 * 100);

            var statisc = cells.GroupBy(x => x.terrainKey).ToDictionary(x => x.Key, y => y.Count() * 100 / cells.Count());

            statisc["TEST1"].Should().BeInRange(19, 21);
            statisc["TEST2"].Should().BeInRange(29, 31);
            statisc["TEST3"].Should().BeInRange(49, 51);
        }
    }
}