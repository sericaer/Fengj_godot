using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Fengj.Map
{
    public enum DIRECTION
    {
        WEST_NORTH,
        EAST_NORTH,
        EAST,
        EAST_SOUTH,
        WEST_SOUTH,
        WEST,
    }

    class MapData
    {
        public int row { get; internal set; }
        public int column { get; internal set; }

        public int size => cells.Count();

        public List<ICell> cells { get; set; }

        public MapData()
        {
            Cell.map = this;
        }

        public Dictionary<DIRECTION, ICell> GetNeighbours((int x, int y) index)
        {
            var rslt = new Dictionary<DIRECTION, ICell>();

            foreach(DIRECTION direct in Enum.GetValues(typeof(DIRECTION)))
            {
                var neighbour = GetNeighbour(index, direct);
                rslt.Add(direct, neighbour);
            }

            return rslt;
        }

        public ICell TryGetCell((int x, int y) index)
        {
            if (index.x < 0 || index.y < 0
                || index.x * column + index.y >= size)
            {
                return null;
            }

            return cells[index.y * column + index.x];
        }

        public ICell GetNeighbour((int x, int y) index, DIRECTION direct)
        {
            (int x, int y) neighbourIndex = (-1, -1);

            switch (direct)
            {
                case DIRECTION.WEST_NORTH:
                    neighbourIndex = (index.x, index.y - 1);
                    break;
                case DIRECTION.EAST_NORTH:
                    neighbourIndex = (index.x+1, index.y - 1);
                    break;
                case DIRECTION.EAST:
                    neighbourIndex = (index.x+1, index.y);
                    break;
                case DIRECTION.EAST_SOUTH:
                    neighbourIndex = (index.x, index.y+1);
                    break;
                case DIRECTION.WEST_SOUTH:
                    neighbourIndex = (index.x-1, index.y+1);
                    break;
                case DIRECTION.WEST:
                    neighbourIndex = (index.x - 1, index.y);
                    break;
            }

            return TryGetCell(neighbourIndex);
        }
    }


}
