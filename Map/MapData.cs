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

    class MapData
    {
        public int row { get; internal set; }
        public int column { get; internal set; }

        public int size => cells.Count();

        public List<ICell> cells { get; internal set; }
        

        public MapData()
        {
            Cell.map = this;
        }

        public Dictionary<DIRECTION, ICell> GetNeighbours((int x, int y) index)
        {
            var rslt = new Dictionary<DIRECTION, ICell>();

            foreach(DIRECTION direct in Enum.GetValues(typeof(DIRECTION)))
            {
                var neighbour = GetNeighbour(index, direct);
                rslt.Add(direct, neighbour);
            }

            return rslt;
        }

        public ICell TryGetCell((int x, int y) index)
        {
            if (index.x < 0 || index.y < 0
                || index.x * column + index.y >= size)
            {
                return null;
            }

            return cells[index.y * column + index.x];
        }

        public ICell GetNeighbour((int x, int y) index, DIRECTION direct)
        {
            (int x, int y) neighbourIndex = (-1, -1);

            switch (direct)
            {
                case DIRECTION.WEST_NORTH:
                    neighbourIndex = (index.x, index.y - 1);
                    break;
                case DIRECTION.EAST_NORTH:
                    neighbourIndex = (index.x+1, index.y - 1);
                    break;
                case DIRECTION.EAST:
                    neighbourIndex = (index.x+1, index.y);
                    break;
                case DIRECTION.EAST_SOUTH:
                    neighbourIndex = (index.x, index.y+1);
                    break;
                case DIRECTION.WEST_SOUTH:
                    neighbourIndex = (index.x-1, index.y+1);
                    break;
                case DIRECTION.WEST:
                    neighbourIndex = (index.x - 1, index.y);
                    break;
            }

            return TryGetCell(neighbourIndex);
        }

        public static class Buider
        {
            public static MapData build((int row, int column) mapSize, IEnumerable<ITerrainOccur> terrainDefs)
            {
                var map = new MapData();

                map.row = mapSize.row;
                map.column = mapSize.column;

                map.cells = generateCells(mapSize.row, mapSize.column, terrainDefs);

                return map;
            }

            private static List<ICell> generateCells(int row, int column, IEnumerable<ITerrainOccur> terrainDefs)
            {
                var tempCells = new List<ICell>();

                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < column; j++)
                    {
                        var nears = tempCells.GetNeighbours((i, j), column).Values.Where(x => x != null);

                        var terrainKey = "_PLAIN";

                        if (nears.Count() > 0)
                        {
                            terrainKey = CalcTerrain(nears.Select(x => x.terrainKey).ToArray(), terrainDefs);
                        }


                        tempCells.Add(new Cell(i, j, terrainKey));
                    }
                }

                return tempCells;
            }

            private static string CalcTerrain(IEnumerable<string> nearTerrainKeys, IEnumerable<ITerrainOccur> terrainDefs)
            {
                var occurDict = terrainDefs.ToDictionary(k => k.key, v => v.CalcOccur(nearTerrainKeys));

                var sumArray = occurDict.Select(x => (key: x.Key, value: x.Value * 1000 / occurDict.Values.Sum())).ToArray();

                byte[] buffer = Guid.NewGuid().ToByteArray();
                Random random = new Random(BitConverter.ToInt32(buffer, 0));

                var value = random.Next(0, 1000);

                double sum = 0;
                for (int i = 0; i < sumArray.Length; i++)
                {
                    var elem = sumArray[i];
                    sum += elem.value;

                    if (value < sum)
                    {
                        return elem.key;
                    }
                }

                return sumArray.Last().key;
            }
        }
    }


}
