//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using Fengj.API;

//namespace Fengj.Map
//{
//    public enum DIRECTION
//    {
//        WEST_NORTH,
//        EAST_NORTH,
//        NORTH,
//        EAST_SOUTH,
//        WEST_SOUTH,
//        SOUTH,
//    }

//    public enum MapBuildType
//    {
//        MAP_PLAIN,
//        MAP_SMALL_HILL,
//        MAP_BIG_HILL,
//        MAP_MOUNT
//    }

//    partial class MapData : HexMatrix<ICell> , INotifyPropertyChanged
//    {
//        public Cell changedCell { get; set; }
        
//        public MapData(int row, int colum) : base(row, colum)
//        {
//            Cell.map = this;
//        }

//        public event PropertyChangedEventHandler PropertyChanged;

//        public new  void SetCell(int index,  ICell cell)
//        {
//            cell.vectIndex = (index / colum, index % colum);
//            base.SetCell(index, cell);
//        }

//        public new void SetCell(int x, int y, ICell cell)
//        {
//            cell.vectIndex = (x, y);
//            base.SetCell(x*colum+y, cell);
//        }

//        public void ReplaceCell(ICell oldCell, ICell cell)
//        {
//            cell.vectIndex = oldCell.vectIndex;
//            base.SetCell(oldCell.vectIndex.x, oldCell.vectIndex.y, cell);
//        }

//        public IEnumerable<ICell> GetBoundCells(params TerrainType[] terrainType)
//        {
//            var rslt = new List<ICell>();

//            var content = cells.Where(x => terrainType.Contains(x.terrainType));
//            foreach (var cell in content)
//            {
//                var nears = cell.GetNeighboursWithDirect().Where(x => x.Value != null);
//                rslt.AddRange(nears.Select(x => x.Value).Where(x => !terrainType.Contains(x.terrainType)));
//            }
//            return rslt.Distinct();
//        }
//    }
//}
