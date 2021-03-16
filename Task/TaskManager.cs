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
    }

    public class TaskManager : ITaskManager
    {
        List<ITask> tasks = new List<ITask>();

        public void AddTask(CellTask cellTask)
        {
            tasks.Add(cellTask);
        }

        public void AddTask(ITask task)
        {
            tasks.Add(task);
        }

        public ITask getCellTask(int q, int r)
        {
            foreach(var elem in tasks)
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
