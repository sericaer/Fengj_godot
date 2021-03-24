using DynamicData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fengj.Clan
{
    class ClanG : ClanBase
    {
        public ClanG()
        {
            traits.Add(new DetectTrait());
        }
    }
}
