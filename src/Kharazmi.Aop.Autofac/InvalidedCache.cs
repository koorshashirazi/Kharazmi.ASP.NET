using System;
using System.Web;
using Mvc.Utility.Core.Helpers;
using Mvc.Utility.Core.Managers.Cache;

namespace Kharazmi.Aop.Autofac
{
    public class InvalidedCache
    {
        private readonly ICacheDependencyManager _dependencyManager;

        public InvalidedCache(HttpContextBase httpContextBase)
        {
            if (httpContextBase == null)
                throw ExceptionHelper.ThrowException<NullReferenceException>(string.Empty, nameof(httpContextBase));

            ICacheStorage cacheStore = new CacheStorage(httpContextBase);
            _dependencyManager = new CacheDependencyManager(cacheStore);
        }

        public void InvalidCache(string key)
        {
            _dependencyManager.Invalidate(key);
        }

        public void InvalidCacheAll()
        {
            _dependencyManager.InvalidateAll();
        }
    }
}