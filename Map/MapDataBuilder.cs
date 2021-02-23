using Fengj.API;
using Fengj.IO;
using Fengj.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Fengj.Map
{
    partial class MapData : HexMatrix<ICell>, INotifyPropertyChanged
    {
        public static class Buider
        {

            public static MapData build(MapBuildType mapType, (int row, int column) mapSize, Dictionary<TerrainType, Dictionary<string, ITerrainDef>> terrainDefs)
            {
                var terrainDict = new Dictionary<TerrainType, ITerrainDef>();

                var map = new MapData(mapSize.row, mapSize.column);
                BuildPlain(ref map, terrainDefs[TerrainType.PLAIN].Values);

                var Type2Percent = calcPercent(mapType);

                BuildMount(ref map, Type2Percent[TerrainType.MOUNT], terrainDefs[TerrainType.MOUNT].Values);

                BuildHill(ref map, Type2Percent[TerrainType.HILL], terrainDefs[TerrainType.HILL].Values);

                //BuildForest(ref map, 0.1);

                BuildRiver(ref map);

                BuildLake(ref map, Type2Percent[TerrainType.LAKE], terrainDefs[TerrainType.LAKE].Values);

                BuildMarsh(ref map, terrainDefs[TerrainType.MARSH].Values);

                LOG.INFO("build" + map.ToString());
                return map;
            }

            public static void BuildMarsh(ref MapData map, IEnumerable<ITerrainDef> terrainDefs)
            {
                var random = new GTRandom();

                var LakeBounds = map.GetBoundCells(TerrainType.LAKE);
                foreach(var bound in LakeBounds.Where(x => x.terrainType == TerrainType.PLAIN))
                {
                    if(random.Next(0, 3) < 2)
                    {
                        map.ReplaceCell(bound, new Cell(terrainDefs.RandomOne()));
                    }
                }

                var riverCells = map.cells.Where(x => x.HasComponent(TerrainCMPType.RIVER));
                foreach (var cell in riverCells)
                {
                    if (random.Next(0, 100) < 95)
                    {
                        ICell newCell = new Cell(terrainDefs.RandomOne());
                        newCell.components.AddRange(cell.components);

                        map.ReplaceCell(cell, newCell);

                        foreach (var near in newCell.GetNeighbours().Where(x => x.terrainType == TerrainType.PLAIN))
                        {
                            if (random.Next(0, 2) < 1)
                            {
                                map.ReplaceCell(near, new Cell(terrainDefs.RandomOne()));
                            }
                        }
                    }
                }
            }

            public static void BuildRiver(ref MapData map)
            {
                var random = new GTRandom();
                var from = map.GetCell(random.Next(0, map.colum), 0);

                SetRiver(ref map, from);
            }

            private static bool SetRiver(ref MapData map, ICell cur)
            {
                cur.components.Add(new TerrainComponent(TerrainCMPType.RIVER));

                if (cur.vectIndex.x == map.colum)
                {
                    return true;
                }

                var random = new GTRandom();

                var dictNear = cur.GetNeighboursWithDirect();

                var maxColum = map.colum - 1;
                var nexts = dictNear.Where(x =>x.Key != DIRECTION.NORTH && x.Key != DIRECTION.WEST_NORTH && x.Key != DIRECTION.WEST_NORTH
                                               && x.Value != null
                                               && x.Value != cur
                                               && x.Value.vectIndex.y < maxColum && x.Value.vectIndex.y > 0);

                if (nexts.All(x => x.Value.components.Any(c => c.type == TerrainCMPType.RIVER)))
                {
                    return true;
                }

                do
                {
                    var plains = nexts.Where(x => x.Value.terrainType == TerrainType.PLAIN && x.Value.components.All(c => c.type != TerrainCMPType.RIVER));
                    if (plains.Count() != 0)
                    {
                        cur = plains.ElementAt(random.Next(0, plains.Count())).Value;
                    }
                    else
                    {
                        return false;
                    }

                }
                while (!SetRiver(ref map, cur));

                return true;
            }

            public static int BuildLake(ref MapData map, double percent, IEnumerable<ITerrainDef> terrainDefs)
            {
                int total = (int)(percent * map.cells.Length);

                var random = new GTRandom();

                var riverCells = map.cells.Where(x => x.HasComponent(TerrainCMPType.RIVER));
                var riverConnCells = riverCells.Where(x=> x.GetNeighbours().Count(n=>n.HasComponent(TerrainCMPType.RIVER)) > 2);

                var colum = map.colum;
                var seeds = riverConnCells.ToList();

                if (seeds.Count < 5)
                {
                    seeds.AddRange(riverCells.RandomFetch(5 - seeds.Count));
                }

                seeds = seeds.Distinct().ToList();

                foreach (var seed in seeds.Select(x => x.vectIndex))
                {
                    map.ReplaceCell(map.TryGetCell(seed.x, seed.y), new Cell(terrainDefs.RandomOne()));
                }

                int curr = seeds.Count();

                while (true)
                {

                    IEnumerable<ICell> bounds = map.GetBoundCells(TerrainType.LAKE);
                    if (bounds.Count() == 0)
                    {
                        throw new Exception("bound is null");
                    }

                    bounds = bounds.OrderBy(a => Guid.NewGuid());

                    foreach (var bound in bounds)
                    {
                        if (bound.terrainType != TerrainType.PLAIN)
                        {
                            continue;
                        }

                        var value = random.Next(0, 2);
                        if (value == 0)
                        {
                            map.ReplaceCell(bound, new Cell(terrainDefs.RandomOne()));
                            curr++;
                        }

                        if (curr == total)
                        {
                            LOG.INFO("BuildLAKE" + curr);
                            return curr;
                        }
                    }
                }
            }

            private static void BuildForest(ref MapData map, double v)
            {
                //TODO BuildForest
            }

            public static int BuildHill(ref MapData map, double percent, IEnumerable<ITerrainDef> terrainDefs)
            {
                byte[] buffer = Guid.NewGuid().ToByteArray();
                Random random = new Random(BitConverter.ToInt32(buffer, 0));

                int total = (int)(percent * map.cells.Length);

                if (total < 20)
                {
                    throw new Exception();
                }

                var cellcount = map.cells.Length;

                IEnumerable<int> seeds = Enumerable.Range(0, total / 20).Select(x => random.Next(0, cellcount)).Distinct().ToArray();

                foreach (var seed in seeds)
                {
                    map.ReplaceCell(map.cells[seed], new Cell(terrainDefs.RandomOne()));
                }

                int curr = seeds.Count();

                while (true)
                {
                    IEnumerable<ICell> bounds = map.GetBoundCells(TerrainType.MOUNT, TerrainType.HILL);
                    if (bounds.Count() == 0)
                    {
                        var cells = map.Where(x => x.terrainType == TerrainType.MOUNT || x.terrainType == TerrainType.HILL);
                        throw new Exception("bound is null" + cells.Count());
                    }

                    bounds = bounds.OrderBy(a => Guid.NewGuid());

                    foreach (var bound in bounds)
                    {
                        double p = 0;
                        if (bound.GetNeighbours(2).Any(x => x.terrainType == TerrainType.MOUNT))
                        {
                            p = 100;
                        }
                        else
                        {
                            p = 1;
                        }

                        if (GRandom.isOccur(p))
                        {
                            map.ReplaceCell(bound, new Cell(terrainDefs.RandomOne()));
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

            public static int BuildMount(ref MapData map, double percent, IEnumerable<ITerrainDef> terrainDefs)
            {
                byte[] buffer = Guid.NewGuid().ToByteArray();
                Random random = new Random(BitConverter.ToInt32(buffer, 0));

                int total = (int)(percent * map.cells.Length);

                var cellcount = map.cells.Length;


                if (total < 5)
                {
                    total = 5;
                }

                IEnumerable<int> seeds = Enumerable.Range(0, total / 5).Select(x => random.Next(0, cellcount)).Distinct().ToArray();

                foreach (var seed in seeds)
                {
                    map.ReplaceCell(map.cells[seed], new Cell(terrainDefs.RandomOne()));
                }

                int curr = seeds.Count();
                while (true)
                {

                    IEnumerable<ICell> bounds = map.GetBoundCells(TerrainType.MOUNT);
                    if (bounds.Count() == 0)
                    {
                        throw new Exception("bound is null");
                    }

                    bounds = bounds.OrderBy(a => Guid.NewGuid());

                    foreach (var bound in bounds)
                    {
                        var value = random.Next(0, 100);
                        if (value == 0)
                        {
                            map.ReplaceCell(bound, new Cell(terrainDefs.RandomOne()));
                            curr++;
                        }

                        if (curr == total)
                        {
                            if (curr != map.Count(x => x.terrainType == TerrainType.MOUNT))
                            {
                                throw new Exception();
                            }
                            LOG.INFO("BuildMount" + curr);
                            return curr;
                        }
                    }
                }
            }

            public static void BuildPlain(ref MapData map, IEnumerable<ITerrainDef> terrainDefs)
            {
                for (int i = 0; i < map.cells.Length; i++)
                {
                    map.SetCell(i, new Cell(terrainDefs.RandomOne()));
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
