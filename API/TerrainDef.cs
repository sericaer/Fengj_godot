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
        MOUNT
    }

    public interface ITerrainDef : ModElement
    {
        TerrainType key { get; }
        string path { get; }
    }
}
