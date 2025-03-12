using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Mvc.Utility.Core;

namespace Kharazmi.AspNetMvc.Core.Managers.Authorization.Infrastructure
{
    /// <summary>
    ///     Implements an <see cref="IAuthorizationHandler" /> and <see cref="IAuthorizationRequirement" />
    ///     which requires at least one role claim whose value must be any of the allowed roles.
    /// </summary>
    public class RolesAuthorizationRequirement : AuthorizationHandler<RolesAuthorizationRequirement>,
        IAuthorizationRequirement
    {
        /// <summary>
        ///     Creates a new instance of <see cref="RolesAuthorizationRequirement" />.
        /// </summary>
        /// <param name="allowedRoles">A collection of allowed roles.</param>
        public RolesAuthorizationRequirement(IEnumerable<string> allowedRoles)
        {
            if (allowedRoles == null) throw new ArgumentNullException(nameof(allowedRoles));

            // ReSharper disable once PossibleMultipleEnumeration because it will not enumerate the entire list
            if (!allowedRoles.Any()) throw new InvalidOperationException(ShareResources.Exception_RoleRequirementEmpty);

            // ReSharper disable once PossibleMultipleEnumeration
            AllowedRoles = allowedRoles;
        }

        /// <summary>
        ///     Gets the collection of allowed roles.
        /// </summary>
        public IEnumerable<string> AllowedRoles { get; }

        /// <summary>
        ///     Makes a decision if authorization is allowed based on a specific requirement.
        /// </summary>
        /// <param name="context">The authorization context.</param>
        /// <param name="requirement">The requirement to evaluate.</param>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            RolesAuthorizationRequirement requirement)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (requirement == null) throw new ArgumentNullException(nameof(requirement));

            if (context.User != null)
            {
                Debug.Assert(requirement.AllowedRoles != null, "requirement.AllowedRoles != null");
                Debug.Assert(requirement.AllowedRoles.Any(), "requirement.AllowedRoles.Any()");

                var found = requirement.AllowedRoles.Any(role => context.User.IsInRole(role));
                if (found) context.Succeed(requirement);
            }

            return Task.FromResult(0);
        }
    }
}