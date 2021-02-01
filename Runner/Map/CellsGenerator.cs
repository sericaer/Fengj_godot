using API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runner
{
    partial class Map
    {
        public interface ICellsGenerator
        {
            List<ICell> generate(int row, int column);
        }

        public class CellsGenerator : ICellsGenerator
        {
            public static List<ITerrainOccur> defs { set; private get; }
            public List<ICell> generate(int row, int column)
            {
                var tempCells = new List<ICell>();
                
                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < column; j++)
                    {
                        var nears = tempCells.GetNeighbours((i, j), column);
                        var terrainKey = CalcTerrain(nears.Values.Select(x => x?.terrainKey).ToArray());

                        tempCells.Add(new Cell(i, j, terrainKey));
                    }
                }

                return tempCells;
            }

            private static string CalcTerrain(IEnumerable<string> nearTerrainKeys)
            {
                var occurDict = defs.ToDictionary(k => k.key, v => v.CalcOccur(nearTerrainKeys));

                var sumArray = occurDict.Select(x => (key:x.Key, value:x.Value * 1000 / occurDict.Values.Sum())).ToArray();

                byte[] buffer = Guid.NewGuid().ToByteArray();
                Random random = new Random(BitConverter.ToInt32(buffer, 0));

                var value = random.Next(0, 1000);

                double sum = 0;
                for(int i=0; i< sumArray.Length; i++)
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
