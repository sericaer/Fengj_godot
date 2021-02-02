using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Fengj.Map;

namespace Fengj
{
    static class MapExtentions
    {
        public static Dictionary<DIRECTION, ICell> GetNeighbours(this List<ICell> self, (int x, int y) index, int column)
        {
            var rslt = new Dictionary<DIRECTION, ICell>();

            foreach (DIRECTION direct in Enum.GetValues(typeof(DIRECTION)))
            {
                var neighbour = self.GetNeighbour(index, direct, column);
                rslt.Add(direct, neighbour);
            }

            return rslt;
        }

        public static ICell TryGetCell(this List<ICell> self, (int x, int y) index, int column)
        {
            if (index.x < 0 || index.y < 0
                || index.x * column + index.y >= self.Count())
            {
                return null;
            }

            return self[index.x * column + index.y];
        }

        public static ICell GetNeighbour(this List<ICell> self, (int x, int y) index, DIRECTION direct, int column)
        {
            (int x, int y) neighbourIndex = (-1, -1);

            switch (direct)
            {
                case DIRECTION.WEST_NORTH:
                    neighbourIndex = (index.x, index.y - 1);
                    break;
                case DIRECTION.EAST_NORTH:
                    neighbourIndex = (index.x + 1, index.y - 1);
                    break;
                case DIRECTION.EAST:
                    neighbourIndex = (index.x + 1, index.y);
                    break;
                case DIRECTION.EAST_SOUTH:
                    neighbourIndex = (index.x + 1, index.y + 1);
                    break;
                case DIRECTION.WEST_SOUTH:
                    neighbourIndex = (index.x - 1, index.y + 1);
                    break;
            }

            return self.TryGetCell(neighbourIndex, column);
        }
    }
}
