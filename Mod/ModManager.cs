using Fengj.API;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fengj.IO;

namespace Fengj.Modder
{
    class ModManager
    {

        public ITerrainDef[] terrainDefs => mods.SelectMany(x => x.terrainDefs).ToArray();


        private List<Mod> mods;

        public ModManager()
        {
            mods = new List<Mod>();
        }

        public static ModManager Load(string path)
        {
            var modder = new ModManager();

            foreach (var subpath in Directory.EnumerateDirectories(path))
            {
                modder.mods.Add(Mod.Builder.Build(subpath));
            }

            return modder;
        }
    }
}
