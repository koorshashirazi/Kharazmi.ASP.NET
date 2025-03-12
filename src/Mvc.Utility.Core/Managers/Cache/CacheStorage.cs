using System;
using System.Web;
using System.Web.Caching;

namespace Mvc.Utility.Core.Managers.Cache
{
    public class CacheStorage : ICacheStorage
    {
        private readonly HttpContextBase _httpContext;

        public CacheStorage(HttpContextBase httpContext)
        {
            _httpContext = httpContext;
        }

        public void Add(string key, object data)
        {
            _httpContext.Cache.Insert(key, data);
        }

        public void Add(string key, object data, CacheDependency dependency)
        {
            _httpContext.Cache.Insert(key, data, dependency);
        }

        public void Add(string key, object data, CacheDependency dependency, DateTime absoluteExpiration,
            TimeSpan slidingExpiration)
        {
            _httpContext.Cache.Insert(key, data, dependency, absoluteExpiration, slidingExpiration);
        }

        public void Add(string key, object data, CacheDependency dependency, DateTime absoluteExpiration,
            TimeSpan slidingExpiration, CacheItemUpdateCallback updateCallback)
        {
            _httpContext.Cache.Insert(key, data, dependency, absoluteExpiration, slidingExpiration, updateCallback);
        }

        public T Get<T>(string key)
        {
            return _httpContext.Cache?.Get(key) != null ? (T) _httpContext.Cache.Get(key) : default;
        }

        public void Remove(string key)
        {
            _httpContext.Cache.Remove(key);
        }

        public void Add(string key, object data, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            _httpContext.Cache.Insert(key, data, null, absoluteExpiration, slidingExpiration);
        }

        //private void HandleNotification(string key, CacheItemUpdateReason reason,
        //    out object data,
        //    out CacheDependency dependency,
        //    out DateTime absoluteExpiry,
        //    out TimeSpan slidingExpiry)
        //{
        //    data = _cacheModel.Data;
        //    dependency = _cacheModel.Dependency;
        //    slidingExpiry = System.Web.Caching.Cache.NoSlidingExpiration;
        //    absoluteExpiry = System.Web.Caching.Cache.NoAbsoluteExpiration;
        //}
    }
}