using DynamicData;
using Fengj.IO;
using Fengj.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fengj.Task
{
    interface ITaskManager
    {
        void AddTask(ITask cellTask);

        void DaysInc();

        void OnRemoveItem(Action<ITask> act);
        void OnAddItem(Action<ITask> act);

        CellTask getCellTask(ICell cell);
    }

    class TaskManager : ITaskManager
    {
        SourceList<ITask> tasks = new SourceList<ITask>();

        public TaskManager()
        {
        }

        public void AddTask(ITask task)
        {
            tasks.Add(task);
        }

        public void OnAddItem(Action<ITask> act)
        {
            tasks.Connect().OnItemAdded(act).Subscribe();
        }

        public void OnRemoveItem(Action<ITask> act)
        {
            tasks.Connect().OnItemRemoved(act).Subscribe();
        }

        public void DaysInc()
        {
            foreach (var elem in tasks.Items)
            {
                elem.DaysInc();
            }

            var needRemove = tasks.Items.Where(x => x.isFinsihed).ToArray();
            tasks.RemoveMany(needRemove);
        }

        public CellTask getCellTask(ICell cell)
        {
            foreach(var elem in tasks.Items)
            {
                if(elem is  CellTask cellTask)
                {
                    if(cellTask.cell == cell)
                    {
                        return cellTask as CellTask;
                    }
                }
            }

            return null;
        }
    }
}
