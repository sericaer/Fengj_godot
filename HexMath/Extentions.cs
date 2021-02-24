using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexMath
{
    public static class Extentions
    {
        public static OffsetCoord ToOffsetCoord(this AxialCoord self)
        {
            return CoordConvert.AxialToOffset(self);
        }

        public static AxialCoord ToAxialCoord(this OffsetCoord self)
        {
            return CoordConvert.OffsetToAxial(self);
        }
    }
}
