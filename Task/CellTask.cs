using Fengj.Clan;
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
    class Task : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public double percent { get { return _percent; } set { _percent = value > 100.0 ? 100.0 : value; } }

        public int difficulty { get; set; }

        public bool isFinsihed => (int)percent == 100;

        public bool isCanceled { get; set; }

        public double speed => speedDetail.Sum(x => x.value);

        public IEnumerable<(string desc, double value)> speedDetail { get; set; }

        public List<IClan> clans { get; set; }

        private double _percent;

        public void DaysInc()
        {
            percent += speed * 100 / difficulty;
        }
    }

    class CellTask : Task
    {
        internal ICell cell { get; set; }

        public enum Type
        {
            Detect
        }

        private Type type { get; set; }


        public CellTask(Type type, ICell cell, List<IClan> clans)
        {
            this.type = type;
            this.cell = cell;
            this.percent = 0;
            this.clans = clans;
            this.speedDetail = this.clans.Select(x => (x.name, x.detectSpeed));
            this.difficulty = cell.terrainDef.detectDifficulty;

            this.WhenPropertyValueChanges(x=>x.isFinsihed).Subscribe(x=>{ if (x) { OnFinsihed();} });
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
