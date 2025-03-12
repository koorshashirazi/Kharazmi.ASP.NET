using System;
using System.Globalization;
using System.Resources;
using System.Threading;
using System.Web;

namespace Mvc.Utility.Core.Helpers
{
    public static class ResourceHelper
    {
        public static string ReadResourceValue(HttpContextBase httpContext, string resxFilename, string key,
            params object[] args)
        {
            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));

            string result;
            try
            {
                result = httpContext.GetGlobalResourceObject(resxFilename, key,
                        CultureInfo.GetCultureInfo(Thread.CurrentThread.CurrentCulture.Name))
                    ?.ToString();

                if (args != null)
                    if (!string.IsNullOrWhiteSpace(result))
                        result = string.Format(result, args);
            }
            catch (Exception)
            {
                result = string.Empty;
            }

            return result;
        }

        public static string ReadResourceValue(string fileName, string key, params object[] args)
        {
            string result;
            try
            {
                var filePath = AppDomain.CurrentDomain.BaseDirectory;

                var resourceManager =
                    ResourceManager.CreateFileBasedResourceManager(fileName, filePath, null);

                result = resourceManager.GetString(key,
                             CultureInfo.GetCultureInfo(Thread.CurrentThread.CurrentCulture.Name)) ?? string.Empty;

                if (args != null) result = string.Format(result, args);
            }
            catch (Exception)
            {
                result = string.Empty;
            }

            return result;
        }

        public static string ReadResourceValue(Type typeOfResource, string key, params object[] args)
        {
            string result;
            try
            {
                var resourceManager = new ResourceManager(typeOfResource);

                result = resourceManager.GetString(key,
                             CultureInfo.GetCultureInfo(Thread.CurrentThread.CurrentCulture.Name)) ?? string.Empty;

                if (args != null) result = string.Format(result, args);
            }
            catch (Exception)
            {
                result = string.Empty;
            }

            return result;
        }
    }
}