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

        public static IEnumerable<T> RandomFetch<T>(this IEnumerable<T> iter, int count)
        {
            var random = new GTRandom();
            return Enumerable.Range(0, count).Select(_ => iter.ElementAt(random.Next(0, iter.Count())));
        }
    }
}
