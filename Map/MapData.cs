using System;
using System.Collections.Generic;
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
        EAST,
        EAST_SOUTH,
        WEST_SOUTH,
        WEST,
    }

    public enum MapBuildType
    {
        MAP_PLAIN,
        MAP_SMALL_HILL,
        MAP_BIG_HILL,
        MAP_MOUNT
    }

    class MapData : HexMatrix<ICell>
    {
        public MapData(int row, int colum) : base(row, colum)
        {
            Cell.map = this;
        }

        public new  void SetCell(int index,  ICell cell)
        {
            cell.vectIndex = (index % colum, index / colum);
            base.SetCell(index, cell);
        }

        public new void SetCell(int x, int y, ICell cell)
        {
            cell.vectIndex = (x, y);
            base.SetCell(y*colum+x, cell);
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
                //BuildLake(ref map, 0.1);
                //BuildRiver(ref map, 0.1);
                //BuildMarsh(ref map, 0.1);

                LOG.INFO("build" + map.ToString());
                return map;
            }

            private static void BuildMarsh(ref MapData map, double v)
            {
                //todo
            }

            private static void BuildRiver(ref MapData map, double v)
            {
                //todo
            }

            private static void BuildLake(ref MapData map, double v)
            {
                //todo
            }

            private static void BuildForest(ref MapData map, double v)
            {
                //todo
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

                if(total < 10 )
                {
                    throw new Exception();
                }

                IEnumerable<int> seeds = Enumerable.Range(0, total / 10 ).Select(x => random.Next(0, cellcount)).Distinct().ToArray();

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
                            { TerrainType.PLAIN, 0.9 },
                            { TerrainType.HILL, 0.099 },
                            { TerrainType.MOUNT, 0.001 },
                        };
                    case MapBuildType.MAP_SMALL_HILL:
                        return new Dictionary<TerrainType, double>(){
                            { TerrainType.PLAIN, 0.5 },
                            { TerrainType.HILL, 0.35 },
                            { TerrainType.MOUNT, 0.15 },
                        };

                    case MapBuildType.MAP_BIG_HILL:
                        return new Dictionary<TerrainType, double>(){
                            { TerrainType.PLAIN, 0.3 },
                            { TerrainType.HILL, 0.5 },
                            { TerrainType.MOUNT, 0.2 },
                        };

                    case MapBuildType.MAP_MOUNT:
                        return new Dictionary<TerrainType, double>(){
                            { TerrainType.PLAIN, 0.2 },
                            { TerrainType.HILL, 0.4 },
                            { TerrainType.MOUNT, 0.4 },
                        };

                    default:
                        throw new Exception();
                }
            }
        }
    }
}
