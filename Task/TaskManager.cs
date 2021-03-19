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
        public static TaskManager inst
        {
            get
            {
                if(_inst  ==  null)
                {
                    throw new Exception();
                }

                return _inst;
            }
        }

        public TaskManager()
        {
            _inst = this;
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

        private static TaskManager _inst;

        private SourceList<ITask> tasks = new SourceList<ITask>();
    }
}
