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

        public Dictionary<TerrainType, Dictionary<string, ITerrainDef>> dictTerrainDefs;

        private List<IMod> mods;

        public ModManager()
        {
            mods = new List<IMod>();
        }

        public static ModManager Load(string path, IModBuilder builder)
        {
            var modder = new ModManager();

            foreach (var subpath in SystemIO.FileSystem.Directory.EnumerateDirectories(path))
            {
                modder.mods.Add(builder.Build(subpath));
            }

            modder.dictTerrainDefs = MergeTerrainDefs(modder.mods.SelectMany(x => x.terrainDefs));

            return modder;
        }

        private static Dictionary<TerrainType, Dictionary<String, ITerrainDef>> MergeTerrainDefs(IEnumerable<ITerrainDef> terrainDefs)
        {
            var rslt = new Dictionary<TerrainType, Dictionary<String, ITerrainDef>>();
            foreach(var elem in terrainDefs)
            {
                if(!rslt.ContainsKey(elem.type))
                {
                    rslt[elem.type] = new Dictionary<string, ITerrainDef>();
                }

                if(rslt[elem.type].ContainsKey(elem.code))
                {
                    throw new Exception();
                }

                rslt[elem.type][elem.code] = elem;
            }
            return rslt;
        }

    }
}
