using System;
using Microsoft.Owin;

namespace Mvc.Utility.Core.Managers.Authorization
{
    /// <summary>
    ///     Extracts authorization objects from an <see cref="IOwinContext" /> environment.
    /// </summary>
    public static class OwinContextExtensions
    {
        /// <summary>
        ///     Extracts an <see cref="AuthorizationOptions" /> from the <see cref="IOwinContext" />.
        /// </summary>
        public static AuthorizationOptions GetAuthorizationOptions(this IOwinContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (context.Environment == null)
                throw new ArgumentNullException(nameof(context),
                    ShareResources.ErrorTheOwinEnvironmentDictionaryWasNull);

            if (context.Environment.TryGetValue(ResourceAuthorizationMiddleware.SERVICE_KEY,
                out var environmentService))
            {
                if (environmentService == null)
                    throw new InvalidOperationException(ShareResources.Exception_AuthorizationOptionsMustNotBeNull);

                return (AuthorizationOptions) environmentService;
            }

            throw new InvalidOperationException(ShareResources
                .Exception_PleaseSetupOwinResourceAuthorizationInYourStartupFile);
        }

        /// <summary>
        ///     Extracts an <see cref="IAuthorizationService" /> from the <see cref="IOwinContext" />.
        /// </summary>
        public static IAuthorizationService GetAuthorizationService(this IOwinContext context)
        {
            return GetAuthorizationOptions(context).Dependencies?.Service;
        }
    }
}