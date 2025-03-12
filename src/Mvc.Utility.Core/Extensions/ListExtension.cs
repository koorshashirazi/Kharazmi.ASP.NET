using System.Collections.Generic;

namespace Mvc.Utility.Core.Extensions
{
    public static partial class Common
    {
        public static string ListToString(this List<string> list)
        {
            return string.Join(",", list);
        }
    }
}