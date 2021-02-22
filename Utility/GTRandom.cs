using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fengj.Utility
{
    public class GTRandom
    {
        public int Next(int b, int e)
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            Random random = new Random(BitConverter.ToInt32(buffer, 0));

            return random.Next(b, e);
        }
    }
}
