using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexMath
{
    public interface Coord<T>
    {
        T Add(T b);
        T Sub(T b);
    }
}
