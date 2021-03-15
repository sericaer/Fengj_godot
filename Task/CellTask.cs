using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fengj.Task
{
    public interface ITask
    {
        int percent { get; set; }
    }

    public class CellTask : ITask
    {
        public enum Type
        {
            Detect
        }

        public (int q, int r) coord;
        private Type type;

        public CellTask(Type type, (int q, int r) p)
        {
            this.type = type;
            this.coord = p;
            this.percent = 0;
        }

        public int percent { get; set; }
    }
}
