using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexMath
{
    //ODD_R 
    public class OffsetCoord : Coord<OffsetCoord>
    {
        public enum DIRECTION
        {
            EAST_SOUTH = 0,
            EAST_NORTH = 1,
            NORTH = 2,
            WEST_NORTH = 3,
            WEST_SOUTH = 4,
            SOUTH = 5
        }

        public OffsetCoord(int col, int row)
        {
            this.col = col;
            this.row = row;
        }

        public readonly int col;
        public readonly int row;

        public OffsetCoord Add(OffsetCoord b)
        {
            return CoordConvert.AxialToOffset(CoordConvert.OffsetToAxial(this).Add(CoordConvert.OffsetToAxial(b)));
        }

        public OffsetCoord Sub(OffsetCoord b)
        {
            return CoordConvert.AxialToOffset(CoordConvert.OffsetToAxial(this).Add(CoordConvert.OffsetToAxial(b)));
        }

        public OffsetCoord GetNeighbor(DIRECTION direct)
        {
            return CoordConvert.AxialToOffset(CoordConvert.OffsetToAxial(this).GetNeighbor((int)direct));
        }

        public Dictionary<DIRECTION, OffsetCoord> GetNeighbors()
        {
            return Enum.GetValues(typeof(DIRECTION)).Cast<DIRECTION>()
                    .ToDictionary(k => k, v => CoordConvert.AxialToOffset(CoordConvert.OffsetToAxial(this).GetNeighbor((int)v)));
        }
    }
}
