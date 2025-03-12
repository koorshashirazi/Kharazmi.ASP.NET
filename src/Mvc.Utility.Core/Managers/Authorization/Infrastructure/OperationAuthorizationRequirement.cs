using System.Diagnostics.CodeAnalysis;

namespace Mvc.Utility.Core.Managers.Authorization.Infrastructure
{
    /// <summary>
    ///     A helper class to provide a useful <see cref="IAuthorizationRequirement" /> which
    ///     contains a name.
    /// </summary>
    public class OperationAuthorizationRequirement : IAuthorizationRequirement
    {
        /// <summary>
        ///     The name of this instance of <see cref="IAuthorizationRequirement" />.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public string Name { get; set; }
    }
}