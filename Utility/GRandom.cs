using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fengj.Utility
{
        public class GRandom
        {
            private static System.Random ra = new System.Random(GetRandomSeed());

            public static bool isOccur(double prob)
            {
                if (Math.Abs(prob) < Double.Epsilon)
                {
                    return false;
                }

                if (prob >= 100)
                {
                    return true;
                }

                if (prob.CompareTo(0.0) == 0)
                {
                    return true;
                }

                int prb = (int)(prob * 100);

                int result = ra.Next(1, 10000);
                if (result <= prb)
                {
                    return true;
                }

                return false;
            }

            public static string CalcGroup(IEnumerable<(string name, double prob)> groups)
            {
                var sum = groups.Sum(x => x.prob);

                var range = groups.Select(x => (name: x.name, Value: x.prob * 100 / sum)).ToArray();

                int raValue = ra.Next(1, 100);

                double start = 0;
                for (int i = 0; i < range.Count(); i++)
                {
                    double end = start + range[i].Value;
                    if (raValue >= start && raValue < end)
                    {
                        return range[i].name;
                    }

                    start = end;
                }

                throw new Exception();
            }

            //public static string RandomGroup((string name, double prob)[] groups)
            //{
            //    var query = groups.Where(x => x.prob > 0.0).Select(x => (name: x.name, index: (int)(x.prob * 1000))).ToArray();
            //    for (int i = 0; i < query.Length; i++)
            //    {
            //        if (i == 0)
            //        {
            //            continue;
            //        }

            //        query[i].index += query[i - 1].index;
            //    }

            //    System.Random ra = new System.Random(GetRandomSeed());
            //    int result = ra.Next(1, query.Last().index + 1);

            //    if (result <= query[0].index)
            //    {
            //        return query[0].name;
            //    }

            //    for (int i = 0; i < query.Length; i++)
            //    {
            //        if (i == 0)
            //        {
            //            continue;
            //        }

            //        if (result > query[i - 1].index && result <= query[i].index)
            //        {
            //            return query[i].name;
            //        }
            //    }

            //    throw new Exception();
            //}

            //internal static int GetRandomNum((int min, int max) num)
            //{
            //    return getNum(num.min, num.max);
            //}

            ////public static void ProbAction(double prob1, Action action1,
            ////                              double prob2, Action action2)
            ////{
            ////    if (Math.Abs(prob1 + prob2) < Double.Epsilon)
            ////    {
            ////        return;
            ////    }

            ////    int prb1 = (int)(prob1 * 1000);
            ////    int prb2 = (int)(prob2 * 1000);

            ////    System.Random ra = new System.Random(GetRandomSeed());
            ////    int result = ra.Next(1, 1000);
            ////    if (result <= prb1)
            ////    {
            ////        action1();
            ////        return;
            ////    }
            ////    if (result <= prb1 + prb2)
            ////    {
            ////        action2();
            ////        return;
            ////    }
            ////}

            public static int getNum(int min, int max)
            {
                if (min == max)
                {
                    return min;
                }

                System.Random ra = new System.Random(GetRandomSeed());
                return ra.Next(min, max);
            }

            public static T get<T>(IEnumerable<T> enumerable)
            {
                if (enumerable == null)
                {
                    throw new ArgumentNullException(nameof(enumerable));
                }

                // note: creating a Random instance each call may not be correct for you,
                // consider a thread-safe static instance
                var r = new Random(GetRandomSeed());
                var list = enumerable as IList<T> ?? enumerable.ToList();
                return list.Count == 0 ? default(T) : list[r.Next(0, list.Count)];
            }

            //public static int getGaussianNum(int mu, int sigma = 1)
            //{
            //    System.Random ra = new System.Random(GetRandomSeed());
            //    return (int)ra.NextGaussian(mu, sigma);
            //}

            //public static bool Calc(int iRate)
            //{
            //    System.Random ran = new System.Random(GetRandomSeed());
            //    int RandKey = ran.Next(1, 100);
            //    if (RandKey <= iRate)
            //    {
            //        return true;
            //    }

            //    return false;
            //}

            ////public static List<int> GetRandomNumArrayWithStableSum(int count, int sum)
            ////{
            ////    List<int> list = new List<int>();
            ////    while (list.Count != count - 1)
            ////    {
            ////        int random = GetRandomNum(0, sum);
            ////        if (list.Contains(random))
            ////        {
            ////            continue;
            ////        }

            ////        list.Add(random);
            ////    }

            ////    list.Sort();

            ////    List<int> resultList = new List<int>();
            ////    for (int i = 0; i < list.Count + 1; i++)
            ////    {
            ////        if (i == 0)
            ////        {
            ////            resultList.Add(list[i] - 0);
            ////        }
            ////        else if (i == list.Count)
            ////        {
            ////            resultList.Add(100 - list[i - 1]);
            ////        }
            ////        else
            ////        {
            ////            resultList.Add(list[i] - list[i - 1]);
            ////        }
            ////    }

            ////    return resultList;
            ////}

            ////public static void ProbGroup(double prob1, Action action1,
            ////                      double prob2, Action action2,
            ////                      double prob3 = 0.0, Action action3 = null,
            ////                      double prob4 = 0.0, Action action4 = null)
            ////{


            ////    int p1 = (int)(prob1 * 1000);
            ////    int p2 = (int)((prob2 + prob1) * 1000);
            ////    int p3 = (int)((prob3 + prob2 + prob1) * 1000);
            ////    int p4 = (int)((prob4 + prob2 + prob1) * 1000);

            ////    if (p4 > 1000)
            ////    {
            ////        throw new ArgumentException("Total prob cout > 1.0");
            ////    }

            ////    int rad = GetRandomNum(0, 1000);
            ////    if (rad < p1)
            ////    {
            ////        action1();
            ////    }
            ////    else if (rad < p2)
            ////    {
            ////        action2();
            ////    }
            ////    else if (rad < p3)
            ////    {
            ////        action3();
            ////    }
            ////    else if (rad < p4)
            ////    {
            ////        action4();
            ////    }
            ////}

            private static int GetRandomSeed()
            {
                byte[] buffer = Guid.NewGuid().ToByteArray();
                return BitConverter.ToInt32(buffer, 0);
            }
        }

        //public static class RandomExtensions
        //{
        //    /// <summary>
        //    ///   Generates normally distributed numbers. Each operation makes two Gaussians for the price of one, and apparently they can be cached or something for better performance, but who cares.
        //    /// </summary>
        //    /// <param name="r"></param>
        //    /// <param name = "mu">Mean of the distribution</param>
        //    /// <param name = "sigma">Standard deviation</param>
        //    /// <returns></returns>
        //    public static double NextGaussian(this Random r, double mu = 0, double sigma = 1)
        //    {
        //        var u1 = r.NextDouble();
        //        var u2 = r.NextDouble();

        //        var rand_std_normal = Math.Sqrt(-2.0 * Math.Log(u1)) *
        //                            Math.Sin(2.0 * Math.PI * u2);

        //        var rand_normal = mu + sigma * rand_std_normal;

        //        return rand_normal;
        //    }

        //    /// <summary>
        //    ///   Generates values from a triangular distribution.
        //    /// </summary>
        //    /// <remarks>
        //    /// See http://en.wikipedia.org/wiki/Triangular_distribution for a description of the triangular probability distribution and the algorithm for generating one.
        //    /// </remarks>
        //    /// <param name="r"></param>
        //    /// <param name = "a">Minimum</param>
        //    /// <param name = "b">Maximum</param>
        //    /// <param name = "c">Mode (most frequent value)</param>
        //    /// <returns></returns>
        //    public static double NextTriangular(this Random r, double a, double b, double c)
        //    {
        //        var u = r.NextDouble();

        //        return u < (c - a) / (b - a)
        //                   ? a + Math.Sqrt(u * (b - a) * (c - a))
        //                   : b - Math.Sqrt((1 - u) * (b - a) * (b - c));
        //    }

        //    /// <summary>
        //    ///   Equally likely to return true or false. Uses <see cref="Random.Next()"/>.
        //    /// </summary>
        //    /// <returns></returns>
        //    public static bool NextBoolean(this Random r)
        //    {
        //        return r.Next(2) > 0;
        //    }

        //    /// <summary>
        //    ///   Shuffles a list in O(n) time by using the Fisher-Yates/Knuth algorithm.
        //    /// </summary>
        //    /// <param name="r"></param>
        //    /// <param name = "list"></param>
        //    public static void Shuffle(this Random r, IList list)
        //    {
        //        for (var i = 0; i < list.Count; i++)
        //        {
        //            var j = r.Next(0, i + 1);

        //            var temp = list[j];
        //            list[j] = list[i];
        //            list[i] = temp;
        //        }
        //    }

        //    /// <summary>
        //    /// Returns n unique random numbers in the range [1, n], inclusive. 
        //    /// This is equivalent to getting the first n numbers of some random permutation of the sequential numbers from 1 to max. 
        //    /// Runs in O(k^2) time.
        //    /// </summary>
        //    /// <param name="rand"></param>
        //    /// <param name="n">Maximum number possible.</param>
        //    /// <param name="k">How many numbers to return.</param>
        //    /// <returns></returns>
        //    public static int[] Permutation(this Random rand, int n, int k)
        //    {
        //        var result = new List<int>();
        //        var sorted = new SortedSet<int>();

        //        for (var i = 0; i < k; i++)
        //        {
        //            var r = rand.Next(1, n + 1 - i);

        //            foreach (var q in sorted)
        //                if (r >= q) r++;

        //            result.Add(r);
        //            sorted.Add(r);
        //        }

        //        return result.ToArray();
        //    }
        //}

}
