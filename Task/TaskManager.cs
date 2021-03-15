using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fengj.Task
{
    public interface ITaskManager
    {
        void AddTask(CellTask cellTask);
    }

    public class TaskManager : ITaskManager
    {
        List<ITask> tasks = new List<ITask>();

        public void AddTask(CellTask cellTask)
        {
            tasks.Add(cellTask);
        }
    }
}
