using Microsoft.Owin;

namespace Mvc.Utility.Core.Managers.Authorization
{
    /// <summary>
    ///     Provides access to an <see cref="IOwinContext" />
    /// </summary>
    public interface IOwinContextAccessor
    {
        /// <summary>
        ///     Gets an <see cref="IOwinContext" />.
        /// </summary>
        IOwinContext Context { get; }
    }
}