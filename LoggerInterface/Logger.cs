using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerInterface
{
    public class LOG
    {
        public static Action<string> logger;
        public static void INFO(string str)
        {
            logger?.Invoke(str);
        }
    }
}
