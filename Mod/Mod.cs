using Fengj.API;
using LoggerInterface;
using System;
using System.Collections.Generic;
using System.IO;


namespace Fengj.Modder
{
    class Mod
    {
        public List<ITerrainDef> terrainDefs;

        public readonly string path;

        public string modName => Path.GetFileName(path);

        public Mod(string path)
        {
            this.path = path;

            terrainDefs = LoadTerrains(modName, path);
        }

        private List<ITerrainDef> LoadTerrains(string modName, string path)
        {
            var rslt = new List<ITerrainDef>();

            var scriptPath = path + "/script/map/terrain/";
            LOG.INFO("LoadTerrains scrpt path:" + scriptPath);

            if (Directory.Exists(scriptPath))
            {
                var pngPath = path + "/png/map/terrain/";
                foreach (var jsonFile in Directory.EnumerateFiles(scriptPath, "*.json"))
                {
                    LOG.INFO("LoadTerrains scrpt file:" + jsonFile);

                    var pngFilePath = pngPath + Path.GetFileNameWithoutExtension(jsonFile) + ".png";
                    if (!File.Exists(pngFilePath))
                    {
                        throw new Exception(); //todo
                    }

                    rslt.Add(new TerrainDef(modName, pngFilePath, File.ReadAllText(jsonFile)));
                }

                //todo check
            }



            LOG.INFO("LoadTerrains count:" + rslt.Count);
            return rslt;
        }
    }
}
