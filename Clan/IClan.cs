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

        int popNum { get; set; }
    }
}
