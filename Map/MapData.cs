using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Fengj.API;
using Fengj.IO;

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

    public enum TerrainType
    {
        PLAIN,
        HILL,
        MOUNT
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

        private void ReplaceCell(ICell oldCell, ICell cell)
        {
            cell.vectIndex = oldCell.vectIndex;
            base.SetCell(oldCell.vectIndex.x, oldCell.vectIndex.y, cell);
        }

        private IEnumerable<ICell> GetBoundCells(params TerrainType[] terrainType)
        {
            var rslt = new List<ICell>();
            foreach(var cell in cells)
            {
                if (terrainType.Contains(cell.terrainType))
                {
                    continue;
                }

                var nears = cell.GetNeighbours().Where(x => x.Value != null);
                if (nears.Select(x => x.Value).Any(x => terrainType.Contains(x.terrainType)))
                {
                    rslt.Add(cell);
                }
            }
            return rslt;
        }

        public static class Buider
        {

            public static MapData build(MapBuildType mapType, (int row, int column) mapSize, IEnumerable<ITerrainOccur> terrainDefs)
            {
                var terrainDict = new Dictionary<TerrainType, ITerrainOccur>();

                var map = new MapData(mapSize.row, mapSize.column);
                BuildPlain(ref map);

                var Type2Percent = calcPercent(mapType);
                BuildMount(ref map, Type2Percent[TerrainType.MOUNT]);
                BuildHill(ref map, Type2Percent[TerrainType.HILL]);

                //BuildForest(ref map, 0.1);
                //BuildLake(ref map, 0.1);
                //BuildRiver(ref map, 0.1);
                //BuildMarsh(ref map, 0.1);

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

            public static void BuildHill(ref MapData map, double percent)
            {
                byte[] buffer = Guid.NewGuid().ToByteArray();
                Random random = new Random(BitConverter.ToInt32(buffer, 0));

                int total = (int)(percent * map.cells.Length);

                int curr = 0;

                while (true)
                {
                    IEnumerable<ICell> bounds = map.GetBoundCells(TerrainType.MOUNT, TerrainType.HILL);
                    if (bounds.Count() == 0)
                    {
                        throw new Exception();
                    }

                    bounds = bounds.OrderBy(a => Guid.NewGuid());

                    foreach (var bound in bounds)
                    {
                        var value = random.Next(0, 10);
                        if (value == 0)
                        {
                            map.ReplaceCell(bound, new Cell(TerrainType.HILL));
                            curr++;
                        }

                        if (curr == total)
                        {
                            return;
                        }
                    }
                }
            }

            public static void BuildMount(ref MapData map, double percent)
            {
                byte[] buffer = Guid.NewGuid().ToByteArray();
                Random random = new Random(BitConverter.ToInt32(buffer, 0));
                
                int total = (int)(percent * map.cells.Length);
   
                var cellcount = map.cells.Length;
                IEnumerable<int> seeds = Enumerable.Range(0, total / 10).Select(x => random.Next(0, cellcount)).Distinct();

                foreach (var seed in seeds)
                {
                    map.ReplaceCell(map.cells[seed], new Cell(TerrainType.MOUNT));
                }

                int curr = seeds.Count();

                while (true)
                {

                    IEnumerable<ICell> bounds = map.GetBoundCells(TerrainType.MOUNT);
                    if (bounds.Count() == 0)
                    {
                        throw new Exception();
                    }

                    bounds = bounds.OrderBy(a => Guid.NewGuid());

                    foreach (var bound in bounds)
                    {
                        var value = random.Next(0, 10);
                        if (value == 0)
                        {
                            map.ReplaceCell(bound, new Cell(TerrainType.MOUNT));
                            curr++;
                        }

                        if (curr == total)
                        {
                            return;
                        }
                    }
                }
            }

            public static void BuildPlain(ref MapData map)
            {
                for (int i = 0; i < map.cells.Length; i++)
                {
                    map.SetCell(i, new Cell(TerrainType.PLAIN));
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
