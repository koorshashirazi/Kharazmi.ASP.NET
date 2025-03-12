using System;
using System.Collections.Generic;
using System.Linq;

namespace Kharazmi.AspNetMvc.Core.Extensions
{
    public static partial class Common
    {
        public static string ListToString(this List<string> list)
        {
            return string.Join(",", list);
        }
        
        public static bool ContainsAll<T, TKey>(this IEnumerable<T> list1, IEnumerable<T> list2, Func<T, TKey> key)
        {
            var containingList = new HashSet<TKey>(list1.Select(key));
            return list2.All(x => containingList.Contains(key(x)));
        }

        public static bool ContainsAll<T>(this IEnumerable<T> list1, IEnumerable<T> list2) =>
            list1.ContainsAll(list2, item => item);
    }
}