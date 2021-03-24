using Fengj.Clan;
using Fengj.IO;
using Fengj.Map;
using ReactiveMarbles.PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fengj.Task
{
    class CellDetectTask : TaskData
    {
        internal ICell cell { get; set; }

        public CellDetectTask(ICell cell, List<ClanBase> clans)
        {
            this.cell = cell;
            this.percent = 0;
            this.clans = clans;
            this.speedDetail = this.clans.Select(x => (x.name, x.detectSpeed));
            this.difficulty = cell.terrainDef.detectDifficulty;
            this.OnFinsihed = () =>
            {
                cell.detectType = DetectType.TERRAIN_VISIBLE;
            };
        }
    }
}
