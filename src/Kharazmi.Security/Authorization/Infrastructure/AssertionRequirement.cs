using System;
using System.Threading.Tasks;

namespace Kharazmi.Security.Authorization.Infrastructure
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
            if (handler == null)
            {
                throw new ArgumentNullException(paramName: nameof(handler));
            }

            Handler = context => Task.FromResult(result: handler(arg: context));
        }

        /// <summary>
        ///     Creates a new instance of <see cref="AssertionRequirement" />.
        /// </summary>
        /// <param name="handler">Function that is called to handle this requirement.</param>
        public AssertionRequirement(Func<AuthorizationHandlerContext, Task<bool>> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(paramName: nameof(handler));
            }

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
            if (context == null)
            {
                throw new ArgumentNullException(paramName: nameof(context));
            }

            if (await Handler(arg: context))
            {
                context.Succeed(requirement: this);
            }
        }
    }
}