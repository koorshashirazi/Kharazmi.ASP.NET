using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Mvc.Utility.Core.Managers.Authorization
{
    /// <summary>
    ///     A type which can provide a <see cref="AuthorizationPolicy" /> for a particular name.
    /// </summary>
    public interface IAuthorizationPolicyProvider
    {
        /// <summary>
        ///     Gets a <see cref="AuthorizationPolicy" /> from the given <paramref name="policyName" />
        /// </summary>
        /// <param name="policyName">The policy name to retrieve.</param>
        /// <returns>The named <see cref="AuthorizationPolicy" />.</returns>
        Task<AuthorizationPolicy> GetPolicyAsync(string policyName);

        /// <summary>
        ///     Gets the default <see cref="AuthorizationPolicy" />
        /// </summary>
        /// <returns>The default <see cref="AuthorizationPolicy" />.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification =
            "It may be time consuming")]
        Task<AuthorizationPolicy> GetDefaultPolicyAsync();
    }
}