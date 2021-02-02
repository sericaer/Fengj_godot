using Fengj.API;
using System;
using Newtonsoft.Json.Linq;

using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace Fengj
{
    class TerrainDef : ITerrainDef
    {
        public string modName { get; set; }

        public string path { get; set; }

        public string key => $"{modName}_{fileName}".ToUpper();

        public string fileName => Path.GetFileNameWithoutExtension(path);

        public Occur occur;

        public TerrainDef(string filePath, string jsonStr)
        {
            path = filePath;

            var json = JObject.Parse(jsonStr);
            occur = new Occur(json["occur"] as JObject);
        }

        public double CalcOccur(IEnumerable<string> nears)
        {
            return occur.Calc(nears);
        }

        public class Occur
        {
            public double baseValue;
            public Dictionary<string, double> nearBuff;

            public Occur(JObject json)
            {
                baseValue = 0;
                nearBuff = new Dictionary<string, double>();

                foreach(var elem in json)
                {
                    nearBuff.Add(elem.Key, elem.Value.ToObject<double>());
                }
            }

            public double Calc(IEnumerable<string> nears)
            {
                return baseValue + nears.Where(x => nearBuff.ContainsKey(x)).Sum(y => nearBuff[y]);
            }
        }
    }
}
