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


        public void GenerateRunData(RunInit runInit)
        {
            runData = new RunData();

            runData.map = mapBuider.build(runInit.mapSize, modder.terrainDefs);
        }

        
        public Facade(string modPath)
        {
            modder = Modder.Load(modPath);
        }
    }
}

