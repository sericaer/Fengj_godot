using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fengj.Utility
{
    public static class Extentions
    {
        public static T RandomOne<T>(this IEnumerable<T> iter)
        {
            return iter.OrderBy(a => Guid.NewGuid()).First();
        }

    }
}
