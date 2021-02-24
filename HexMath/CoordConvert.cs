using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexMath
{
    public class CoordConvert
    {
        //ODD_R
        const int offset = -1;

        public static AxialCoord OffsetToAxial(OffsetCoord o)
        {
            int q = o.col - (int)((o.row + offset * (o.row & 1)) / 2);
            int r = o.row;
            int s = -q - r;

            return new AxialCoord(q, r);
        }

        public static OffsetCoord AxialToOffset(AxialCoord h)
        {
            int col = h.q + (int)((h.r + offset * (h.r & 1)) / 2);
            int row = h.r;

            return new OffsetCoord(col, row);
        }
    }
}
