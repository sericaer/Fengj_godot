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

    public interface ITerrainOccur : ModElement
    {
        TerrainType key { get; }
        string path { get; }

        double height { get; }

        double CalcOccur(IEnumerable<string> near);
    }

    public interface ITerrainDef : ITerrainOccur
    {

    }
}
