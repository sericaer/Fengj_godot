using API;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modder
{
    public class Modder
    {
        public ITerrainDef[] terrainDefs;

        public static Modder Load(string path)
        {
            var modder = new Modder();
            modder.terrainDefs = new ITerrainDef[]
            {
                new TerrainDef(){ key = "terrain_1", occur = new TerrainDef.Occur(){ baseValue = 1} },
                new TerrainDef(){ key = "terrain_2", occur = new TerrainDef.Occur(){ baseValue = 2} },
                new TerrainDef(){ key = "terrain_3", occur = new TerrainDef.Occur(){ baseValue = 3} },
                new TerrainDef(){ key = "terrain_4", occur = new TerrainDef.Occur(){ baseValue = 4} },
                new TerrainDef(){ key = "terrain_5", occur = new TerrainDef.Occur(){ baseValue = 5} }
            };

            return modder;
        }
    }
}
