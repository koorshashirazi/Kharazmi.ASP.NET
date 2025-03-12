using System;
using System.Web.Caching;

namespace Kharazmi.AspNetMvc.Core.Managers.Cache
{
    public interface ICacheStorage
    {
        void Remove(string key);

        void Add(string key, object data);

        void Add(string key, object data, CacheDependency dependency);

        void Add(string key, object data, CacheDependency dependency, DateTime absoluteExpiration,
            TimeSpan slidingExpiration);

        void Add(string key, object data, CacheDependency dependency, DateTime absoluteExpiration,
            TimeSpan slidingExpiration, CacheItemUpdateCallback updateCallback);

        T Get<T>(string key);
    }
}