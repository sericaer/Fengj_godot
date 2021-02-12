using Fengj.API;
using ReactiveMarbles.PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Fengj.Map
{

    interface ICell : IMatrixElem
    {
        TerrainType terrainType { get; set; }

        ITerrainDef terrainDef { get; }
        int detectLevel { get; set; }

        IEnumerable<ICell> GetNeighbours(int distance = 1);

        Dictionary<DIRECTION, ICell> GetNeighboursWithDirect();
    }

    class Cell : ICell, INotifyPropertyChanged
    {
        public static MapData map;

        public static Func<TerrainType, string, ITerrainDef> funcGetTerrainDef;

        public (int x, int y) vectIndex { get; set; }

        public TerrainType terrainType { get; set; }

        public string terrainCode;

        public event PropertyChangedEventHandler PropertyChanged;

        public ITerrainDef terrainDef => funcGetTerrainDef(terrainType, terrainCode);

        public int detectLevel { get; set; }

        public Cell(ITerrainDef def)
        {
            this.terrainType = def.type;
            this.terrainCode = def.code;
            this.detectLevel = 0;

            Intergrate();
        }

        public Cell(TerrainType type, string code)
        {
            this.terrainType = type;
            this.terrainCode = code;
            this.detectLevel = 0;

            Intergrate();
        }

        public Dictionary<DIRECTION, ICell> GetNeighboursWithDirect()
        {
            return map.GetNears(vectIndex.x, vectIndex.y);
        }

        public IEnumerable<ICell> GetNeighbours(int distance)
        {
            return map.GetCellsWithDistance(vectIndex.x, vectIndex.y, distance);
        }

        private void Intergrate()
        {
            this.WhenPropertyChanges(x => x.detectLevel).Subscribe(_ => map.changedCell = this);
        }
    }
}