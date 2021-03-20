using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fengj.Clan
{
    interface IClan
    {
        string name { get; set; }
        string origin { get; set; }

        string key { get; }

        int popNum { get; set; }

        double detectSpeed { get; }
    }
}
