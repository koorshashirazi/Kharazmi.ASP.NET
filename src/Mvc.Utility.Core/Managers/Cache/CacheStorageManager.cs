namespace Mvc.Utility.Core.Managers.Cache
{
    public class CacheStorageManager : ManagerBase, ICacheStorageManager
    {
        private readonly ICacheStorage _cache;
        private readonly ICacheDependencyManager _cacheDependencyManager;

        public CacheStorageManager(ICacheStorage cache, ICacheDependencyManager cacheDependencyManager)
        {
            _cache = cache;
            _cacheDependencyManager = cacheDependencyManager;
        }

        public void AddOrUpdateCache(CacheModel cacheModel)
        {
            TryExecute(
                () =>
                {
                    _cacheDependencyManager.Invalidate(cacheModel.Key);

                    if (cacheModel.Dependency.HasChanged)
                        cacheModel = new CacheModel
                        {
                            Key = cacheModel.Key,
                            Data = cacheModel.Data,
                            Dependency = _cacheDependencyManager.GetOrAddCache(cacheModel.Key)
                        };

                    _cache.Add(cacheModel.Key, cacheModel.Data, cacheModel.Dependency, cacheModel.AbsoluteExpiration,
                        cacheModel.SlidingExpiration);
                });
        }

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public void AddOrUpdateCache(string key, object data)
        {
            TryExecute(
                () =>
                {
                    _cacheDependencyManager.Invalidate(key);

                    var cacheModel = new CacheModel
                    {
                        Key = key,
                        Data = data,
                        Dependency = _cacheDependencyManager.GetOrAddCache(key)
                    };

                    _cache.Add(cacheModel.Key, cacheModel.Data, cacheModel.Dependency, cacheModel.AbsoluteExpiration,
                        cacheModel.SlidingExpiration);
                });
        }

        public void RemoveCache(string key)
        {
            TryExecute(() => { _cacheDependencyManager.Invalidate(key); });
        }

        public T GetCache<T>(string key)
        {
            return TryExecute(() => _cache.Get<T>(key));
        }

        public void AddOrUpdateCachePerUser(CacheModel cacheModel)
        {
            TryExecute(
                () =>
                {
                    var uniqueKey = $"{cacheModel.UserId}/{cacheModel.Key}";

                    _cacheDependencyManager.Invalidate(uniqueKey);

                    _cache.Add(uniqueKey, cacheModel.Data, cacheModel.Dependency, cacheModel.AbsoluteExpiration,
                        cacheModel.SlidingExpiration);
                });
        }

        public void AddOrUpdateCachePerUser(string userId, string key, object data)
        {
            TryExecute(
                () =>
                {
                    var uniqueKey = $"{userId}/{key}";

                    _cacheDependencyManager.Invalidate(uniqueKey);

                    var cacheModel = new CacheModel
                    {
                        Key = uniqueKey,
                        Data = data,
                        Dependency = _cacheDependencyManager.GetOrAddCache(uniqueKey)
                    };

                    _cache.Add(uniqueKey, cacheModel.Data, cacheModel.Dependency, cacheModel.AbsoluteExpiration,
                        cacheModel.SlidingExpiration);
                });
        }

        public T GetCachePerUser<T>(string userId, string key)
        {
            return TryExecute(
                () =>
                {
                    var uniqueKey = $"{userId}/{key}";
                    return _cache.Get<T>(uniqueKey);
                });
        }

        public void RemoveCachePerUser(string userId, string key)
        {
            TryExecute(
                () =>
                {
                    var uniqueKey = $"{userId}/{key}";
                    _cacheDependencyManager.Invalidate(uniqueKey);
                });
        }
    }
}