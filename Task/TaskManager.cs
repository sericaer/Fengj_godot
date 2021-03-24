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
        void AddTask(TaskData cellTask);

        void DaysInc();

        void OnRemoveItem(Action<TaskData> act);
        void OnAddItem(Action<TaskData> act);

        CellDetectTask getCellTask(ICell cell);
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

        public void AddTask(TaskData task)
        {
            tasks.Add(task);
        }

        public void Cancel(TaskData task)
        {
            task.isCanceled = true;
            tasks.Remove(task);
        }

        public void OnAddItem(Action<TaskData> act)
        {
            tasks.Connect().OnItemAdded(act).Subscribe();
        }

        public void OnRemoveItem(Action<TaskData> act)
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

        public CellDetectTask getCellTask(ICell cell)
        {
            foreach(var elem in tasks.Items)
            {
                if(elem is  CellDetectTask cellTask)
                {
                    if(cellTask.cell == cell)
                    {
                        return cellTask as CellDetectTask;
                    }
                }
            }

            return null;
        }

        private static TaskManager _inst;

        private SourceList<TaskData> tasks = new SourceList<TaskData>();


    }
}
