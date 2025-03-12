using System;
using System.Web.Caching;

namespace Kharazmi.AspNetMvc.Core.Managers.Cache
{
    /// <inheritdoc src="CacheDependency" />
    /// <summary>
    ///     No file system dependencies
    /// </summary>
    public class CacheDependencyModel : CacheDependency
    {
        private readonly string _uniqueKey;

        public CacheDependencyModel(string uniqueKey) : base(new string[0])
        {
            _uniqueKey = uniqueKey;
        }

        public override string GetUniqueID()
        {
            return _uniqueKey;
        }

        public void Invalidate()
        {
            NotifyDependencyChanged(this, EventArgs.Empty);
        }
    }
}