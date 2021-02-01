using System.Collections.Generic;

namespace Runner
{
    partial class Map
    {
        public interface ICell
        {
            string terrainKey { get; }

            Dictionary<Map.DIRECTION, ICell> GetNeighbours();
        }

        class Cell : ICell
        {
            public static Map map;

            public readonly (int x, int y) index;

            public string terrainKey { get; set; }

            public Cell(int x, int y, string terrainKey)
            {
                this.index = (x, y);
                this.terrainKey = terrainKey;
            }

            public Dictionary<DIRECTION, ICell> GetNeighbours()
            {
                return map.GetNeighbours(index);
            }
        }
    }
}