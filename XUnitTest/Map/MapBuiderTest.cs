using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Moq;
using Xunit;
using Fengj.Map;
using Fengj.API;
using Fengj.Utility;

namespace XUnitTest.Map
{
    [Collection("MapAndCell")]
    public class MapBuiderTest
    {
        [Fact]
        public void BuildPlainTest()
        {
            var map = new MapData(100, 100);
            MapData.Buider.BuildPlain(ref map, new ITerrainDef[] { Mock.Of<ITerrainDef>(x => x.type == TerrainType.PLAIN && x.code == "plan") });

            foreach (var cell in map)
            {
                cell.terrainType.Should().Be(TerrainType.PLAIN);
            }
        }

        [Fact]
        public void BuildMountTest()
        {
            var map = new MapData(50, 50);

            for (int i = 0; i < map.cells.Length; i++)
            {
                map.SetCell(i, new Cell(TerrainType.PLAIN, "plain"));
            }

            var percent = 0.1;
            MapData.Buider.BuildMount(ref map, percent, new ITerrainDef[] { Mock.Of<ITerrainDef>(x => x.type == TerrainType.MOUNT && x.code == "mount") });

            map.cells.Where(x => x.terrainType == TerrainType.MOUNT).Count().Should().Be((int)(percent * map.cells.Length));
        }

        [Fact]
        public void BuildHillTest()
        {
            var map = new MapData(50, 50);

            for (int i = 0; i < map.cells.Length; i++)
            {
                map.SetCell(i, new Cell(TerrainType.PLAIN, "plain"));
            }

            byte[] buffer = Guid.NewGuid().ToByteArray();
            Random random = new Random(BitConverter.ToInt32(buffer, 0));

            int mountCount = random.Next(1, 11);
            for (int i = 0; i < mountCount; i++)
            {
                map.SetCell(random.Next(0, 50 * 50), new Cell(TerrainType.MOUNT, "mount"));
            }

            var percent = 0.5;
            MapData.Buider.BuildHill(ref map, percent, new ITerrainDef[] { Mock.Of<ITerrainDef>(x => x.type == TerrainType.HILL && x.code == "hill") });

            map.cells.Where(x => x.terrainType == TerrainType.HILL).Count().Should().Be((int)(percent * map.cells.Length));
        }


        [Fact]
        public void BuildLakeTest()
        {
            var map = new MapData(50, 50);

            for (int i = 0; i < map.cells.Length; i++)
            {
                map.SetCell(i, new Cell(TerrainType.PLAIN, "plain"));
            }

            var random = new GTRandom();

            int mountCount = random.Next(1, 11);
            for (int i = 0; i < mountCount; i++)
            {
                map.SetCell(random.Next(0, 50 * 50), new Cell(TerrainType.MOUNT, "mount"));
            }

            int hillCount = random.Next(11, 31);
            for (int i = 0; i < mountCount; i++)
            {
                map.SetCell(random.Next(0, 50 * 50), new Cell(TerrainType.HILL, "hill"));
            }

            int riverCount = random.Next(1,10);
            for (int i = 0; i < riverCount; i++)
            {
                var cell = map.cells[random.Next(0, 50 * 50)];
                if(cell.components.All(x=>x.type != TerrainCMPType.RIVER))
                {
                    cell.components.Add(new TerrainComponent(TerrainCMPType.RIVER));
                }
            }

            var percent = 0.5;
            MapData.Buider.BuildLake(ref map, percent, new ITerrainDef[] { Mock.Of<ITerrainDef>(x => x.type == TerrainType.LAKE && x.code == "lake") });

            map.cells.Where(x => x.terrainType == TerrainType.LAKE).Count().Should().Be((int)(percent * map.cells.Length));
        }

        [Fact]
        public void BuildRiverTest()
        {
            var map = new MapData(50, 50);

            for (int i = 0; i < map.cells.Length; i++)
            {
                map.SetCell(i, new Cell(TerrainType.PLAIN, "plain"));
            }

            var random = new GTRandom();

            int mountCount = random.Next(1, 11);
            for (int i = 0; i < mountCount; i++)
            {
                map.SetCell(random.Next(0, 50 * 50), new Cell(TerrainType.MOUNT, "mount"));
            }

            int hillCount = random.Next(11, 31);
            for (int i = 0; i < mountCount; i++)
            {
                map.SetCell(random.Next(0, 50 * 50), new Cell(TerrainType.HILL, "hill"));
            }

            int lakeCount = random.Next(10, 21);
            for (int i = 0; i < mountCount; i++)
            {
                map.SetCell(random.Next(0, 50 * 50), new Cell(TerrainType.LAKE, "lake"));
            }

            MapData.Buider.BuildRiver(ref map);

            var riverCells = map.cells.Where(x => x.components.Any(c => c.type == TerrainCMPType.RIVER)).ToArray();

            riverCells.Count().Should().NotBe(0);

            for (int i=0; i< riverCells.Count(); i++)
            {
                var cell = riverCells[i];
                cell.terrainType.Should().Match<TerrainType>(x => x == TerrainType.LAKE || x == TerrainType.PLAIN);

                cell.GetNeighbours().Any(x => riverCells.Contains(x)).Should().BeTrue();
            }
        }
    }
}