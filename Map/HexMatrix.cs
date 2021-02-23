using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fengj.Map
{
    public interface IMatrixElem
    {
        (int x, int y) vectIndex { get; set; }
    }

    public class HexMatrix<T> : IEnumerable<T> where T: IMatrixElem
    {
        public int row;
        public int colum;
        public T[] cells;

        public T this[int i] => cells[i];

        public HexMatrix(int row, int colum)
        {
            this.row = row;
            this.colum = colum;
            this.cells = Enumerable.Range(0, row * colum).Select(x => default(T)).ToArray();
        }

        public void SetCell(int index, T cell)
        {
            cells[index] = cell;
        }

        public Dictionary<DIRECTION, T> GetNears(int x, int y)
        {
            Dictionary<DIRECTION, T> rslt = new Dictionary<DIRECTION, T>();

            foreach (DIRECTION direct in Enum.GetValues(typeof(DIRECTION)))
            {
                var near = GetNearWithDirect(x, y, direct);
                rslt.Add(direct, near);
            }

            return rslt;
        }

        public T GetCell(int x, int y)
        {
            if (x > colum - 1 || x < 0
                || y > row - 1 || y < 0)
            {
                throw new IndexOutOfRangeException();
            }

            return cells[x * colum + y];
        }

        public T TryGetCell(int x, int y)
        {
            if (x > colum - 1 || x < 0
                || y > row - 1 || y < 0)
            {
                return default(T);
            }

            return cells[x * colum  + y];
        }

        public void SetCell(int x, int y, T value)
        {
            if (x > colum - 1 || x < 0
                || y > row - 1 || y < 0)
            {
                throw new IndexOutOfRangeException();
            }

            SetCell(x*colum + y, value);
        }


        public T GetNearWithDirect(int x, int y, DIRECTION direct)
        {
            (int x, int y) neighbourIndex = (-1, -1);

            switch (direct)
            {
                case DIRECTION.EAST_NORTH:
                    if(y %2 != 0)
                    {
                        neighbourIndex = (x, y + 1);
                        break;
                    }
                    else
                    {
                        neighbourIndex = (x - 1, y + 1);
                        break;
                    }
                case DIRECTION.EAST_SOUTH:
                    if (y % 2 != 0)
                    {
                        neighbourIndex = (x+1, y+1);
                        break;
                    }
                    else
                    {
                        neighbourIndex = (x, y+1);
                        break;
                    }
                case DIRECTION.SOUTH:
                    neighbourIndex = (x + 1, y);
                    break;
                case DIRECTION.WEST_SOUTH:
                    if (y % 2 != 0)
                    {
                        neighbourIndex = (x+1, y-1);
                        break;
                    }
                    else
                    {
                        neighbourIndex = (x, y - 1);
                        break;
                    }
                case DIRECTION.WEST_NORTH:
                    if (y % 2 != 0)
                    {
                        neighbourIndex = (x, y - 1);
                    }
                    else
                    {
                        neighbourIndex = (x-1, y - 1);
                    }
                    break;
                case DIRECTION.NORTH:
                    neighbourIndex = (x - 1, y);
                    break;
            }

            return TryGetCell(neighbourIndex.x, neighbourIndex.y);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)cells).GetEnumerator();
        }

        public IEnumerable<T> GetCellsWithDistance(int x, int y, int distance)
        {
            if (distance < 0)
            {
                throw new Exception();
            }

            List<T> list = new List<T>();

            var nears = GetNears(x, y);
            var cells = nears.Values.Where(v => v != null);
            list.AddRange(cells);

            if (distance > 1)
            {
                list.AddRange(cells.SelectMany(n => GetCellsWithDistance(n.vectIndex.x, n.vectIndex.y,  distance - 1)).Distinct());
            }

            list.RemoveAll(n => n.vectIndex == (x, y));
            return list.Distinct();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return cells.GetEnumerator();
        }
    }
}
