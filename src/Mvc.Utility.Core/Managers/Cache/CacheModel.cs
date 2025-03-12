using System;
using System.Web.Caching;

namespace Mvc.Utility.Core.Managers.Cache
{
    public class CacheModel
    {
        public string Key { get; set; }
        public string UserId { get; set; }
        public object Data { get; set; }
        public CacheDependency Dependency { get; set; }
        public DateTime AbsoluteExpiration { get; set; } = System.Web.Caching.Cache.NoAbsoluteExpiration;
        public TimeSpan SlidingExpiration { get; set; } = System.Web.Caching.Cache.NoSlidingExpiration;
    }
}