using Fengj.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fengj.Map
{
    static class Extentions
    {
        public static IEnumerable<ICell> GetNearTerrain(this ICell self, TerrainType type, int distance, MapData map)
        {
            return self.axialCoord.GetRingWithWidth(distance, distance)
                .Where(x=>map.HasCell(x))
                .Select(x=>map.GetCell(x))
                .Where(x=>x.terrainType == type);
        }

        public static bool HasComponent(this ICell self, TerrainCMPType type)
        {
            return self.components.Any(x => x.type == type);
        }
    }
}
