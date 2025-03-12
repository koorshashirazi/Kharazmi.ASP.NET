using System.Web.Caching;

namespace Mvc.Utility.Core.Managers.Cache
{
    public interface ICacheDependencyManager
    {
        CacheDependency GetOrAddCache(string key);

        CacheDependency GetOrAddCachePerUser(string key, string userId);

        void Invalidate(string key);

        void Invalidate(string userId, string key);

        void InvalidateAll();

        void InvalidateAllPerUser(string userId);
    }
}