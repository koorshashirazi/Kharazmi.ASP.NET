using System;

namespace Mvc.Utility.Core.Extensions
{
    public static partial class Common
    {
        public static string GetEnglishNumber(this string data)
        {
            if (string.IsNullOrEmpty(data)) return string.Empty;

            for (var i = 1777; i < 1786; i++) data = data.Replace(Convert.ToChar(i), Convert.ToChar(i - 1728));

            return data;
        }
    }
}