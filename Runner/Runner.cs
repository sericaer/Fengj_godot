using Fengj.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fengj
{
    class Runner
    {
        public Map map { get; set; }

        static Runner()
        {
            Map.cellsGenerator = new Map.CellsGenerator();
        }

        internal static Runner Gen(ITerrainDef[] terrainDefs)
        {
            Map.CellsGenerator.defs = terrainDefs;

            var instance = new Runner();
            instance.map = Map.Gen(200, 200);

            instance.Integrate();

            return instance;
        }

        private void Integrate()
        {

        }
    }
}
