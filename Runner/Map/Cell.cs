using System.Collections.Generic;

namespace Fengj
{
    partial class Map
    {
        public enum CellDetectLevel
        {
            LEVEL0,
            LEVEL1,
            LEVEL2
        }

        public interface ICell
        {
            string terrainKey { get; }

            (int x, int y) index { get; }

            CellDetectLevel detectLevel { get; }

            Dictionary<Map.DIRECTION, ICell> GetNeighbours();
        }

        public class Cell : ICell
        {
            public static Map map;

            public (int x, int y) index { get; set; }

            public string terrainKey { get; set; }

            public CellDetectLevel detectLevel { get; set; }

            public Cell(int x, int y, string terrainKey)
            {
                this.index = (x, y);
                this.terrainKey = terrainKey;
                this.detectLevel = CellDetectLevel.LEVEL0;
            }

            public Dictionary<DIRECTION, ICell> GetNeighbours()
            {
                return map.GetNeighbours(index);
            }
        }
    }
}