using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexMath
{
    public class CoordConvert
    {
        //ODD_Q
        const int offset = -1;


        public static AxialCoord OffsetToAxial(OffsetCoord o)
        {
            int q = o.col;
            int r = o.row - (int)((o.col + offset * (o.col & 1)) / 2);

            return new AxialCoord(q, r);
        }

        public static OffsetCoord AxialToOffset(AxialCoord h)
        {
            int col = h.q;
            int row = h.r + (int)((h.q + offset * (h.q & 1)) / 2);

            return new OffsetCoord(col, row);
        }
    }
}
