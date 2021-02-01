using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Moq;
using Runner;

using Xunit;

using static Runner.Map;

namespace XUnitTest.Runner
{
    public class MapTest
    {
        [Fact]
        public void GenerateTest()
        {
            var map = new Map();

            map.row = 5;
            map.column = 5;

            map.cells = new List<ICell>()
            {
                Mock.Of<ICell>(x=>x.terrainKey=="T00"),
                Mock.Of<ICell>(x=>x.terrainKey=="T10"),
                Mock.Of<ICell>(x=>x.terrainKey=="T20"),
                Mock.Of<ICell>(x=>x.terrainKey=="T30"),
                Mock.Of<ICell>(x=>x.terrainKey=="T40"),
                Mock.Of<ICell>(x=>x.terrainKey=="T01"),
                Mock.Of<ICell>(x=>x.terrainKey=="T11"),
                Mock.Of<ICell>(x=>x.terrainKey=="T21"),
                Mock.Of<ICell>(x=>x.terrainKey=="T31"),
                Mock.Of<ICell>(x=>x.terrainKey=="T41"),
                Mock.Of<ICell>(x=>x.terrainKey=="T02"),
                Mock.Of<ICell>(x=>x.terrainKey=="T12"),
                Mock.Of<ICell>(x=>x.terrainKey=="T22"),
                Mock.Of<ICell>(x=>x.terrainKey=="T32"),
                Mock.Of<ICell>(x=>x.terrainKey=="T42"),
                Mock.Of<ICell>(x=>x.terrainKey=="T03"),
                Mock.Of<ICell>(x=>x.terrainKey=="T13"),
                Mock.Of<ICell>(x=>x.terrainKey=="T23"),
                Mock.Of<ICell>(x=>x.terrainKey=="T33"),
                Mock.Of<ICell>(x=>x.terrainKey=="T43"),
                Mock.Of<ICell>(x=>x.terrainKey=="T04"),
                Mock.Of<ICell>(x=>x.terrainKey=="T14"),
                Mock.Of<ICell>(x=>x.terrainKey=="T24"),
                Mock.Of<ICell>(x=>x.terrainKey=="T34"),
                Mock.Of<ICell>(x=>x.terrainKey=="T44"),
            };

            var nears = map.GetNeighbours((0, 0));
            nears[DIRECTION.WEST_NORTH].Should().BeNull();
            nears[DIRECTION.EAST_NORTH].Should().BeNull();
            nears[DIRECTION.EAST].terrainKey.Should().Be("T10");
            nears[DIRECTION.EAST_SOUTH].terrainKey.Should().Be("T01");
            nears[DIRECTION.WEST_SOUTH].Should().BeNull();
        }
    }
}
