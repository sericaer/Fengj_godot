using Fengj.API;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fengj
{
    public class Modder
    {
        public ITerrainDef[] terrainDefs => mods.SelectMany(x => x.terrainDefs).ToArray();

        private List<Mod> mods;

        public Modder()
        {
            mods = new List<Mod>();
        }

        public static Modder Load(string path)
        {
            var modder = new Modder();

            foreach (var subpath in Directory.EnumerateDirectories(path))
            {
                modder.mods.Add(new Mod(subpath));
            }

            return modder;
        }
    }
}
