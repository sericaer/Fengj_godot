using Fengj.API;
using System;
using System.Collections.Generic;
using System.IO;


namespace Fengj.Modder
{
    class Mod
    {
        public List<ITerrainDef> terrainDefs;

        public readonly string path;

        public Mod(string path)
        {
            this.path = path;

            terrainDefs = LoadTerrains(path);
        }

        private List<ITerrainDef> LoadTerrains(string path)
        {
            var rslt = new List<ITerrainDef>();

            var scriptPath = path + "script/map/terrain/";
            if(!Directory.Exists(scriptPath))
            {
                return rslt;
            }

            var pngPath = path + "png/map/terrain/";
            foreach (var jsonFile in Directory.EnumerateFiles(scriptPath, "json"))
            {
                var fileName = Path.GetFileNameWithoutExtension(jsonFile);
                if (!File.Exists($"{pngPath}{fileName}.png"))
                {
                    throw new Exception(); //todo
                }

                rslt.Add(new TerrainDef(fileName, jsonFile));
            }

            //todo check

            return rslt;
        }
    }
}
