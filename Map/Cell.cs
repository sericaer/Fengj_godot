using Fengj.API;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fengj.Map
{

    interface ICell
    {
        TerrainType terrainType { get; set; }

        (int x, int y) vectIndex { get; set; }

        ITerrainDef terrainDef { get; }

        IEnumerable<ICell> GetNeighbours();

        Dictionary<DIRECTION, ICell> GetNeighboursWithDirect();
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

        public Dictionary<DIRECTION, ICell> GetNeighboursWithDirect()
        {
            return map.GetNears(vectIndex.x, vectIndex.y);
        }

        public IEnumerable<ICell> GetNeighbours()
        {
            return GetNeighboursWithDirect().Values.Where(x => x != null);
        }
    }
}