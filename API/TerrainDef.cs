using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fengj.API
{
    public interface ModElement
    {
        string modName { get; }
    }

    public enum TerrainType
    {
        PLAIN,
        HILL,
        MOUNT,
        LAKE,
        MARSH
    }

    public enum TerrainCMPType
    {
        RIVER,
    }

    public interface ITerrainDef : ModElement
    {
        TerrainType type { get; }
        
        string code { get; }

        string path { get; }

        int detectSpeed { get; }
    }
}
