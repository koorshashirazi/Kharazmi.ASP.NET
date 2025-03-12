namespace Kharazmi.AspNetMvc.Core.Managers.Cache
{
    public interface ICacheStorageManager
    {
        void AddOrUpdateCache(CacheModel cacheModel);

        void AddOrUpdateCachePerUser(CacheModel cacheModel);

        void AddOrUpdateCache(string key, object data);

        void AddOrUpdateCachePerUser(string userId, string key, object data);

        T GetCache<T>(string key);

        T GetCachePerUser<T>(string userId, string key);

        void RemoveCache(string key);

        void RemoveCachePerUser(string userId, string key);
    }
}