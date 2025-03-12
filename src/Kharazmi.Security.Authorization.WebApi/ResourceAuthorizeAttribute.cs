using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Kharazmi.Security.Authorization.WebApi
{
    /// <summary>
    ///     Specifies that the class or method that this attribute is applied to requires the specified authorization.
    /// </summary>
    [SuppressMessage(category: "Microsoft.Performance", checkId: "CA1813:AvoidUnsealedAttributes", Justification = "It must remain extensible")]
    [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class ResourceAuthorizeAttribute : AuthorizeAttribute, IAuthorizeData
    {
        /// <inheritdoc />
        public string Policy { get; set; }

        /// <inheritdoc />
        public string ActiveAuthenticationSchemes { get; set; }

        /// <inheritdoc />
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException(paramName: nameof(actionContext));
            }

            var controller = actionContext.ControllerContext.Controller as IAuthorizationController;
            var user = (ClaimsPrincipal) actionContext.RequestContext.Principal;
            var owinAccessor = new HttpRequestMessageOwinContextAccessor(httpRequestMessage: actionContext.Request);
            var helper = new AuthorizationHelper(owinContextAccessor: owinAccessor);
            return helper.IsAuthorizedAsync(controller: controller, user: user, authorizeAttribute: this).Result;
        }
    }
}