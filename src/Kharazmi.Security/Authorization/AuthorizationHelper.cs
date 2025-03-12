using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Kharazmi.Security.Authorization.Infrastructure;
using Microsoft.Owin;
using Microsoft.Owin.Logging;

namespace Kharazmi.Security.Authorization
{
    /// <summary>
    ///     Infrastructure class which can authorize with or without owin.
    /// </summary>
    public class AuthorizationHelper : IResourceAuthorizationHelper
    {
        private readonly IOwinContextAccessor _owinContextAccessor;

        /// <summary>
        ///     Creates a new instance of <see cref="AuthorizationHelper" />.
        /// </summary>
        /// <param name="owinContextAccessor"><see cref="IOwinContextAccessor" /> used to retrieve the <see cref="IOwinContext" />.</param>
        public AuthorizationHelper(IOwinContextAccessor owinContextAccessor) { _owinContextAccessor = owinContextAccessor ?? throw new ArgumentNullException(paramName: nameof(owinContextAccessor)); }

        /// <summary>
        ///     Determines if a user is authorized.
        /// </summary>
        /// <param name="controller">The controller from which <see cref="AuthorizationOptions" /> may be obtained.</param>
        /// <param name="user">The user to evaluate the authorize data against.</param>
        /// <param name="authorizeAttribute">The <see cref="IAuthorizeData" /> to evaluate.</param>
        /// <returns>
        ///     A flag indicating whether authorization has succeeded.
        ///     This value is
        ///     <value>true</value>
        ///     when the <paramref name="user" /> fulfills the <paramref name="authorizeAttribute" />; otherwise
        ///     <value>false</value>
        ///     .
        /// </returns>
        /// <remarks>
        ///     If <paramref name="controller" /> is not null, it will be used to find <see cref="AuthorizationOptions" /> instead
        ///     of the current <see cref="IOwinContext" />.
        /// </remarks>
        public async Task<bool> IsAuthorizedAsync(IAuthorizationController controller, ClaimsPrincipal user, IAuthorizeData authorizeAttribute)
        {
            if (user == null)
            {
                throw new ArgumentNullException(paramName: nameof(user));
            }

            if (authorizeAttribute == null)
            {
                throw new ArgumentNullException(paramName: nameof(authorizeAttribute));
            }

            var options = ResolveAuthorizationOptions(controller: controller);

            var dependencies = options.Dependencies ?? new AuthorizationDependencies();
            var policyProvider = dependencies.PolicyProvider ?? new DefaultAuthorizationPolicyProvider(options: options);
            var authorizationService = dependencies.Service;
            if (authorizationService == null)
            {
                var handlerProvider = new DefaultAuthorizationHandlerProvider(new PassThroughAuthorizationHandler());
                var handlers = await handlerProvider.GetHandlersAsync();
                var loggerFactory = dependencies.LoggerFactory ?? new DiagnosticsLoggerFactory();

                authorizationService = new DefaultAuthorizationService(
                        policyProvider: policyProvider,
                        handlers: handlers,
                        logger: loggerFactory.CreateDefaultLogger(),
                        contextFactory: new DefaultAuthorizationHandlerContextFactory(),
                        evaluator: new DefaultAuthorizationEvaluator());
            }

            var policy = await AuthorizationPolicy.CombineAsync(policyProvider: policyProvider, authorizeData: new[] {authorizeAttribute});
            return await authorizationService.AuthorizeAsync(user: user, policy: policy);
        }

        private AuthorizationOptions ResolveAuthorizationOptions(IAuthorizationController controller)
        {
            if (controller != null)
            {
                return controller.AuthorizationOptions;
            }

            return _owinContextAccessor.Context.GetAuthorizationOptions();
        }
    }
}