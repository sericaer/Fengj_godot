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

        public MapBuider mapBuider;

        public RunData runData;

        public static Action<string> logger
        {
            set
            {
                LOG.logger = value;
            }
        }

        public Facade()
        {
            mapBuider = new MapBuider();
        }

        public void InitIO()
        {
            SystemIO.FileSystem = new FileSystem();
        }

        public void CreateModder(string modPath)
        {
            modder = ModManager.Load(modPath);
        }

        public void CreateRunData(RunInit runInit)
        {
            runData = new RunData();

            runData.map = mapBuider.build(runInit.mapSize, modder.terrainDefs);
        }
    }
}

