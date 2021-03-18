using Fengj.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class DataDispatch
{
    public static Facade facade;

    internal static void Require(object obj)
    {
        var itfs = obj.GetType().GetInterfaces();
        if (itfs.Contains(typeof(IRequireCell)))
        {
            var require = obj as IRequireCell;
            require.cell = facade.runData.map.GetCell(require.coord.q, require.coord.r);
        }
    }
}
