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
    public interface ITerrainOccur : ModElement
    {
        string key { get; }
        string path { get; }

        double height { get; }

        double CalcOccur(IEnumerable<string> near);
    }

    public interface ITerrainDef : ITerrainOccur
    {

    }
}
