using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fengj.Clan
{
    interface IClanManager: IEnumerable<IClan>
    {

    }

    class ClanManager : IClanManager
    {
        List<IClan> clans;

        public ClanManager()
        {
            clans = new List<IClan>()
            {
                new ClanG(){ name = "A", origin = "ab", popNum = 1000},
                new ClanQ(){ name = "B", origin = "bc", popNum = 1200},
                new ClanQ(){ name = "c", origin = "cd", popNum = 900},
            };
        }

        public IEnumerator<IClan> GetEnumerator()
        {
            return ((IEnumerable<IClan>)clans).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)clans).GetEnumerator();
        }
    }
}
