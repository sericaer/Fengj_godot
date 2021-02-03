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
            public static MapData build(MapBuildType mapType,(int row, int column) mapSize, IEnumerable<ITerrainOccur> terrainDefs)
            {
                var map = new MapData();

                map.row = mapSize.row;
                map.column = mapSize.column;

                map.cells = generateCells(mapType, mapSize.row, mapSize.column, terrainDefs);

                return map;
            }

            private static List<ICell> generateCells(MapBuildType mapType, int row, int column, IEnumerable<ITerrainOccur> terrainDefs)
            {
                var tempCells = new List<ICell>();

                var orderDefs = terrainDefs.OrderBy(x => x.height);

                var Type2Percent = calcPercent(mapType, terrainDefs);

                var mapTemplate = MapTemplate.build(row, column, Type2Percent);

                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < column; j++)
                    {
                        tempCells.Add(new Cell(i, j, mapTemplate[i*column + j]));
                    }
                }


                return tempCells;
            }

            private static List<(string key, int percent)> calcPercent(MapBuildType mapType, IEnumerable<ITerrainOccur> terrainDefs)
            {
                switch(mapType)
                {
                    case MapBuildType.MAP_PLAIN:
                        return new List<(string key, int percent)>(){
                            ("NATIVE_PLAIN", 90),
                            ("NATIVE_HILL", 9),
                            ("NATIVE_MOUNT", 1),
                        };
                    case MapBuildType.MAP_SMALL_HILL:
                        return new List<(string key, int percent)>(){
                            ("NATIVE_PLAIN", 60),
                            ("NATIVE_HILL", 30),
                            ("NATIVE_MOUNT", 10),
                        };

                    case MapBuildType.MAP_BIG_HILL:
                        return new List<(string key, int percent)>(){
                            ("NATIVE_PLAIN", 40),
                            ("NATIVE_HILL", 40),
                            ("NATIVE_MOUNT", 30),
                        };

                    case MapBuildType.MAP_MOUNT:
                        return new List<(string key, int percent)>(){
                            ("NATIVE_PLAIN", 20),
                            ("NATIVE_HILL", 30),
                            ("NATIVE_MOUNT", 50),
                        };

                    default:
                        throw new Exception();
                }
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


            private class MapTemplate : HexMatrix<string>
            {
                public MapTemplate(int row, int column) : base(row, column)
                {

                }

                public static MapTemplate build(int row, int column, List<(string key, int percent)> type2Percent)
                {
                    var rslt = new MapTemplate(row, column);

                    for (int j = 0; j < column * row; j++)
                    {
                        rslt.list.Add(type2Percent.First().key);
                    }

                    List<(int x, int y)> back = new List<(int x, int y)>();

                    for (int i = 0; i < row; i++)
                    {
                        for (int j = 0; j < column; j++)
                        {
                            back.Add((i, j));
                        }
                    }

                    byte[] buffer = Guid.NewGuid().ToByteArray();
                    Random random = new Random(BitConverter.ToInt32(buffer, 0));

                    for (int index = 1; index < type2Percent.Count; index++)
                    {
                        var  newback = new List<(int x, int y)>();

                        var count = type2Percent[index].percent * row * column / 100;

                        LOG.INFO(type2Percent[index].ToString());


                        int curr = 0;
                        while (curr < count)
                        {
                            for (int m = 0; m < back.Count; m++)
                            {
                                if (curr < count)
                                {
                                    int i = m / column;
                                    int j = m % column;

                                    double occur = CalcKeyOccur(type2Percent[index].key, i, j, rslt.GetNears(i, j).Where(x => x.Value != null).Select(x => x.Value));

                                    var rd = random.Next(0, 100);
                                    if (occur > rd)
                                    {
                                        rslt.SetElem(i, j, type2Percent[index].key);
                                        curr++;

                                        newback.Add((i, j));
                                    }
                                }
                            }
                        }
                    }

                    return rslt;
                }

                public static double CalcKeyOccur(string curr, int x, int y, IEnumerable<string> value)
                {
                    var same_count = value.Where(v => v == curr).Count();
                    switch(same_count)
                    {
                        case 0:
                            return 0.01;
                        case 1:
                            return 50;
                        case 2:
                            return 80;
                        case 3:
                            return 85;
                        case 4:
                            return 90;
                        case 5:
                            return 95;
                        case 6:
                            return 100;
                        default:
                            throw new InvalidOperationException();
                    }

                }
            }
        }
    }

    public class HexMatrix<T>
    {
        int row;
        int colum;

        protected List<T> list;

        public T this[int i] => list[i];

        public HexMatrix(int row, int colum)
        {
            this.row = row;
            this.colum = colum;
            this.list = new List<T>(row * colum);
        }


        public Dictionary<DIRECTION, T> GetNears(int x, int y)
        {
            Dictionary<DIRECTION, T> rslt = new Dictionary<DIRECTION, T>();

            foreach (DIRECTION direct in Enum.GetValues(typeof(DIRECTION)))
            {
                var near = GetNear(x, y, direct);
                rslt.Add(direct, near);
            }

            return rslt;
        }

        public T GetElem(int x, int y)
        {
            if (x > colum - 1 || x < 0
                || y > row - 1 || y < 0)
            {
                throw new IndexOutOfRangeException();
            }

            return list[x * colum + y];
        }

        public T TryGetElem(int x, int y)
        {
            if (x > colum - 1 || x < 0
                || y > row - 1 || y < 0)
            {
                return default(T);
            }

            return list[x * colum + y];
        }

        public void SetElem(int x, int y, T value)
        {
            if (x > colum - 1 || x < 0
                || y > row - 1 || y < 0)
            {
                throw new IndexOutOfRangeException();
            }

            list[x * colum + y] = value;
        }


        public T GetNear(int x, int y, DIRECTION direct)
        {
            (int x, int y) neighbourIndex = (-1, -1);

            switch (direct)
            {
                case DIRECTION.WEST_NORTH:
                    neighbourIndex = (x, y - 1);
                    break;
                case DIRECTION.EAST_NORTH:
                    neighbourIndex = (x + 1, y - 1);
                    break;
                case DIRECTION.EAST:
                    neighbourIndex = (x + 1, y);
                    break;
                case DIRECTION.EAST_SOUTH:
                    neighbourIndex = (x + 1, y + 1);
                    break;
                case DIRECTION.WEST_SOUTH:
                    neighbourIndex = (x - 1, y + 1);
                    break;
            }

            return TryGetElem(neighbourIndex.x, neighbourIndex.y);
        }
    }

}
