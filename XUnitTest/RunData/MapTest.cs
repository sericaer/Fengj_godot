using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Moq;
using Fengj;

using Xunit;
using Fengj.Map;

namespace XUnitTest.Runner
{
    public class MapTest
    {
        [Fact]
        public void GenerateTest()
        {
            var map = new MapData();

            map.row = 5;
            map.column = 5;

            map.cells = new List<ICell>();

            for (int i = 0; i < map.row; i++)
            {
                for (int j = 0; j < map.column; j++)
                {
                    map.cells.Add(Mock.Of<ICell>(x => x.terrainKey == $"T{j}{i}"));
                }
            }



            var nears = map.GetNeighbours((0, 0));
            nears[DIRECTION.WEST_NORTH].Should().BeNull();
            nears[DIRECTION.EAST_NORTH].Should().BeNull();
            nears[DIRECTION.EAST].terrainKey.Should().Be("T10");
            nears[DIRECTION.EAST_SOUTH].terrainKey.Should().Be("T01");
            nears[DIRECTION.WEST_SOUTH].Should().BeNull();
        }
    }
}
