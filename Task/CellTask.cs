using ReactiveMarbles.PropertyChanged;
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
        bool isFinsihed { get; }
    }

    public class CellTask : ITask
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public static Action<(int q, int r), Type> finishAction;

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

            this.WhenPropertyValueChanges(x=>x.isFinsihed).Subscribe(x=>{
                if (x)
                {
                    finishAction(p, type);
                }
            });
        }

        public int percent { get; set; }
        public bool isFinsihed => percent == 100;
    }
}
