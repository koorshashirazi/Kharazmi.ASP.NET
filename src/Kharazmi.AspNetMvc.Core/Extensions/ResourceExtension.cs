using System;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace Kharazmi.AspNetMvc.Core.Extensions
{
    public static partial class Common
    {
        public static string GetResource(this Type resourceType, string key)
        {
            return new ResourceManager(resourceType).GetString(key,
                new CultureInfo(Thread.CurrentThread.CurrentCulture.Name));
        }
    }
}