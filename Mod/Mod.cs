using Fengj.API;
using Fengj.IO;

using System;
using System.Collections.Generic;
using System.IO;


namespace Fengj.Modder
{
    partial class Mod
    {
        public List<ITerrainDef> terrainDefs;

        public string path { get; private set; }

        public string modName => Path.GetFileName(path);

        public Mod()
        {
            this.path = path;

            terrainDefs = TerrainDef.Builder.BuildArray(modName, path);
        }

        public static class Builder
        {
            public static Mod Build(string path)
            {
                LOG.INFO("Build MOD : " + path);

                var mod = new Mod();

                mod.path = path;
                mod.terrainDefs = TerrainDef.Builder.BuildArray(mod.modName, path);

                return mod;
            }
        }


    }
}
