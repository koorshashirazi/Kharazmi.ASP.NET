using System;
using Microsoft.Owin;

namespace Kharazmi.Security.Authorization
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
            if (context == null)
            {
                throw new ArgumentNullException(paramName: nameof(context));
            }

            if (context.Environment == null)
            {
                throw new ArgumentNullException(paramName: nameof(context), message: ShareResources.ErrorTheOwinEnvironmentDictionaryWasNull);
            }

            if (context.Environment.TryGetValue(key: ResourceAuthorizationMiddleware.SERVICE_KEY, value: out var environmentService))
            {
                if (environmentService == null)
                {
                    throw new InvalidOperationException(message: ShareResources.Exception_AuthorizationOptionsMustNotBeNull);
                }

                return (AuthorizationOptions) environmentService;
            }

            throw new InvalidOperationException(message: ShareResources.Exception_PleaseSetupOwinResourceAuthorizationInYourStartupFile);
        }

        /// <summary>
        ///     Extracts an <see cref="IAuthorizationService" /> from the <see cref="IOwinContext" />.
        /// </summary>
        public static IAuthorizationService GetAuthorizationService(this IOwinContext context) { return GetAuthorizationOptions(context: context).Dependencies?.Service; }
    }
}