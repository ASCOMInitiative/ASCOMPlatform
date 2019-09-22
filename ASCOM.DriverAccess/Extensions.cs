using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASCOM.DriverAccess
{
    internal static class Extensions
    {
        internal static ArrayList ToArrayList(this IEnumerable enumerable)
        {
            var templist = new ArrayList();

            foreach (var item in enumerable)
            {
                templist.Add(item);
            }
            return templist;
        }

        internal static ArrayList ComObjToArrayList(this object obj)
        {
            return obj as ArrayList ?? ((IEnumerable)obj).ToArrayList();
        }
    }
}
