﻿using Fengj.Clan;
using Fengj.Map;
using Fengj.Task;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fengj.Facade
{
    class RunData
    {
        public MapData map;
        public ITaskManager taskManager;
        public IClanManager clanManager;

        internal void DaysInc()
        {
            taskManager.DaysInc();
        }
    }
}
