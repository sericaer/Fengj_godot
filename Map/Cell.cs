using Fengj.API;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fengj.Map
{

    interface ICell : IMatrixElem
    {
        TerrainType terrainType { get; set; }

        ITerrainDef terrainDef { get; }

        IEnumerable<ICell> GetNeighbours(int distance = 1);

        Dictionary<DIRECTION, ICell> GetNeighboursWithDirect();
    }

    class Cell : ICell
    {
        public static MapData map;

        public static Func<TerrainType, string, ITerrainDef> funcGetTerrainDef;

        public (int x, int y) vectIndex { get; set; }

        public TerrainType terrainType { get; set; }

        public string terrainCode;

        public ITerrainDef terrainDef => funcGetTerrainDef(terrainType, terrainCode);

        public Cell(ITerrainDef def)
        {
            this.terrainType = def.type;
            this.terrainCode = def.code;
        }

        public Cell(TerrainType type, string code)
        {
            this.terrainType = type;
            this.terrainCode = code;
        }

        public Dictionary<DIRECTION, ICell> GetNeighboursWithDirect()
        {
            return map.GetNears(vectIndex.x, vectIndex.y);
        }

        public IEnumerable<ICell> GetNeighbours(int distance)
        {
            return map.GetCellsWithDistance(vectIndex.x, vectIndex.y, distance);
        }
    }
}