using System;
using System.Threading.Tasks;

namespace Mvc.Utility.Core.Managers.Authorization
{
    /// <summary>
    ///     The default implementation of an <see cref="IAuthorizationPolicyProvider" />,
    ///     which provides an <see cref="AuthorizationPolicy" /> for a particular name.
    /// </summary>
    public class DefaultAuthorizationPolicyProvider : IAuthorizationPolicyProvider
    {
        private readonly AuthorizationOptions _options;

        /// <summary>
        ///     Creates a new instance of <see cref="DefaultAuthorizationPolicyProvider" />.
        /// </summary>
        /// <param name="options">The options used to configure this instance.</param>
        public DefaultAuthorizationPolicyProvider(AuthorizationOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        ///     Gets the default <see cref="AuthorizationPolicy" />
        /// </summary>
        /// <returns>The default <see cref="AuthorizationPolicy" />.</returns>
        public virtual Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return Task.FromResult(_options.DefaultPolicy);
        }

        /// <summary>
        ///     Gets a <see cref="AuthorizationPolicy" /> from the given <paramref name="policyName" />
        /// </summary>
        /// <param name="policyName">The policy name to retrieve.</param>
        /// <returns>The named <see cref="AuthorizationPolicy" />.</returns>
        public virtual Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            return Task.FromResult(_options.GetPolicy(policyName));
        }
    }
}