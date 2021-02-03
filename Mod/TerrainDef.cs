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

        public string fileName { get; private set; }

        public string key => $"{modName}_{fileName}".ToUpper();

        private Occur occur;

        public double CalcOccur(IEnumerable<string> nears)
        {
            return occur.Calc(nears);
        }

        private class Occur
        {
            public double baseValue;
            public Dictionary<string, double> nearBuff;

            public Occur()
            {
                nearBuff = new Dictionary<string, double>();
            }

            public double Calc(IEnumerable<string> nears)
            {
                return baseValue + nears.Where(x => nearBuff.ContainsKey(x)).Sum(y => nearBuff[y]);
            }
        }

        public static class Builder
        {
            public const string scriptPath = "/script/map/terrain/";
            public const string imagePath = "/image/map/terrain/";

            public static List<ITerrainDef>  BuildArray(string modName, string path)
            {
                var rslt = new List<ITerrainDef>();

                var scriptDirPath = path + scriptPath;

                LOG.INFO("Check Terrains path:" + scriptPath);

                if (Directory.Exists(scriptPath))
                {
                    foreach (var scriptFilePath in Directory.EnumerateFiles(scriptDirPath, "*.json"))
                    {
                        rslt.Add(Build(modName, scriptFilePath));
                    }
                }

                CheckITerrainDefList(rslt);

                LOG.INFO("Builded Terrains count:" + rslt.Count);
                return rslt;
            }

            private static ITerrainDef Build(string modName, string scriptFilePath)
            {
                TerrainDef rslt = new TerrainDef();

                rslt.modName = modName;
                rslt.fileName = SystemIO.FileSystem.Path.GetFileNameWithoutExtension(scriptFilePath);
                rslt.path = scriptFilePath.Replace(scriptPath, imagePath).Replace("json", "png");

                var json = JObject.Parse(SystemIO.FileSystem.File.ReadAllText(scriptFilePath));
                rslt.occur = new Occur();

                foreach (var elem in json["occur"] as JObject)
                {
                    rslt.occur.nearBuff.Add($"{modName}_{elem.Key}".ToUpper(), elem.Value.ToObject<double>());
                }

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
