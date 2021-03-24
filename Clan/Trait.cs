using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fengj.Clan
{
    class Trait
    {
        public string name;
    }

    interface IEffectDetect
    {
        double value { get; set; }
    }

    class DetectTrait : Trait, IEffectDetect
    {
        public DetectTrait()
        {
            name = "DetectTrait";
            value = 1.0;
        }

        public double value { get; set; }
    }
}
