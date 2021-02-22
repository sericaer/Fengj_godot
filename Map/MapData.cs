using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Fengj.API;
using Fengj.IO;
using Fengj.Utility;

namespace Fengj.Map
{
    public enum DIRECTION
    {
        WEST_NORTH,
        EAST_NORTH,
        NORTH,
        EAST_SOUTH,
        WEST_SOUTH,
        SOUTH,
    }

    public enum MapBuildType
    {
        MAP_PLAIN,
        MAP_SMALL_HILL,
        MAP_BIG_HILL,
        MAP_MOUNT
    }

    class MapData : HexMatrix<ICell> , INotifyPropertyChanged
    {
        public Cell changedCell { get; set; }
        
        public MapData(int row, int colum) : base(row, colum)
        {
            Cell.map = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public new  void SetCell(int index,  ICell cell)
        {
            cell.vectIndex = (index / colum, index % colum);
            base.SetCell(index, cell);
        }

        public new void SetCell(int x, int y, ICell cell)
        {
            cell.vectIndex = (x, y);
            base.SetCell(x*colum+y, cell);
        }

        public void ReplaceCell(ICell oldCell, ICell cell)
        {
            cell.vectIndex = oldCell.vectIndex;
            base.SetCell(oldCell.vectIndex.x, oldCell.vectIndex.y, cell);
        }

        public IEnumerable<ICell> GetBoundCells(params TerrainType[] terrainType)
        {
            var rslt = new List<ICell>();

            var content = cells.Where(x => terrainType.Contains(x.terrainType));
            foreach (var cell in content)
            {
                var nears = cell.GetNeighboursWithDirect().Where(x => x.Value != null);
                rslt.AddRange(nears.Select(x => x.Value).Where(x => !terrainType.Contains(x.terrainType)));
            }
            return rslt.Distinct();
        }

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
                BuildLake(ref map, Type2Percent[TerrainType.LAKE], terrainDefs[TerrainType.LAKE].Values);
                //BuildMarsh(ref map, 0.7);

                BuildRiver(ref map);


                LOG.INFO("build" + map.ToString());
                return map;
            }

            private static void BuildMarsh(ref MapData map, double v)
            {
                //TODO BuildMarsh
            }

            public static void BuildRiver(ref MapData map)
            {
                var random = new GTRandom();

                ICell per = null;
                var from = map.GetCell(0, random.Next(0, map.colum));

                SetRiver(ref map, from);

            }

            private static bool SetRiver(ref MapData map, ICell cur)
            {
                cur.components.Add(new TerrainComponent(TerrainCMPType.RIVER));

                if(cur.vectIndex.x == map.colum)
                {
                    return true;
                }

                var random = new GTRandom();

                var dictNear = cur.GetNeighboursWithDirect();

                var maxColum = map.colum - 1;
                var nexts = dictNear.Where(x =>
                                               x.Value != null
                                               && x.Value != cur
                                               && x.Value.vectIndex.y < maxColum && x.Value.vectIndex.y > 0);

                if(nexts.All(x=>x.Value.components.Any(c=>c.type == TerrainCMPType.RIVER)))
                {
                    return true;
                }

                do
                {
                    var lakes = nexts.Where(x => x.Value.terrainType == TerrainType.LAKE && x.Value.components.All(c => c.type != TerrainCMPType.RIVER));
                    if (lakes.Count() != 0)
                    {
                        cur = lakes.ElementAt(random.Next(0, lakes.Count())).Value;
                    }
                    else
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

                }
                while (!SetRiver(ref map, cur));

                return true;
            }

            public static int BuildLake(ref MapData map, double percent, IEnumerable<ITerrainDef> terrainDefs)
            {
                byte[] buffer = Guid.NewGuid().ToByteArray();
                Random random = new Random(BitConverter.ToInt32(buffer, 0));

                int total = (int)(percent * map.cells.Length);

                var cellcount = map.cells.Length;


                if (total < 10)
                {
                    total = 10;
                }

                var plains = map.cells.Where(x => x.terrainType == TerrainType.PLAIN).ToArray();
                IEnumerable<int> seeds = Enumerable.Range(0, total / 10).Select(x => random.Next(0, plains.Count())).Distinct().ToArray();

                foreach (var seed in seeds.Select(x=> plains[x].vectIndex))
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
                        if(bound.terrainType != TerrainType.PLAIN)
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
                            if (curr != map.Count(x => x.terrainType == TerrainType.LAKE))
                            {
                                throw new Exception();
                            }
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
                        if(bound.GetNeighbours(2).Any(x=>x.terrainType == TerrainType.MOUNT))
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

                
                if (total < 5 )
                {
                    total = 5;
                }

                IEnumerable<int> seeds = Enumerable.Range(0, total / 5 ).Select(x => random.Next(0, cellcount)).Distinct().ToArray();

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
