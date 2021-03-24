using DynamicData;
using System.Collections.Generic;
using System.Linq;

namespace Fengj.Clan
{
    class ClanBase
    {
        public string name { get; set; }
        public string origin { get; set; }

        public string key => name + origin;

        public int popNum { get; set; }

        public double detectSpeed => detectSpeedDetail.Sum(x => x.value);

        public IEnumerable<(string desc, double value)> detectSpeedDetail { get; set; }

        public SourceList<Trait> traits;
    }
}
