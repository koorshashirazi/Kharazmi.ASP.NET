using System;
using System.Web;
using Castle.DynamicProxy;
using Mvc.Utility.Core.Helpers;
using Mvc.Utility.Core.Managers.Cache;

namespace Kharazmi.Aop.Autofac
{
    public class CacheInterceptor : IInterceptor
    {
        private static readonly object LOCK = new object();
        private readonly ICacheStorageManager _cacheStorageManager;
        private readonly ICacheDependencyManager _dependencyManager;

        public CacheInterceptor(HttpContextBase httpContextBase)
        {
            if (httpContextBase == null)
                throw ExceptionHelper.ThrowException<NullReferenceException>(string.Empty, nameof(HttpContextBase));

            ICacheStorage cacheStore = new CacheStorage(httpContextBase);
            _dependencyManager = new CacheDependencyManager(cacheStore);
            _cacheStorageManager = new CacheStorageManager(cacheStore, _dependencyManager);
        }

        public void Intercept(IInvocation invocation)
        {
            CacheMethod(invocation);
        }

        private void CacheMethod(IInvocation invocation)
        {
            var cacheMethodAttribute = GetCacheMethodAttribute(invocation);
            if (cacheMethodAttribute == null)
            {
                invocation.Proceed();
                return;
            }

            var cacheDuration = ((CacheMethodAttribute) cacheMethodAttribute).DateTimeToCache;
            var cacheKey = GetCacheKey(invocation);

            var cachedResult = _cacheStorageManager.GetCache<object>(cacheKey);

            if (cachedResult != null)
                invocation.ReturnValue = cachedResult;
            else
                lock (LOCK)
                {
                    invocation.Proceed();
                    if (invocation.ReturnValue == null)
                        return;

                    _dependencyManager.Invalidate(cacheKey);

                    var cacheModel = new CacheModel
                    {
                        Key = cacheKey,
                        Data = invocation.ReturnValue,
                        Dependency = _dependencyManager.GetOrAddCache(cacheKey),
                        AbsoluteExpiration = cacheDuration,
                        SlidingExpiration = TimeSpan.Zero
                    };

                    _cacheStorageManager.AddOrUpdateCache(cacheModel);
                }
        }

        private static Attribute GetCacheMethodAttribute(IInvocation invocation)
        {
            var methodInfo = invocation.MethodInvocationTarget;
            if (methodInfo == null) methodInfo = invocation.Method;
            return Attribute.GetCustomAttribute(methodInfo, typeof(CacheMethodAttribute), inherit: true);
        }

        private static string GetCacheKey(IInvocation invocation)
        {
            var cacheKey = invocation.Method.Name;

            foreach (var argument in invocation.Arguments) cacheKey += ":" + argument;
            return HashHelper.HashMd5(cacheKey);
        }
    }
}