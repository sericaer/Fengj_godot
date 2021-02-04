using Fengj.API;
using System;
using System.Collections.Generic;

namespace Fengj.Map
{

    interface ICell
    {
        TerrainType terrainType { get; set; }

        (int x, int y) vectIndex { get; set; }

        ITerrainDef terrainDef { get; }

        Dictionary<DIRECTION, ICell> GetNeighbours();
    }

    class Cell : ICell
    {
        public static MapData map;

        public static Func<TerrainType, ITerrainDef> funcGetTerrainDef;

        public (int x, int y) vectIndex { get; set; }

        public TerrainType terrainType { get; set; }

        public ITerrainDef terrainDef => funcGetTerrainDef(terrainType);

        public Cell(TerrainType type)
        {
            this.terrainType = type;
        }

        public Dictionary<DIRECTION, ICell> GetNeighbours()
        {
            return map.GetNears(vectIndex.x, vectIndex.y);
        }
    }
}