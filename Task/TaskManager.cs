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
    }

    public class TaskManager : ITaskManager
    {
        List<ITask> tasks = new List<ITask>();

        public void AddTask(ITask task)
        {
            tasks.Add(task);
        }

        public void DaysInc()
        {
            foreach (var elem in tasks)
            {
                elem.percent++;
            }
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
