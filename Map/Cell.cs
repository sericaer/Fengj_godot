using System.Collections.Generic;

namespace Fengj.Map
{

    interface ICell
    {
        string terrainKey { get; }

        (int x, int y) index { get; }

        Dictionary<Map.DIRECTION, ICell> GetNeighbours();
    }

    class Cell : ICell
    {
        public static MapData map;

        public (int x, int y) index { get; set; }

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