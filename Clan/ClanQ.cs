using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fengj.Clan
{
    class ClanQ : IClan
    {
        public string name { get; set; }

        public string origin { get; set; }

        public int popNum { get; set; }

        public string key => name + origin;

        public double detectSpeed => 2.0;
    }
}
