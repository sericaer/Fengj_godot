using Fengj.Clan;
using Fengj.IO;
using Fengj.Map;
using ReactiveMarbles.PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fengj.Task
{
    interface ITask : INotifyPropertyChanged
    {
        int percent { get; set; }

        double difficulty { get; set;}

        double speed { get; }

        IEnumerable<(string desc, double value)> speedDetail { get;}

        List<IClan> clans { get; }

        bool isFinsihed { get; }

        void DaysInc();
    }

    class Task : ITask
    {
        [Range(0.0, 100.0)]
        public int percent { get; set; }

        double difficulty { get; set; }

        public bool isFinsihed => percent == 100;

        public int speed => speedDetail.Sum(x => x.value);

        public List<IClan> clans { get; set; }

        private int _percent;

        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable<(string desc, int value)> speedDetail { get; set; }
        double ITask.difficulty { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        double ITask.speed => throw new NotImplementedException();

        IEnumerable<(string desc, double value)> ITask.speedDetail => throw new NotImplementedException();

        public void DaysInc()
        {
            percent += speed;
        }
    }

    class CellTask : ITask
    {
        public event PropertyChangedEventHandler PropertyChanged;

        internal ICell cell { get; set; }

        public enum Type
        {
            Detect
        }

        private Type type { get; set; }

        public int percent
        {
            get
            {
                return _percent;
            }
            set
            {
                _percent = value < 100 ? value : 100;
            }
        }

        public bool isFinsihed => percent == 100;

        public int speed => speedDetail.Sum(x => x.value);

        public List<IClan> clans { get; set; }

        private int _percent;

        public IEnumerable<(string desc, int value)> speedDetail
        {
            get
            {
                var list = new List<(string desc, int value)>();
                list.Add((cell.terrainType.ToString(), cell.terrainDef.detectSpeed));

                return list;
            }
        }

        public double difficulty { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        double ITask.speed => throw new NotImplementedException();

        IEnumerable<(string desc, double value)> ITask.speedDetail => throw new NotImplementedException();

        public CellTask(Type type, ICell cell, List<IClan> clans)
        {
            this.type = type;
            this.cell = cell;
            this.percent = 0;
            this.clans = clans;

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
