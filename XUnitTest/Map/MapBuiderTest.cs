using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Moq;
using Xunit;
using Fengj.Map;
using Fengj.API;

namespace XUnitTest.Map
{
    [Collection("MapAndCell")]
    public class MapBuiderTest
    {
        [Fact]
        public void BuildPlainTest()
        {
            var map = new MapData(100, 100);
            MapData.Buider.BuildPlain(ref map);

            foreach(var cell in map)
            {
                cell.terrainType.Should().Be(TerrainType.PLAIN);
            }
        }

        [Fact]
        public void BuildMountTest()
        {
            var map = new MapData(50, 50);

            for (int i=0; i<map.cells.Length; i++)
            {
                map.SetCell(i, new Cell(TerrainType.PLAIN));
            }

            var percent = 0.1;
            MapData.Buider.BuildMount(ref map, percent);

            map.cells.Where(x => x.terrainType == TerrainType.MOUNT).Count().Should().Be((int)(percent * map.cells.Length));
        }

        [Fact]
        public void BuildHillTest()
        {
            var map = new MapData(50, 50);

            for (int i = 0; i < map.cells.Length; i++)
            {
                map.SetCell(i, new Cell(TerrainType.PLAIN));
            }

            byte[] buffer = Guid.NewGuid().ToByteArray();
            Random random = new Random(BitConverter.ToInt32(buffer, 0));

            int mountCount = random.Next(1, 11);
            for (int i=0; i<mountCount; i++)
            {
                map.SetCell(random.Next(0, 50*50), new Cell(TerrainType.MOUNT));
            }

            var percent = 0.5;
            MapData.Buider.BuildHill(ref map, percent);

            map.cells.Where(x => x.terrainType == TerrainType.HILL).Count().Should().Be((int)(percent * map.cells.Length));
        }
    }
}