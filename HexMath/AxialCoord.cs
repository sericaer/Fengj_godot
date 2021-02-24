using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexMath
{
    public class AxialCoord : Coord<AxialCoord>
    {
        public AxialCoord(int q, int r)
        {
            this.q = q;
            this.r = r;
        }

        public readonly int q;
        public readonly int r;

        public int s => (q + r) * -1;

        public static bool operator == (AxialCoord left, AxialCoord right)
        {
            return left.q == right.q && left.r == right.r;
        }

        public static bool operator !=(AxialCoord left, AxialCoord right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public AxialCoord Add(AxialCoord b)
        {
            return new AxialCoord(q + b.q, r + b.r);
        }

        public AxialCoord Sub(AxialCoord b)
        {
            return new AxialCoord(q - b.q, r - b.r);
        }

        public AxialCoord Scale(int k)
        {
            return new AxialCoord(q * k, r * k);
        }

        public IEnumerable<AxialCoord> GetNeighbors()
        {
            return Enumerable.Range(0, 6).Select(x => GetNeighbor(x));
        }

        public AxialCoord RotateLeft()
        {
            return new AxialCoord(-s, -q);
        }

        public AxialCoord RotateRight()
        {
            return new AxialCoord(-r, -s);
        }

        static public List<AxialCoord> directions = new List<AxialCoord> { new AxialCoord(1, 0), new AxialCoord(1, -1), new AxialCoord(0, -1), new AxialCoord(-1, 0), new AxialCoord(-1, 1), new AxialCoord(0, 1) };

        static public AxialCoord Direction(int direction)
        {
            return AxialCoord.directions[direction];
        }

        public AxialCoord GetNeighbor(int direction)
        {
            return Add(AxialCoord.Direction(direction));
        }

        static public List<AxialCoord> diagonals = new List<AxialCoord> { new AxialCoord(2, -1), new AxialCoord(1, -2), new AxialCoord(-1, -1), new AxialCoord(-2, 1), new AxialCoord(-1, 2), new AxialCoord(1, 1) };

        public AxialCoord DiagonalNeighbor(int direction)
        {
            return Add(AxialCoord.diagonals[direction]);
        }

        public int Length()
        {
            return (int)((Math.Abs(q) + Math.Abs(r) + Math.Abs(s)) / 2);
        }

        public int Distance(AxialCoord b)
        {
            return Sub(b).Length();
        }

        public IEnumerable<AxialCoord> GetRing(int distance)
        {
            var rslt = new List<AxialCoord>();

            if(distance == 0)
            {
                rslt.Add(this);

                return rslt;
            }

            var curr = this;
            for(int i=0; i< distance; i++)
            {
                curr = curr.GetNeighbor(4);
            }

            for (int i=0; i<6; i++)
            {
                for(int d=0; d<distance; d++)
                {
                    rslt.Add(curr);
                    curr = curr.GetNeighbor(i);
                }
            }

            return rslt;
        }

        public IEnumerable<AxialCoord> GetRingWithWidth(int distance, int width)
        {
            return Enumerable.Range(distance, width).Select(x => GetRing(distance)).SelectMany(x=>x);
        }
    }
}
