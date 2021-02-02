using Fengj.API;
using System;
using System.Collections.Generic;
using Fengj.Map;
using System.Linq;

namespace Fengj.Facade
{
    class MapBuider
    {
        public MapData build((int row, int column) mapSize, IEnumerable<ITerrainOccur> terrainDefs)
        {
            var map = new MapData();

            map.row = mapSize.row;
            map.column = mapSize.column;

            map.cells = generateCells(mapSize.row, mapSize.column, terrainDefs);

            return map;
        }

        private List<ICell> generateCells(int row, int column, IEnumerable<ITerrainOccur> terrainDefs)
        {
            var tempCells = new List<ICell>();

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    var nears = tempCells.GetNeighbours((i, j), column).Values.Where(x => x != null) ;

                    var terrainKey = "_PLAIN";

                    if(nears.Count() > 0 )
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