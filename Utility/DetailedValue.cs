using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class IntDetailedValue : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int value => detail.Sum(x => x.value);
        public IEnumerable<(string desc, int value)> detail { get; set; }

    }

    public class DoubleDetailedValue : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public double value => detail.Sum(x => x.value);
        public IEnumerable<(string desc, double value)> detail { get; set; }
    }
}
