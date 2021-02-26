using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexMath
{
    class FractionalHex
    {
        public FractionalHex(double q, double r, double s)
        {
            this.q = q;
            this.r = r;
            this.s = s;
            if (Math.Round(q + r + s) != 0) throw new ArgumentException("q + r + s must be 0");
        }
        public readonly double q;
        public readonly double r;
        public readonly double s;

        public AxialCoord HexRound()
        {
            int qi = (int)(Math.Round(q));
            int ri = (int)(Math.Round(r));
            int si = (int)(Math.Round(s));
            double q_diff = Math.Abs(qi - q);
            double r_diff = Math.Abs(ri - r);
            double s_diff = Math.Abs(si - s);
            if (q_diff > r_diff && q_diff > s_diff)
            {
                qi = -ri - si;
            }
            else
                if (r_diff > s_diff)
            {
                ri = -qi - si;
            }
            else
            {
                si = -qi - ri;
            }
            return new AxialCoord(qi, ri);
        }
    }
}
