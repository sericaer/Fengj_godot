﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Moq;
using Xunit;
using Fengj.Map;
using Fengj.API;
using Fengj.Utility;
using HexMath;

namespace XUnitTest.Map
{
    [Collection("MapAndCell")]
    public class MapBuiderTest
    {
        [Fact]
        public void BuildPlainTest()
        {
            var map = new MapData(20);

            MapData.Buider.BuildPlain(ref map, new ITerrainDef[] { Mock.Of<ITerrainDef>(x => x.type == TerrainType.PLAIN && x.code == "plan") });

            foreach (var elem in map)
            {
                elem.Value.terrainType.Should().Be(TerrainType.PLAIN);
            }
        }

        [Fact]
        public void BuildMountTest()
        {
            var random = new GTRandom();

            var map = new MapData(20);

            var terrainType = Mock.Of<ITerrainDef>(x => x.type == TerrainType.PLAIN && x.code == "plain");
            foreach (var key in map.Keys.ToArray())
            {
                var cell = new Cell(new AxialCoord(key.q, key.r), terrainType);
                if (random.Next(0, 100) < 5)
                {
                    cell.components.Add(new TerrainComponent(TerrainCMPType.RIVER));
                }

                map.SetCell(cell);
            }

            var percent = 0.1;
            MapData.Buider.BuildMount(ref map, percent, new ITerrainDef[] { Mock.Of<ITerrainDef>(x => x.type == TerrainType.MOUNT && x.code == "mount") });

            map.cells.Where(x => x.terrainType == TerrainType.MOUNT).Count().Should().Be((int)(percent * map.cells.Count()));
        }

        [Fact]
        public void BuildHillTest()
        {
            var random = new GTRandom();

            var map = new MapData(50);

            var terrainDef = Mock.Of<ITerrainDef>(x => x.type == TerrainType.PLAIN && x.code == "plain");
            foreach (var key in map.Keys.ToArray())
            {
                var cell = new Cell(new AxialCoord(key.q, key.r), terrainDef);
                map.SetCell(cell);
            }

            int mountCount = 20;
            var mountCells = map.cells.RandomFetch(mountCount).ToArray();
            terrainDef = Mock.Of<ITerrainDef>(x => x.type == TerrainType.MOUNT && x.code == "mount");
            for (int i = 0; i < mountCount; i++)
            {
                map.SetCell(new Cell(mountCells[i].axialCoord, terrainDef));
            }

            int riverCount = 20;
            var riverCells = map.cells.Where(x => x.terrainType == TerrainType.PLAIN).RandomFetch(riverCount).ToArray();
            for (int i = 0; i < riverCount; i++)
            {
                riverCells[i].components.Add(new TerrainComponent(TerrainCMPType.RIVER));
            }

            var percent = 0.2;
            MapData.Buider.BuildHill(ref map, percent, new ITerrainDef[] { Mock.Of<ITerrainDef>(x => x.type == TerrainType.HILL && x.code == "hill") });

            map.cells.Where(x => x.terrainType == TerrainType.HILL).Count().Should().Be((int)(percent * map.cells.Count()));
        }


        [Fact]
        public void BuildLakeTest()
        {
            var map = new MapData(50);


            var terrainDef = Mock.Of<ITerrainDef>(x => x.type == TerrainType.PLAIN && x.code == "plain");
            foreach (var key in map.Keys.ToArray())
            {
                var cell = new Cell(new AxialCoord(key.q, key.r), terrainDef);
                map.SetCell(cell);
            }

            int mountCount = 20;
            var mountCells = map.cells.RandomFetch(mountCount).ToArray();
            terrainDef = Mock.Of<ITerrainDef>(x => x.type == TerrainType.MOUNT && x.code == "mount");
            for (int i = 0; i < mountCount; i++)
            {
                map.SetCell(new Cell(mountCells[i].axialCoord, terrainDef));
            }

            int hillCount = 30;
            var hillCells = map.cells.Where(x=>x.terrainType == TerrainType.PLAIN).RandomFetch(hillCount).ToArray();
            terrainDef = Mock.Of<ITerrainDef>(x => x.type == TerrainType.HILL && x.code == "hill");
            for (int i = 0; i < hillCount; i++)
            {
                map.SetCell(new Cell(hillCells[i].axialCoord, terrainDef));
            }

            int riverCount = 20;
            var riverCells = map.cells.Where(x => x.terrainType == TerrainType.PLAIN).RandomFetch(riverCount).ToArray();
            for (int i = 0; i < riverCount; i++)
            {
                riverCells[i].components.Add(new TerrainComponent(TerrainCMPType.RIVER));
            }

            var percent = 0.3;
            MapData.Buider.BuildLake(ref map, percent, new ITerrainDef[] { Mock.Of<ITerrainDef>(x => x.type == TerrainType.LAKE && x.code == "lake") });

            map.cells.Where(x => x.terrainType == TerrainType.LAKE).Count().Should().Be((int)(percent * map.cells.Count()));
        }

        [Fact]
        public void BuildRiverTest()
        {
            var map = new MapData(20);

            var terrainType = Mock.Of<ITerrainDef>(x => x.type == TerrainType.PLAIN && x.code == "plain");
            foreach (var key in map.Keys.ToArray())
            {
                map.SetCell(new Cell(new AxialCoord(key.q, key.r), terrainType));
            }


            MapData.Buider.BuildRiver(ref map);

            var riverCells = map.cells.Where(x => x.components.Any(c => c.type == TerrainCMPType.RIVER)).ToArray();

            riverCells.Count().Should().NotBe(0);
            foreach(var riverCell in riverCells)
            {
                riverCell.axialCoord.GetNeighbors()
                    .Where(x=>map.HasCell(x))
                    .Select(x=>map.GetCell(x))
                    .Any(x=> riverCells.Contains(x)).Should().BeTrue();
            }
        }


        [Fact]
        public void BuildMarshTest()
        {
            var map = new MapData(50);


            var terrainType = Mock.Of<ITerrainDef>(x => x.type == TerrainType.PLAIN && x.code == "plain");
            foreach (var key in map.Keys.ToArray())
            {
                var cell = new Cell(new AxialCoord(key.q, key.r), terrainType);
                map.SetCell(cell);
            }

            int mountCount = 20;
            var mountCells = map.cells.RandomFetch(mountCount).ToArray();
            terrainType = Mock.Of<ITerrainDef>(x => x.type == TerrainType.MOUNT && x.code == "mount");
            for (int i = 0; i < mountCount; i++)
            {
                map.SetCell(new Cell(mountCells[i].axialCoord, terrainType));
            }

            int hillCount = 30;
            var hillCells = map.cells.Where(x => x.terrainType == TerrainType.PLAIN).RandomFetch(hillCount).ToArray();
            terrainType = Mock.Of<ITerrainDef>(x => x.type == TerrainType.HILL && x.code == "hill");
            for (int i = 0; i < hillCount; i++)
            {
                map.SetCell(new Cell(hillCells[i].axialCoord, terrainType));
            }

            int riverCount = 20;
            var riverCells = map.cells.Where(x => x.terrainType == TerrainType.PLAIN).RandomFetch(riverCount).ToArray();
            for (int i = 0; i < riverCount; i++)
            {
                riverCells[i].components.Add(new TerrainComponent(TerrainCMPType.RIVER));
            }

            int lakeCount = 20;
            var lakeCells = map.cells.Where(x => x.terrainType == TerrainType.PLAIN).RandomFetch(lakeCount).ToArray();
            terrainType = Mock.Of<ITerrainDef>(x => x.type == TerrainType.LAKE && x.code == "lake");
            for (int i = 0; i < lakeCount; i++)
            {
                map.SetCell(new Cell(lakeCells[i].axialCoord, terrainType));
            }

            MapData.Buider.BuildMarsh(ref map, new ITerrainDef[] { Mock.Of<ITerrainDef>(x => x.type == TerrainType.MARSH && x.code == "marsh") });

            var marshCells = map.cells.Where(x => x.terrainType == TerrainType.MARSH);
            marshCells.Count().Should().NotBe(0);

            foreach (var cell in marshCells)
            {
                Assert.True(cell.HasComponent(TerrainCMPType.RIVER)
                    || cell.GetNearTerrain(TerrainType.LAKE, 1, map).Count() > 0 
                    || cell.GetNearTerrain(TerrainType.MARSH, 1, map).Count() > 0);
            }
        }
    }
}