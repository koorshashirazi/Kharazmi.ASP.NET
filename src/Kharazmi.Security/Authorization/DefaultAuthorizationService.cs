using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Kharazmi.Security.Authorization.Infrastructure;
using Microsoft.Owin.Logging;

namespace Kharazmi.Security.Authorization
{
    /// <summary>
    ///     The default implementation of an <see cref="IAuthorizationService" />.
    /// </summary>
    public class DefaultAuthorizationService : IAuthorizationService
    {
        private readonly IAuthorizationHandlerContextFactory _contextFactory;
        private readonly IAuthorizationEvaluator _evaluator;
        private readonly IList<IAuthorizationHandler> _handlers;
        private readonly ILogger _logger;
        private readonly IAuthorizationPolicyProvider _policyProvider;

        /// <summary>
        ///     Creates a new instance of <see cref="DefaultAuthorizationService" />.
        /// </summary>
        /// <param name="policyProvider">The <see cref="IAuthorizationPolicyProvider" /> used to provide policies.</param>
        /// <param name="handlers">The handlers used to fulfill <see cref="IAuthorizationRequirement" />s.</param>
        /// <remarks>Uses the <see cref="DiagnosticsLoggerFactory" /> to create a logger.</remarks>
        public DefaultAuthorizationService(IAuthorizationPolicyProvider policyProvider, IEnumerable<IAuthorizationHandler> handlers)
                : this(policyProvider: policyProvider, handlers: handlers, logger: null, contextFactory: new DefaultAuthorizationHandlerContextFactory(), evaluator: new DefaultAuthorizationEvaluator())
        {
        }

        /// <summary>
        ///     Creates a new instance of <see cref="DefaultAuthorizationService" />.
        /// </summary>
        /// <param name="policyProvider">The <see cref="IAuthorizationPolicyProvider" /> used to provide policies.</param>
        /// <param name="handlers">The handlers used to fulfill <see cref="IAuthorizationRequirement" />s.</param>
        /// <param name="logger">The logger used to log messages, warnings and errors.</param>
        /// <param name="contextFactory">
        ///     The <see cref="IAuthorizationHandlerContextFactory" /> used to create the context to
        ///     handle the authorization.
        /// </param>
        /// <param name="evaluator">The <see cref="IAuthorizationEvaluator" /> used to determine if authorzation was successful.</param>
        public DefaultAuthorizationService(IAuthorizationPolicyProvider policyProvider, IEnumerable<IAuthorizationHandler> handlers, ILogger logger, IAuthorizationHandlerContextFactory contextFactory, IAuthorizationEvaluator evaluator)
        {
            if (policyProvider == null)
            {
                throw new ArgumentNullException(paramName: nameof(policyProvider));
            }

            if (handlers == null)
            {
                throw new ArgumentNullException(paramName: nameof(handlers));
            }

            if (contextFactory == null)
            {
                throw new ArgumentNullException(paramName: nameof(contextFactory));
            }

            if (evaluator == null)
            {
                throw new ArgumentNullException(paramName: nameof(evaluator));
            }

            _handlers = InitializeHandlers(handlers: handlers);
            _policyProvider = policyProvider;
            _logger = logger ?? new DiagnosticsLoggerFactory().CreateDefaultLogger();
            _contextFactory = contextFactory;
            _evaluator = evaluator;
        }

        /// <summary>
        ///     Checks if a user meets a specific set of requirements for the specified resource.
        /// </summary>
        /// <param name="user">The user to evaluate the requirements against.</param>
        /// <param name="resource">The resource to evaluate the requirements against.</param>
        /// <param name="requirements">The requirements to evaluate.</param>
        /// <returns>
        ///     A flag indicating whether authorization has succeded.
        ///     This value is
        ///     <value>true</value>
        ///     when the user fulfills the policy otherwise
        ///     <value>false</value>
        ///     .
        /// </returns>
        public async Task<bool> AuthorizeAsync(ClaimsPrincipal user, object resource, IEnumerable<IAuthorizationRequirement> requirements)
        {
            if (requirements == null)
            {
                throw new ArgumentNullException(paramName: nameof(requirements));
            }

            var authContext = _contextFactory.CreateContext(requirements: requirements, user: user, resource: resource);
            foreach (var handler in _handlers)
            {
                await handler.HandleAsync(context: authContext);
            }

            if (_evaluator.HasSucceeded(context: authContext))
            {
                _logger.UserAuthorizationSucceeded(userName: GetUserNameForLogging(user: user));
                return true;
            }

            _logger.UserAuthorizationFailed(userName: GetUserNameForLogging(user: user));
            return false;
        }

        /// <summary>
        ///     Checks if a user meets a specific authorization policy.
        /// </summary>
        /// <param name="user">The user to check the policy against.</param>
        /// <param name="resource">The resource the policy should be checked with.</param>
        /// <param name="policyName">The name of the policy to check against a specific context.</param>
        /// <returns>
        ///     A flag indicating whether authorization has succeded.
        ///     This value is
        ///     <value>true</value>
        ///     when the user fulfills the policy otherwise
        ///     <value>false</value>
        ///     .
        /// </returns>
        public async Task<bool> AuthorizeAsync(ClaimsPrincipal user, object resource, string policyName)
        {
            if (policyName == null)
            {
                throw new ArgumentNullException(paramName: nameof(policyName));
            }

            var policy = await _policyProvider.GetPolicyAsync(policyName: policyName);
            if (policy == null)
            {
                throw new InvalidOperationException(message: ResourceHelper.FormatException_AuthorizationPolicyNotFound(p0: policyName));
            }

            return await this.AuthorizeAsync(user: user, resource: resource, policy: policy);
        }

        private static IList<IAuthorizationHandler> InitializeHandlers(IEnumerable<IAuthorizationHandler> handlers)
        {
            Debug.Assert(condition: handlers != null, message: "handlers != null");

            var allHandlers = new List<IAuthorizationHandler>();
            var passThroughFound = false;
            foreach (var handler in handlers)
            {
                if (handler is PassThroughAuthorizationHandler)
                {
                    passThroughFound = true;
                }

                allHandlers.Add(item: handler);
            }

            if (!passThroughFound)
            {
                allHandlers.Add(item: new PassThroughAuthorizationHandler());
            }

            return allHandlers;
        }

        private static string GetUserNameForLogging(ClaimsPrincipal user)
        {
            var identity = user?.Identity;
            if (identity != null)
            {
                var name = identity.Name;
                if (name != null)
                {
                    return name;
                }

                return GetClaimValue(identity: identity, claimsType: "sub") ?? GetClaimValue(identity: identity, claimsType: ClaimTypes.Name) ?? GetClaimValue(identity: identity, claimsType: ClaimTypes.NameIdentifier);
            }

            return null;
        }

        private static string GetClaimValue(IIdentity identity, string claimsType)
        {
            var claimsIdentity = identity as ClaimsIdentity;
            // ReSharper disable once UseNullPropagation because it compiles to more efficient IL
            if (claimsIdentity != null)
            {
                var claim = claimsIdentity.FindFirst(type: claimsType);
                if (claim != null)
                {
                    return claim.Value;
                }
            }

            return null;
        }
    }
}