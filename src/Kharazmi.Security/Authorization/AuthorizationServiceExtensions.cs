using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kharazmi.Security.Authorization
{
    /// <summary>
    ///     Extension methods for <see cref="IAuthorizationService" />.
    /// </summary>
    public static class AuthorizationServiceExtensions
    {
        /// <summary>
        ///     Checks if a user meets a specific requirement for the specified resource
        /// </summary>
        /// <param name="service">The <see cref="IAuthorizationService" /> providing authorization.</param>
        /// <param name="user">The user to evaluate the policy against.</param>
        /// <param name="resource">The resource to evaluate the policy against.</param>
        /// <param name="requirement">The requirement to evaluate the policy against.</param>
        /// <returns>
        ///     A flag indicating whether requirement evaluation has succeeded or failed.
        ///     This value is
        ///     <value>true</value>
        ///     when the user fulfills the policy, otherwise
        ///     <value>false</value>
        ///     .
        /// </returns>
        public static Task<bool> AuthorizeAsync(this IAuthorizationService service, ClaimsPrincipal user, object resource, IAuthorizationRequirement requirement)
        {
            if (service == null)
            {
                throw new ArgumentNullException(paramName: nameof(service));
            }

            if (requirement == null)
            {
                throw new ArgumentNullException(paramName: nameof(requirement));
            }

            return service.AuthorizeAsync(user: user, resource: resource, requirements: new[] {requirement});
        }

        /// <summary>
        ///     Checks if a user meets a specific authorization policy against the specified resource.
        /// </summary>
        /// <param name="service">The <see cref="IAuthorizationService" /> providing authorization.</param>
        /// <param name="user">The user to evaluate the policy against.</param>
        /// <param name="resource">The resource to evaluate the policy against.</param>
        /// <param name="policy">The policy to evaluate.</param>
        /// <returns>
        ///     A flag indicating whether policy evaluation has succeeded or failed.
        ///     This value is
        ///     <value>true</value>
        ///     when the user fulfills the policy, otherwise
        ///     <value>false</value>
        ///     .
        /// </returns>
        public static Task<bool> AuthorizeAsync(this IAuthorizationService service, ClaimsPrincipal user, object resource, AuthorizationPolicy policy)
        {
            if (service == null)
            {
                throw new ArgumentNullException(paramName: nameof(service));
            }

            if (policy == null)
            {
                throw new ArgumentNullException(paramName: nameof(policy));
            }

            return service.AuthorizeAsync(user: user, resource: resource, requirements: policy.Requirements.ToArray());
        }

        /// <summary>
        ///     Checks if a user meets a specific authorization policy against the specified resource.
        /// </summary>
        /// <param name="service">The <see cref="IAuthorizationService" /> providing authorization.</param>
        /// <param name="user">The user to evaluate the policy against.</param>
        /// <param name="policy">The policy to evaluate.</param>
        /// <returns>
        ///     A flag indicating whether policy evaluation has succeeded or failed.
        ///     This value is
        ///     <value>true</value>
        ///     when the user fulfills the policy, otherwise
        ///     <value>false</value>
        ///     .
        /// </returns>
        public static Task<bool> AuthorizeAsync(this IAuthorizationService service, ClaimsPrincipal user, AuthorizationPolicy policy) { return AuthorizeAsync(service: service, user: user, resource: null, policy: policy); }

        /// <summary>
        ///     Checks if a user meets a specific authorization policy against the specified resource.
        /// </summary>
        /// <param name="service">The <see cref="IAuthorizationService" /> providing authorization.</param>
        /// <param name="user">The user to evaluate the policy against.</param>
        /// <param name="policyName">The name of the policy to evaluate.</param>
        /// <returns>
        ///     A flag indicating whether policy evaluation has succeeded or failed.
        ///     This value is
        ///     <value>true</value>
        ///     when the user fulfills the policy, otherwise
        ///     <value>false</value>
        ///     .
        /// </returns>
        public static Task<bool> AuthorizeAsync(this IAuthorizationService service, ClaimsPrincipal user, string policyName)
        {
            if (service == null)
            {
                throw new ArgumentNullException(paramName: nameof(service));
            }

            if (policyName == null)
            {
                throw new ArgumentNullException(paramName: nameof(policyName));
            }

            return service.AuthorizeAsync(user: user, resource: null, policyName: policyName);
        }
    }
}