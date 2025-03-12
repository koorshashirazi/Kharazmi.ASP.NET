using System;

namespace Mvc.Utility.Core.Managers.InstanceManager
{
    public abstract class Singleton<T> where T : class
    {
        private static readonly Lazy<T> LazyInstance = new Lazy<T>(CreateInstance);

        public static T Instance => LazyInstance.Value;

        private static T CreateInstance()
        {
            return Activator.CreateInstance(typeof(T), true) as T;
        }
    }
}