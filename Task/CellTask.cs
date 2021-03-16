using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fengj.Task
{
    public interface ITask : INotifyPropertyChanged
    {
        int percent { get; set; }
    }

    public class CellTask : ITask
    {
        public event PropertyChangedEventHandler PropertyChanged;

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
            this.percent = 50;
        }

        public int percent { get; set; }
    }
}
