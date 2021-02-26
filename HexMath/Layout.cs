using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace HexMath
{
    public class Layout
    {
        public Layout(Orientation orientation, Point size, Point origin)
        {
            this.orientation = orientation;
            this.size = size;
            this.origin = origin;
        }

        public readonly Orientation orientation;
        public readonly Point size;
        public readonly Point origin;
        static public Orientation pointy = new Orientation(Math.Sqrt(3.0), Math.Sqrt(3.0) / 2.0, 0.0, 3.0 / 2.0, Math.Sqrt(3.0) / 3.0, -1.0 / 3.0, 0.0, 2.0 / 3.0, 0.5);
        static public Orientation flat = new Orientation(3.0 / 2.0, 0.0, Math.Sqrt(3.0) / 2.0, Math.Sqrt(3.0), 2.0 / 3.0, 0.0, -1.0 / 3.0, Math.Sqrt(3.0) / 3.0, 0.0);

        public Point HexToPixel(AxialCoord h)
        {
            Orientation M = orientation;
            double x = (M.f0 * h.q + M.f1 * h.r) * size.x;
            double y = (M.f2 * h.q + M.f3 * h.r) * size.y;
            return new Point(x + origin.x, y + origin.y);
        }


        public AxialCoord PixelToHex(Point p)
        {
            Orientation M = orientation;
            Point pt = new Point((p.x - origin.x) / size.x, (p.y - origin.y) / size.y);
            double q = M.b0 * pt.x + M.b1 * pt.y;
            double r = M.b2 * pt.x + M.b3 * pt.y;
            return new AxialCoord((int)(Math.Round(q)), (int)(Math.Round(r)));
        }

    }
}
