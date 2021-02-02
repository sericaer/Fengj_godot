using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fengj.Facade
{
    class Facade
    {
        public Modder modder;

        public MapBuider mapBuider;

        public RunData runData;


        public void CreateModder(string modPath)
        {
            modder = Modder.Load(modPath);
        }

        public void CreateRunData(RunInit runInit)
        {
            runData = new RunData();

            runData.map = mapBuider.build(runInit.mapSize, modder.terrainDefs);
        }
    }
}

