using Fengj.Map;
using Fengj.Modder;
using Fengj.IO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Abstractions;

namespace Fengj.Facade
{
    class Facade
    {

        public ModManager modder;

        public RunData runData;

        public IModBuilder modBuilder;

        public static Action<string> logger
        {
            set
            {
                LOG.logger = value;
            }
        }

        public static void InitStatic()
        {
            SystemIO.FileSystem = new FileSystem();
        }


        public Facade()
        {
            modBuilder = new Mod.Builder();
        }

        public void CreateModder(string modPath)
        {
            modder = ModManager.Load(modPath, modBuilder);

            Cell.funcGetTerrainDef = (type, code) => modder.dictTerrainDefs[type][code];
        }

        public void CreateRunData(RunInit runInit)
        {
            runData = new RunData();

            runData.map = MapData2.Buider.build(runInit.mapBuildType, 50, modder.dictTerrainDefs);
        }
    }
}

