﻿//using System;
//using System.Collections.Generic;
//using System.Text;
//using FluentAssertions;
//using Moq;
//using Fengj;

//using Xunit;
//using Fengj.Map;
//using System.Linq;
//using Fengj.API;

//namespace XUnitTest.Map
//{
//    [Collection("MapAndCell")]
//    public class MapDataTest
//    {
//        [Fact]
//        public void GetNeighboursTopLeftTest()
//        {
//            var map = new MapData(5, 5);

//            for (int i = 0; i < map.row * map.colum; i++)
//            {
//                map.SetCell(i, Mock.Of<ICell>());
//            }

//            var nears = map.GetNears(0, 0);
//            nears[DIRECTION.WEST_NORTH].Should().BeNull();
//            nears[DIRECTION.EAST_NORTH].Should().BeNull();
//            nears[DIRECTION.NORTH].Should().BeNull();
//            nears[DIRECTION.EAST_SOUTH].vectIndex.Should().Be((0, 1));
//            nears[DIRECTION.WEST_SOUTH].Should().BeNull();
//            nears[DIRECTION.SOUTH].vectIndex.Should().Be((1, 0));
//        }

//        [Fact]
//        public void GetNeighboursTopRightTest()
//        {
//            var map = new MapData(5, 5);

//            for (int i = 0; i < map.row * map.colum; i++)
//            {
//                map.SetCell(i, Mock.Of<ICell>());
//            }

//            var nears = map.GetNears(0, 4);
//            nears[DIRECTION.WEST_NORTH].Should().BeNull();
//            nears[DIRECTION.EAST_NORTH].Should().BeNull();
//            nears[DIRECTION.NORTH].Should().BeNull();
//            nears[DIRECTION.EAST_SOUTH].Should().BeNull();
//            nears[DIRECTION.WEST_SOUTH].vectIndex.Should().Be((0, 3));
//            nears[DIRECTION.SOUTH].vectIndex.Should().Be((1, 4));
//         }

//        [Fact]
//        public void GetNeighboursBottomRightTest()
//        {
//            var map = new MapData(5, 5);

//            for (int i = 0; i < map.row * map.colum; i++)
//            {
//                map.SetCell(i, Mock.Of<ICell>());
//            }

//            var nears = map.GetNears(4, 4);
//            nears[DIRECTION.WEST_NORTH].vectIndex.Should().Be((3, 3));
//            nears[DIRECTION.EAST_NORTH].Should().BeNull();
//            nears[DIRECTION.NORTH].vectIndex.Should().Be((3, 4));
//            nears[DIRECTION.EAST_SOUTH].Should().BeNull();
//            nears[DIRECTION.WEST_SOUTH].vectIndex.Should().Be((4, 3));
//            nears[DIRECTION.SOUTH].Should().BeNull();
//        }


//        [Fact]
//        public void GetNeighboursBottomLeftTest()
//        {
//            var map = new MapData(5, 5);

//            for (int i = 0; i < map.row * map.colum; i++)
//            {
//                map.SetCell(i, Mock.Of<ICell>());
//            }

//            var nears = map.GetNears(4, 0);
//            nears[DIRECTION.WEST_NORTH].Should().BeNull();
//            nears[DIRECTION.EAST_NORTH].vectIndex.Should().Be((3, 1));
//            nears[DIRECTION.NORTH].vectIndex.Should().Be((3, 0));
//            nears[DIRECTION.EAST_SOUTH].vectIndex.Should().Be((4, 1));
//            nears[DIRECTION.WEST_SOUTH].Should().BeNull();
//            nears[DIRECTION.SOUTH].Should().BeNull();
//        }

//        [Fact]
//        public void GetNeighboursInSingleRow()
//        {
//            var map = new MapData(5, 5);

//            for (int i = 0; i < map.row * map.colum; i++)
//            {
//                map.SetCell(i, Mock.Of<ICell>());
//            }

//            var nears = map.GetNears(2, 3);
//            nears[DIRECTION.WEST_NORTH].vectIndex.Should().Be((2, 2));
//            nears[DIRECTION.EAST_NORTH].vectIndex.Should().Be((2, 4));
//            nears[DIRECTION.NORTH].vectIndex.Should().Be((1, 3));
//            nears[DIRECTION.EAST_SOUTH].vectIndex.Should().Be((3, 4));
//            nears[DIRECTION.WEST_SOUTH].vectIndex.Should().Be((3, 2));
//            nears[DIRECTION.SOUTH].vectIndex.Should().Be((3, 3));
//        }


//        [Fact]
//        public void GetNeighboursWithDistance()
//        {
//            var map = new MapData(5, 5);

//            for (int i = 0; i < map.row * map.colum; i++)
//            {
//                map.SetCell(i, Mock.Of<ICell>());
//            }

//            var cells = map.GetCellsWithDistance(0, 0, 2);
//            var indexList = new List<(int x, int y)>()
//            {
//                (1,0), (0, 1), (2, 0),(1,1),(0,2), (1,2)
//            };

//            cells.Select(x => x.vectIndex).Should().BeEquivalentTo(indexList);
//        }

//        [Fact]
//        public void GetBoundLeftTopTest()
//        {
//            var map = new MapData(10, 10);

//            for (int i = 0; i < map.row * map.colum; i++)
//            {
//                map.SetCell(i, new Cell(TerrainType.PLAIN, "plain"));
//            }

//            map.SetCell(0, 0, new Cell(TerrainType.HILL, "hill"));
//            map.SetCell(0, 1, new Cell(TerrainType.HILL, "hill"));
//            map.SetCell(1, 0, new Cell(TerrainType.HILL, "hill"));

//            var bounds = map.GetBoundCells(TerrainType.HILL);
//            bounds.Select(x => x.terrainType).All(x => x == TerrainType.PLAIN).Should().BeTrue();

//            var indexList = new List<(int x, int y)>()
//            {
//                (2, 0), (1,1),(1,2), (0,2)
//            };

//            var boundIndex = bounds.Select(x => x.vectIndex);
//            boundIndex.Should().BeEquivalentTo(indexList);
//        }

//        [Fact]
//        public void GetBoundRightTopTest()
//        {
//            var map = new MapData(10, 10);

//            for (int i = 0; i < map.row * map.colum; i++)
//            {
//                map.SetCell(i, new Cell(TerrainType.PLAIN, "plain"));
//            }

//            map.SetCell(0, 8, new Cell(TerrainType.HILL, "hill"));
//            map.SetCell(0, 9, new Cell(TerrainType.HILL, "hill"));
//            map.SetCell(1, 9, new Cell(TerrainType.HILL, "hill"));

//            var bounds = map.GetBoundCells(TerrainType.HILL);
//            bounds.Select(x => x.terrainType).All(x => x == TerrainType.PLAIN).Should().BeTrue();

//            var indexList = new List<(int x, int y)>()
//            {
//                (0,7),(1,8),(2,8),(2,9),
//            };

//            var boundIndex = bounds.Select(x => x.vectIndex);
//            boundIndex.Should().BeEquivalentTo(indexList);
//        }

//        [Fact]
//        public void GetBoundLeftBottomTest()
//        {
//            var map = new MapData(10, 10);

//            for (int i = 0; i < map.row * map.colum; i++)
//            {
//                map.SetCell(i, new Cell(TerrainType.PLAIN, "plain"));
//            }

//            map.SetCell(9, 0, new Cell(TerrainType.HILL, "hill"));
//            map.SetCell(9, 1, new Cell(TerrainType.HILL, "hill"));
//            map.SetCell(8, 0, new Cell(TerrainType.HILL, "hill"));

//            var bounds = map.GetBoundCells(TerrainType.HILL);
//            bounds.Select(x => x.terrainType).All(x => x == TerrainType.PLAIN).Should().BeTrue();

//            var indexList = new List<(int x, int y)>()
//            {
//                (7,0), (7,1),(8,1),(9,2)
//            };

//            var boundIndex = bounds.Select(x => x.vectIndex);
//            boundIndex.Should().BeEquivalentTo(indexList);
//        }


//        [Fact]
//        public void GetBoundRightBottomTest()
//        {
//            var map = new MapData(10, 10);

//            for (int i = 0; i < map.row * map.colum; i++)
//            {
//                map.SetCell(i, new Cell(TerrainType.PLAIN, "plain"));
//            }

//            map.SetCell(9, 9, new Cell(TerrainType.HILL, "hill"));
//            map.SetCell(9, 8, new Cell(TerrainType.HILL, "hill"));
//            map.SetCell(8, 9, new Cell(TerrainType.HILL, "hill"));

//            var bounds = map.GetBoundCells(TerrainType.HILL);
//            bounds.Select(x => x.terrainType).All(x => x == TerrainType.PLAIN).Should().BeTrue();

//            var indexList = new List<(int x, int y)>()
//            {
//                (9,7),(8,7),(8,8),(7,9)
//            };

//            var boundIndex = bounds.Select(x => x.vectIndex);
//            boundIndex.Should().BeEquivalentTo(indexList);
//        }
//    }
//}
