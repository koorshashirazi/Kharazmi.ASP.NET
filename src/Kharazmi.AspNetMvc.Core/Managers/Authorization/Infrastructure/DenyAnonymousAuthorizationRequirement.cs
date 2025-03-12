using System;
using System.Linq;
using System.Threading.Tasks;

namespace Kharazmi.AspNetMvc.Core.Managers.Authorization.Infrastructure
{
    /// <summary>
    ///     Implements an <see cref="IAuthorizationHandler" /> and <see cref="IAuthorizationRequirement" />
    ///     which requires the current user must be authenticated.
    /// </summary>
    public class DenyAnonymousAuthorizationRequirement : AuthorizationHandler<DenyAnonymousAuthorizationRequirement>,
        IAuthorizationRequirement
    {
        /// <summary>
        ///     Makes a decision if authorization is allowed based on a specific requirement.
        /// </summary>
        /// <param name="context">The authorization context.</param>
        /// <param name="requirement">The requirement to evaluate.</param>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            DenyAnonymousAuthorizationRequirement requirement)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var user = context.User;
            var userIsAnonymous = user?.Identity == null || !user.Identities.Any(i => i.IsAuthenticated);
            if (!userIsAnonymous) context.Succeed(requirement);

            return Task.FromResult(0);
        }
    }
}