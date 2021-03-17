using Fengj.Map;
using Fengj.Modder;
using Fengj.IO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Abstractions;
using Fengj.Task;
using Fengj.Clan;

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

            //<<<<<<< HEAD
            runData.map = MapData.Buider.build(runInit.mapBuildType, 50, modder.dictTerrainDefs);
            var detectdCells = runData.map.center.axialCoord.GetRingWithWidth(0, 3).Select(x => runData.map.GetCell(x));
            foreach (var cell in detectdCells)
            {
                cell.detectType = DetectType.TERRAIN_VISIBLE;
            }

            runData.taskManager = new TaskManager();
            runData.clanManager = new ClanManager();

            //=======
            //runData.map = MapData.Buider.build(runInit.mapBuildType, 10, modder.dictTerrainDefs);
            //runData.map.center.detectType = DetectType.TERRAIN_VISIBLE;
            //>>>>>>> master
        }
    }
}

