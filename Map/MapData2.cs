using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Fengj.API;
using HexMath;

namespace Fengj.Map
{
    partial class MapData2 : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Cell changedCell { get; set; }

        public int maxDist;

        public Cell center => GetCell(0, 0);

        private Dictionary<AxialCoord, Cell> cells;

        public MapData2(int maxDist)
        {
            this.cells = new Dictionary<AxialCoord, Cell>();
            this.maxDist = maxDist;
        }

        public void AddCell(Cell cell)
        {
            cells.Add(cell.axialCoord, cell);
        }

        public Cell GetCell(AxialCoord cood)
        {
            return cells[cood];
        }

        public Cell GetCell(int q, int r)
        {
            return GetCell(new AxialCoord(q, r));
        }

        //public new  void SetCell(int index,  ICell cell)
        //{
        //    cell.vectIndex = (index / colum, index % colum);
        //    base.SetCell(index, cell);
        //}

        //public new void SetCell(int x, int y, ICell cell)
        //{
        //    cell.vectIndex = (x, y);
        //    base.SetCell(x*colum+y, cell);
        //}

        //public void ReplaceCell(ICell oldCell, ICell cell)
        //{
        //    cell.vectIndex = oldCell.vectIndex;
        //    base.SetCell(oldCell.vectIndex.x, oldCell.vectIndex.y, cell);
        //}

        //public IEnumerable<ICell> GetBoundCells(params TerrainType[] terrainType)
        //{
        //    var rslt = new List<ICell>();

        //    var content = cells.Where(x => terrainType.Contains(x.terrainType));
        //    foreach (var cell in content)
        //    {
        //        var nears = cell.GetNeighboursWithDirect().Where(x => x.Value != null);
        //        rslt.AddRange(nears.Select(x => x.Value).Where(x => !terrainType.Contains(x.terrainType)));
        //    }
        //    return rslt.Distinct();
        //}
    }
}
