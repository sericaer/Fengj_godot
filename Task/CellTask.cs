using Fengj.IO;
using Fengj.Map;
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

        int speed { get; }

        IEnumerable<(string desc, int value)> speedDetail { get;}
            
        bool isFinsihed { get; }

        void DaysInc();
    }

    class CellTask : ITask
    {
        public event PropertyChangedEventHandler PropertyChanged;

        internal ICell cell;

        public enum Type
        {
            Detect
        }

        public (int q, int r) coord;

        private Type type;

        public int percent { get; set; }
        public bool isFinsihed => percent == 100;

        public int speed => speedDetail.Sum(x => x.value);

        public IEnumerable<(string desc, int value)> speedDetail
        {
            get
            {
                var list = new List<(string desc, int value)>();
                list.Add((cell.terrainType.ToString(), cell.terrainDef.detectSpeed));

                return list;
            }
        }

        public CellTask(Type type, ICell cell)
        {
            this.type = type;
            this.cell = cell;
            this.percent = 50;

            this.WhenPropertyValueChanges(x=>x.isFinsihed).Subscribe(x=>{ if (x) { OnFinsihed();} });
        }

        public void DaysInc()
        {
            percent += speed;
        }

        public void OnFinsihed()
        {
            switch (type)
            {
                case Type.Detect:
                    cell.detectType = DetectType.TERRAIN_VISIBLE;
                    break;
            }
        }
    }
}
