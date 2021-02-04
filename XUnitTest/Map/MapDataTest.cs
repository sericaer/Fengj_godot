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
    public class MapDataTest
    {
        [Fact]
        public void GetNeighboursTest()
        {
            var map = new MapData(5, 5);

            for (int i = 0; i < map.row * map.colum; i++)
            {
                map.SetCell(i, Mock.Of<ICell>());
            }

            var nears = map.GetNears(0, 0);
            nears[DIRECTION.WEST_NORTH].Should().BeNull();
            nears[DIRECTION.EAST_NORTH].Should().BeNull();
            nears[DIRECTION.EAST].vectIndex.Should().Be((1, 0));
            nears[DIRECTION.EAST_SOUTH].vectIndex.Should().Be((0, 1));
            nears[DIRECTION.WEST_SOUTH].Should().BeNull();
        }
    }
}
