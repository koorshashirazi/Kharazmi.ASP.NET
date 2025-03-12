using System;
using System.Linq;
using System.Threading.Tasks;

namespace Kharazmi.AspNetMvc.Core.Managers.Authorization.Infrastructure
{
    /// <summary>
    ///     Infrastructre class which allows an <see cref="IAuthorizationRequirement" /> to
    ///     be its own <see cref="IAuthorizationHandler" />.
    /// </summary>
    public class PassThroughAuthorizationHandler : IAuthorizationHandler
    {
        /// <summary>
        ///     Makes a decision if authorization is allowed.
        /// </summary>
        /// <param name="context">The authorization context.</param>
        public async Task HandleAsync(AuthorizationHandlerContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            foreach (var handler in context.Requirements.OfType<IAuthorizationHandler>())
                await handler.HandleAsync(context);
        }
    }
}