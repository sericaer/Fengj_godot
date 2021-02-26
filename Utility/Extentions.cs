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
            if(count > iter.Count())
            {
                throw new Exception();
            }

            var random = new GTRandom();
            var index = Enumerable.Range(0, iter.Count()).OrderBy(_ => random.Next(0, 100)).Take(count);

            return index.Select(x => iter.ElementAt(x));
        }
    }
}
