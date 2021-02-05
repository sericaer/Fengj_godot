using Fengj.API;
using Fengj.IO;

using System;
using Newtonsoft.Json.Linq;



using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace Fengj
{
    class TerrainDef : ITerrainDef
    {
        public string modName { get; private set; }

        public string path { get; private set; }

        public TerrainType key { get; private set; }

        public static class Builder
        {
            public const string scriptPath = "/script/map/terrain/";
            public const string imagePath = "/image/map/terrain/";

            public static List<ITerrainDef>  BuildArray(string modName, string path)
            {
                var rslt = new List<ITerrainDef>();

                var scriptDirPath = path + scriptPath;

                LOG.INFO("Check Terrains path:" + scriptDirPath);

                foreach(TerrainType type in Enum.GetValues(typeof(TerrainType)))
                {
                    var scriptFilePath = scriptDirPath + type.ToString().ToLower() + ".json";
                    LOG.INFO("Load Terrains script:" + scriptFilePath);

                    rslt.Add(Build(modName, type, scriptFilePath));
                }

                CheckITerrainDefList(rslt);

                LOG.INFO("Builded Terrains count:" + rslt.Count);
                return rslt;
            }

            private static ITerrainDef Build(string modName, TerrainType type, string scriptFilePath)
            {
                TerrainDef rslt = new TerrainDef();

                rslt.modName = modName;
                rslt.key = type;
                //var json = JObject.Parse(SystemIO.FileSystem.File.ReadAllText(scriptFilePath));
                //rslt.occur = new Occur();

                rslt.path = scriptFilePath.Replace(scriptPath, imagePath).Replace("json", "png");



                //foreach (var elem in json["occur"] as JObject)
                //{
                //    rslt.occur.nearBuff.Add($"{modName}_{elem.Key}".ToUpper(), elem.Value.ToObject<double>());
                //}

                return rslt;
            }

            private static void CheckITerrainDefList(List<ITerrainDef> list)
            {
                foreach(var def in list)
                {
                    if(!SystemIO.FileSystem.File.Exists(def.path))
                    {
                        throw new FileNotFoundException(def.path);
                    }
                }
            }
        }
    }
}
