using Fengj.API;
using Fengj.IO;

using System;
using Newtonsoft.Json.Linq;



using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace Fengj.Modder
{
    class TerrainDef : ITerrainDef
    {
        public const string scriptPath = "/script/map/terrain/";

        public const string imagePath = "/image/map/terrain/";

        public string modName { get; set; }

        public string path { get;  set; }

        public TerrainType type { get;  set; }

        public string code { get;  set; }

        public int detectDifficulty { get; set; }

        public static class Builder
        {
  
            public static List<ITerrainDef>  BuildArray(string modName, string path)
            {
                var rslt = new List<ITerrainDef>();

                var scriptDirPath = path + scriptPath;

                LOG.INFO("Check Terrains path:" + scriptDirPath);

                foreach(TerrainType type in Enum.GetValues(typeof(TerrainType)))
                {
                    var scriptFilePath = scriptDirPath + type.ToString().ToLower() + ".json";

                    LOG.INFO("Load Terrains script:" + scriptFilePath);

                    var pngDirPath = path + imagePath + type.ToString();
                    var pngFilePaths = SystemIO.FileSystem.Directory.EnumerateFiles(pngDirPath, "*.png");
                    if(pngFilePaths.Count() == 0)
                    {
                        throw new Exception();
                    }

                    foreach (var pngFilePath in pngFilePaths)
                    {
                        LOG.INFO("Load Terrains png:" + pngFilePath);

                        rslt.Add(Build(modName, type, scriptFilePath, pngFilePath));
                    }
                }

                LOG.INFO("Builded Terrains count:" + rslt.Count);
                return rslt;
            }

            private static ITerrainDef Build(string modName, TerrainType type, string scriptFilePath, string pngFilePath)
            {
                TerrainDef rslt = new TerrainDef();


                rslt.modName = modName;
                rslt.type = type;
                var json = JObject.Parse(SystemIO.FileSystem.File.ReadAllText(scriptFilePath));
                //rslt.occur = new Occur();
                rslt.code = SystemIO.FileSystem.Path.GetFileNameWithoutExtension(pngFilePath);
                rslt.path = pngFilePath;
                rslt.detectDifficulty = (int)json["detect_difficulty"];

                LOG.INFO($"Build TerrainDef, type:{rslt.type} code:{rslt.code} path:{rslt.path}");
                //foreach (var elem in json["occur"] as JObject)
                //{
                //    rslt.occur.nearBuff.Add($"{modName}_{elem.Key}".ToUpper(), elem.Value.ToObject<double>());
                //}

                return rslt;
            }
        }
    }
}
