using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Fengj.API;
using HexMath;

namespace Fengj.Map
{
    partial class MapData : INotifyPropertyChanged, IEnumerable<KeyValuePair<(int q, int r), ICell>>
    {
        public event PropertyChangedEventHandler PropertyChanged;


        private static MapData _inst;
        public static MapData inst
        {
            get
            {
                if(_inst == null)
                {
                    throw new Exception();
                }

                return _inst;
            }
        }

        public int maxDist;

        public ICell changedCell { get; set; }

        public ICell center => GetCell(0, 0);

        public Dictionary<(int q, int r), ICell> dictCell;

        public IEnumerable<(int q, int r)> Keys => dictCell.Keys;

        public IEnumerable<ICell> cells => dictCell.Values;

        public MapData(int maxDist)
        {
            _inst = this;

            this.maxDist = maxDist;

            this.dictCell = new Dictionary<(int q, int r), ICell>();

            var centerCoord = new AxialCoord(0, 0);
            for (int i = 0; i <= this.maxDist; i++)
            {
                var coords = centerCoord.GetRing(i);
                foreach (var coord in coords)
                {
                    dictCell.Add((coord.q, coord.r), null);
                }
            }
        }

        public void SetCell(ICell cell)
        {
            dictCell[(cell.axialCoord.q, cell.axialCoord.r)] = cell;
        }

        public ICell GetCell(AxialCoord cood)
        {
            return GetCell(cood.q, cood.r);
        }

        public ICell GetCell(int q, int r)
        {
            return dictCell[(q, r)];
        }

        public bool HasCell(AxialCoord coord)
        {
            return dictCell.ContainsKey((coord.q, coord.r));
        }

        public IEnumerator<KeyValuePair<(int q, int r), ICell>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<(int q, int r), ICell>>)dictCell).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)dictCell).GetEnumerator();
        }

        public IEnumerable<ICell> GetBoundCells(params TerrainType[] terrainType)
        {
            var coordSet = new List<AxialCoord>();

            var coords = cells.Where(x => terrainType.Contains(x.terrainType)).Select(x=>x.axialCoord);

            foreach (var coord in coords)
            {
                var nears = coord.GetNeighbors();

                var bound = nears.Where(x => !coords.Contains(x) && HasCell(x));

                coordSet.AddRange(bound);
            }

            var rslt = coordSet.Distinct().Select(x => GetCell(x));
            return rslt;
        }
    }
}
