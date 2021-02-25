﻿using Fengj.API;
using Fengj.IO;
using Fengj.Utility;
using HexMath;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Fengj.Map
{
    public enum MapBuildType
    {
        MAP_PLAIN,
        MAP_SMALL_HILL,
        MAP_BIG_HILL,
        MAP_MOUNT
    }

    partial class MapData
    {
        public static class Buider
        {

            public static MapData build(MapBuildType mapType, int maxDist, Dictionary<TerrainType, Dictionary<string, ITerrainDef>> terrainDefs)
            {
                var terrainDict = new Dictionary<TerrainType, ITerrainDef>();

                var map = new MapData(maxDist);

                BuildPlain(ref map, terrainDefs[TerrainType.PLAIN].Values);

                BuildRiver(ref map);

                var Type2Percent = calcPercent(mapType);

                BuildMount(ref map, Type2Percent[TerrainType.MOUNT], terrainDefs[TerrainType.MOUNT].Values);

                BuildHill(ref map, Type2Percent[TerrainType.HILL], terrainDefs[TerrainType.HILL].Values);

                //BuildLake(ref map, Type2Percent[TerrainType.LAKE], terrainDefs[TerrainType.LAKE].Values);

                //BuildMarsh(ref map, terrainDefs[TerrainType.MARSH].Values);

                ////BuildForest(ref map, 0.1);

                //BuildRiver(ref map);




                LOG.INFO("build" + map.ToString());
                return map;
            }


            public static void BuildRiver(ref MapData map)
            {
                var coords = map.center.axialCoord.GetRingWithWidth(4, 2);

                var cell = map.GetCell(coords.RandomOne());

                SetRiver(ref map, cell, 1);
                SetRiver(ref map, cell, -1);
            }

            private static void SetRiver(ref MapData map, ICell cell, int direct)
            {
                var random = new GTRandom();

                cell.components.Add(new TerrainComponent(TerrainCMPType.RIVER));

                var currMap = map;

                if (cell.axialCoord.Length() >= currMap.maxDist)
                {
                    return;
                }

                var nextCells = cell.axialCoord.GetNeighbors()
                                    .Select(x => currMap.GetCell(x));

                nextCells = nextCells.Where(x => !x.HasComponent(TerrainCMPType.RIVER))
                                     .Where(x => x.axialCoord.GetNeighbors()
                                                  .Where(n => currMap.HasCell(n))
                                                  .Select(y => currMap.GetCell(y))
                                                  .Count(z => z.HasComponent(TerrainCMPType.RIVER)) < 2);
                if(nextCells.Count() == 0)
                {
                    return;
                }

                var norths = nextCells.Where(x => x.axialCoord.Sub(cell.axialCoord).Equals(AxialCoord.directions[1])
                                  || x.axialCoord.Sub(cell.axialCoord).Equals(AxialCoord.directions[2])
                                  || x.axialCoord.Sub(cell.axialCoord).Equals(AxialCoord.directions[3]));

                var souths = nextCells.Where(x => x.axialCoord.Sub(cell.axialCoord).Equals(AxialCoord.directions[0])
                                  || x.axialCoord.Sub(cell.axialCoord).Equals(AxialCoord.directions[4])
                                  || x.axialCoord.Sub(cell.axialCoord).Equals(AxialCoord.directions[5]));

                var firstCells = souths;
                var second = norths;

                if (direct == -1)
                {
                    firstCells = norths;
                    second = souths;
                }

                if(second.Count() > 0)
                {
                    if(random.Next(0, 100) < 10 || firstCells.Count() == 0)
                    {
                        SetRiver(ref map, second.RandomOne(), direct);
                        return;
                    }
                }

                SetRiver(ref map, firstCells.RandomOne(), direct);
            }


            public static void BuildMarsh(ref MapData map, IEnumerable<ITerrainDef> terrainDefs)
            {
                var random = new GTRandom();

                var LakeBounds = map.GetBoundCells(TerrainType.LAKE);
                foreach (var bound in LakeBounds.Where(x => x.terrainType == TerrainType.PLAIN))
                {
                    if (random.Next(0, 3) < 2)
                    {
                        map.SetCell(new Cell(bound.axialCoord, terrainDefs.RandomOne()));
                    }
                }

                var riverCells = map.cells.Where(x => x.HasComponent(TerrainCMPType.RIVER)).ToArray();
                foreach (var cell in riverCells)
                {
                    if (random.Next(0, 100) < 95)
                    {
                        ICell newCell = new Cell(cell.axialCoord, terrainDefs.RandomOne());
                        newCell.components.AddRange(cell.components);

                        map.SetCell(newCell);

                        foreach (var near in newCell.GetNearTerrain(TerrainType.PLAIN, 1, map))
                        {
                            if (random.Next(0, 2) < 1)
                            {
                                map.SetCell(new Cell(near.axialCoord, terrainDefs.RandomOne()));
                            }
                        }
                    }
                }
            }

            //private static void BuildForest(ref MapData map, double v)
            //{
            //    //TODO BuildForest
            //}

            public static int BuildHill(ref MapData map, double percent, IEnumerable<ITerrainDef> terrainDefs)
            {
                var random = new GTRandom();

                var cellcount = map.cells.Count();
                int total = (int)(percent * cellcount);

                if (total < 20)
                {
                    total = 20;
                }

                var seedCount = total / 20;
                seedCount = seedCount > 10 ? 10 : seedCount;

                var seeds = map.cells.Where(x => x.terrainType == TerrainType.PLAIN && !x.HasComponent(TerrainCMPType.RIVER)).RandomFetch(seedCount).ToList(); ;
                foreach (var seed in seeds)
                {
                    map.SetCell(new Cell(seed.axialCoord, terrainDefs.RandomOne()));
                }

                var currMap = map;

                int curr = seeds.Count();

                var innerCells = map.cells.Where(x => x.terrainType == TerrainType.HILL || x.terrainType == TerrainType.MOUNT).ToArray();
                
                while(true)
                {
                    foreach (var cell in innerCells)
                    {
                        var distCells = map.cells.Where(x => x.terrainType == TerrainType.PLAIN && !x.HasComponent(TerrainCMPType.RIVER)).ToArray();
                        foreach (var distCell in distCells)
                        {
                            var dist = distCell.axialCoord.Distance(cell.axialCoord);
                            var p = dist / 3;
                            if (random.Next(0, dist*dist*dist) == 0)
                            {
                                map.SetCell(new Cell(distCell.axialCoord, terrainDefs.RandomOne()));
                                curr++;

                                if (curr == total)
                                {
                                    LOG.INFO("BuildHill" + curr);
                                    return curr;
                                }
                            }
                        }
                    }
                }


                return curr;
                //while (true)
                //{
                //    IEnumerable<ICell> bounds = map.GetBoundCells(TerrainType.MOUNT, TerrainType.HILL).Where(x => !x.HasComponent(TerrainCMPType.RIVER));
                //    if (bounds.Count() == 0)
                //    {
                //        throw new Exception("bound is null");
                //    }

                //    var bound = bounds.OrderBy(_ => random.Next(0, 100)).First();

                //    int p = 10;
                //    //if (bound.GetNearTerrain(TerrainType.MOUNT, 2, map).Count() > 0)
                //    //{
                //    //    p = 1;
                //    //}

                //    //if (random.Next(0, p) == 0)
                //    {
                //        map.SetCell(new Cell(bound.axialCoord, terrainDefs.RandomOne()));
                //        curr++;

                //        if (curr == total)
                //        {
                //            LOG.INFO("BuildHill" + curr);
                //            return curr;
                //        }
                //    }
                //}
            }

            public static int BuildMount(ref MapData map, double percent, IEnumerable<ITerrainDef> terrainDefs)
            {
                var random = new GTRandom();

                var cellcount = map.cells.Count();
                var total = (int)(percent * map.cells.Count());

                if (total < 5)
                {
                    total = 5;
                }

                var seedCount = total / 5;
                seedCount = seedCount > 5 ? 5 : seedCount;

                IEnumerable<ICell> seeds = map.cells.Where(x=>!x.HasComponent(TerrainCMPType.RIVER)).RandomFetch(seedCount);

                foreach (var seed in seeds)
                {
                    map.SetCell(new Cell(seed.axialCoord, terrainDefs.RandomOne()));
                }

                int curr = seeds.Count();
                while (true)
                {
                    IEnumerable<ICell> bounds = map.GetBoundCells(TerrainType.MOUNT).Where(x => !x.HasComponent(TerrainCMPType.RIVER));
                    if (bounds.Count() == 0)
                    {
                        throw new Exception("bound is null");
                    }

                    bounds = bounds.OrderBy(_ => random.Next(0, 100));

                    foreach (var bound in bounds)
                    {
                        if (random.Next(0, 10) == 0)
                        {
                            map.SetCell(new Cell(bound.axialCoord, terrainDefs.RandomOne()));
                            curr++;
                        }

                        if (curr == total)
                        {
                            return curr;
                        }
                    }
                }
            }


            public static int BuildLake(ref MapData map, double percent, IEnumerable<ITerrainDef> terrainDefs)
            {
                var random = new GTRandom();

                var cellcount = map.cells.Count();
                var total = (int)(percent * map.cells.Count());

                var seedCount = total / 5;
                seedCount = seedCount > 5 ? 5 : seedCount;

                var seeds = map.cells.Where(x => x.HasComponent(TerrainCMPType.RIVER)).RandomFetch(seedCount);

                foreach (var seed in seeds)
                {
                    map.SetCell(new Cell(seed.axialCoord, terrainDefs.RandomOne()));
                }

                int curr = seeds.Count();

                while (true)
                {
                    IEnumerable<ICell> bounds = map.GetBoundCells(TerrainType.LAKE);
                    if (bounds.Count() == 0)
                    {
                        throw new Exception("bound is null");
                    }

                    bounds = bounds.OrderBy(_ => random.Next(0, 100));

                    foreach (var bound in bounds)
                    {
                        if (bound.terrainType != TerrainType.PLAIN)
                        {
                            continue;
                        }

                        var value = random.Next(0, 2);
                        if (value == 0)
                        {
                            map.SetCell(new Cell(bound.axialCoord, terrainDefs.RandomOne()));
                            curr++;
                        }

                        if (curr == total)
                        {
                            return curr;
                        }
                    }
                }
            }

            public static void BuildPlain(ref MapData map, IEnumerable<ITerrainDef> terrainDefs)
            {
                var cells = map.Keys.Select(x => new Cell(new AxialCoord(x.q, x.r), terrainDefs.RandomOne())).ToArray();

                foreach(var cell in cells)
                {
                    map.SetCell(cell);
                }
            }

            public static Dictionary<TerrainType, double> calcPercent(MapBuildType mapType)
            {
                switch (mapType)
                {
                    case MapBuildType.MAP_PLAIN:
                        return new Dictionary<TerrainType, double>(){
                            { TerrainType.PLAIN, 0.87 },
                            { TerrainType.HILL, 0.099 },
                            { TerrainType.MOUNT, 0.001 },
                            { TerrainType.LAKE, 0.03 },
                        };
                    case MapBuildType.MAP_SMALL_HILL:
                        return new Dictionary<TerrainType, double>(){
                            { TerrainType.PLAIN, 0.47 },
                            { TerrainType.HILL, 0.35 },
                            { TerrainType.MOUNT, 0.15 },
                            { TerrainType.LAKE, 0.03 },
                        };

                    case MapBuildType.MAP_BIG_HILL:
                        return new Dictionary<TerrainType, double>(){
                            { TerrainType.PLAIN, 0.27 },
                            { TerrainType.HILL, 0.5 },
                            { TerrainType.MOUNT, 0.2 },
                            { TerrainType.LAKE, 0.03 },
                        };

                    case MapBuildType.MAP_MOUNT:
                        return new Dictionary<TerrainType, double>(){
                            { TerrainType.PLAIN, 0.17 },
                            { TerrainType.HILL, 0.4 },
                            { TerrainType.MOUNT, 0.4 },
                            { TerrainType.LAKE, 0.03},
                        };

                    default:
                        throw new Exception();
                }
            }
        }
    }
}
