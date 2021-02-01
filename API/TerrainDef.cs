using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API
{
    public interface ITerrainOccur
    {
        string key { get; }
        string path { get; }
        double CalcOccur(IEnumerable<string> near);
    }

    public interface ITerrainDef : ITerrainOccur
    {

    }

    public class TerrainDef : ITerrainDef
    {
        public string key { get; set; }

        public string path { get; set; }

        public Occur occur;

        public double CalcOccur(IEnumerable<string> nears)
        {
            return occur.Calc(nears);
        }

        public class Occur
        {
            public double baseValue;
            public Dictionary<string, double> nearBuff;

            public Occur()
            {
                baseValue = 0;
                nearBuff = new Dictionary<string, double>();
            }

            public double Calc(IEnumerable<string> nears)
            {
                return baseValue + nears.Where(x => nearBuff.ContainsKey(x)).Sum(y => nearBuff[y]);
            }
        }
    }
}
