using DynamicData;
using Fengj.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fengj.Task
{
    public interface ITaskManager
    {
        void AddTask(ITask cellTask);
        ITask getCellTask(int q, int r);
        void DaysInc();

        void OnRemoveItem(Action<ITask> act);
        void OnAddItem(Action<ITask> act);
    }

    public class TaskManager : ITaskManager
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
                elem.percent++;
            }

            var needRemove = tasks.Items.Where(x => x.isFinsihed).ToArray();
            tasks.RemoveMany(needRemove);
        }

        public ITask getCellTask(int q, int r)
        {
            foreach(var elem in tasks.Items)
            {
                if(elem is  CellTask cellTask)
                {
                    if(cellTask.coord.q == q && cellTask.coord.r == r)
                    {
                        return cellTask;
                    }
                }
            }

            return null;
        }
    }
}
