using Fengj.Clan;
using ReactiveMarbles.PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fengj.Task
{
    class TaskData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public double percent { get { return _percent; } set { _percent = value > 100.0 ? 100.0 : value; } }

        public int difficulty { get; set; }

        public bool isFinsihed => (int)percent == 100;

        public bool isCanceled { get; set; }

        public double speed => speedDetail.Sum(x => x.value);

        public IEnumerable<(string desc, double value)> speedDetail { get; set; }

        public List<IClan> clans { get; set; }

        protected Action OnFinsihed;

        private double _percent;

        public TaskData()
        {
            this.WhenPropertyValueChanges(x => x.isFinsihed).Subscribe(x => { if (x) { OnFinsihed?.Invoke(); } });
        }

        public void DaysInc()
        {
            percent += speed * 100 / difficulty;
        }
    }
}
