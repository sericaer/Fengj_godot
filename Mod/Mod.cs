using Fengj.API;
using Fengj.IO;

using System;
using System.Collections.Generic;
using System.IO;


namespace Fengj.Modder
{
    interface IMod
    {
        string name { get; }
        string path { get; }
        List<ITerrainDef> terrainDefs { get; }
    }

    class Mod: IMod
    {
        public List<ITerrainDef> terrainDefs { get; private set; }

        public string path { get; private set; }

        public string name => Path.GetFileName(path);

        public class Builder : IModBuilder
        {
            public IMod Build(string path)
            {
                LOG.INFO("Build MOD : " + path);

                var mod = new Mod();

                mod.path = path;
                mod.terrainDefs = TerrainDef.Builder.BuildArray(mod.name, path);

                return mod;
            }
        }


    }
}
