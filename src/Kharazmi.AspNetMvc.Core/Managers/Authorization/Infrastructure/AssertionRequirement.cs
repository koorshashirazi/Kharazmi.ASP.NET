using System;
using System.Threading.Tasks;

namespace Kharazmi.AspNetMvc.Core.Managers.Authorization.Infrastructure
{
    /// <summary>
    ///     Implements an <see cref="IAuthorizationHandler" /> and <see cref="IAuthorizationRequirement" />
    ///     that takes a user specified assertion.
    /// </summary>
    public class AssertionRequirement : IAuthorizationHandler, IAuthorizationRequirement
    {
        /// <summary>
        ///     Creates a new instance of <see cref="AssertionRequirement" />.
        /// </summary>
        /// <param name="handler">Function that is called to handle this requirement.</param>
        public AssertionRequirement(Func<AuthorizationHandlerContext, bool> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            Handler = context => Task.FromResult(handler(context));
        }

        /// <summary>
        ///     Creates a new instance of <see cref="AssertionRequirement" />.
        /// </summary>
        /// <param name="handler">Function that is called to handle this requirement.</param>
        public AssertionRequirement(Func<AuthorizationHandlerContext, Task<bool>> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            Handler = handler;
        }

        /// <summary>
        ///     Function that is called to handle this requirement.
        /// </summary>
        public Func<AuthorizationHandlerContext, Task<bool>> Handler { get; }

        /// <summary>
        ///     Calls <see cref="AssertionRequirement.Handler" /> to see if authorization is allowed.
        /// </summary>
        /// <param name="context">The authorization information.</param>
        public async Task HandleAsync(AuthorizationHandlerContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (await Handler(context)) context.Succeed(this);
        }
    }
}