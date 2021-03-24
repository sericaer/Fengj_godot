using DynamicData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Fengj.Clan
{
    class ClanBase : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public string name { get; set; }
        public string origin { get; set; }

        public string key => name + origin;

        public int popNum { get; set; }

        public double detectSpeed => detectSpeedDetail.Sum(x => x.value);

        public IEnumerable<(string desc, double value)> detectSpeedDetail { get; set; }

        public SourceList<Trait> traits;


        public ClanBase()
        {
            traits = new SourceList<Trait>();

            var traisObservableList = traits.Connect().AsObservableList();
            traisObservableList.CountChanged.Subscribe(_ => detectSpeedDetail = traits.Items.OfType<IEffectDetect>().Select(x => (((Trait)x).name, x.value)));
        }
    }
}
