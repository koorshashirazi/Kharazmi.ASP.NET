using System.Collections.Generic;
using System.Linq;
using System.Web.Caching;
using Kharazmi.AspNetMvc.Core.Constraints;
using Kharazmi.AspNetMvc.Core.Helpers;

namespace Kharazmi.AspNetMvc.Core.Managers.Cache
{
    public class CacheDependencyManager : ManagerBase, ICacheDependencyManager
    {
        private readonly string _cacheDependencyKey = HashHelper.HashMd5(Constraint.APPLICATION_CACHE_DEPENDENCY);
        private readonly ICacheStorage _cacheStorage;
        private readonly Dictionary<string, CacheDependencyModel> _dependencies;

        public CacheDependencyManager(ICacheStorage cacheStorage)
        {
            _cacheStorage = cacheStorage;
            _dependencies =
                _cacheStorage.Get<Dictionary<string, CacheDependencyModel>>(_cacheDependencyKey) ??
                new Dictionary<string, CacheDependencyModel>();
        }

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public CacheDependency GetOrAddCache(string key)
        {
            if (_dependencies.ContainsKey(key)) return _dependencies[key];

            return TryExecute(
                () =>
                {
                    _dependencies.Add(key, new CacheDependencyModel(key));
                    _cacheStorage.Add(_cacheDependencyKey, _dependencies);
                    return _dependencies[key];
                });
        }

        public CacheDependency GetOrAddCachePerUser(string key, string userId)
        {
            var uniqueKey = $"{userId}/{key}";

            if (_dependencies.ContainsKey(uniqueKey)) return _dependencies[uniqueKey];

            return TryExecute(
                () =>
                {
                    _dependencies.Add(uniqueKey, new CacheDependencyModel(uniqueKey));
                    _cacheStorage.Add(_cacheDependencyKey, _dependencies);
                    return _dependencies[uniqueKey];
                });
        }

        /// <summary>
        ///     To invalidate the program cache.
        /// </summary>
        /// <param name="key"></param>
        public void Invalidate(string key)
        {
            if (!_dependencies.ContainsKey(key)) return;

            TryExecute(
                () =>
                {
                    var dependency = _dependencies[key];
                    dependency.Invalidate();
                    dependency.Dispose();
                    _dependencies.Remove(key);
                });
        }

        public void Invalidate(string userId, string key)
        {
            var uniqueKey = $"{userId}/{key}";

            if (!_dependencies.ContainsKey(uniqueKey)) return;

            TryExecute(
                () =>
                {
                    var dependency = _dependencies[uniqueKey];
                    dependency.Invalidate();
                    dependency.Dispose();
                    _dependencies.Remove(uniqueKey);
                });
        }

        public void InvalidateAll()
        {
            TryExecute(
                () =>
                {
                    var dependency = _dependencies[_cacheDependencyKey];
                    dependency.Invalidate();
                    dependency.Dispose();
                    _dependencies.Remove(_cacheDependencyKey);
                });
        }

        /// <summary>
        ///     This method should be used to exit the program
        /// </summary>
        public void InvalidateAllPerUser(string userId)
        {
            TryExecute(
                () =>
                {
                    foreach (var cache in _dependencies
                        .Where(x => x.Key.Split('/')[0].Equals(userId))
                        .ToList())
                    {
                        var dependency = _dependencies[cache.Key];
                        dependency.Invalidate();
                        dependency.Dispose();
                        _dependencies.Remove(cache.Key);
                    }
                });
        }
    }
}