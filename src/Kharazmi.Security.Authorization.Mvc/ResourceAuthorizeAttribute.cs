using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace Kharazmi.Security.Authorization.Mvc
{
    /// <summary>
    ///     Specifies that the class or method that this attribute is applied to requires the specified authorization.
    /// </summary>
    [SuppressMessage(category: "Microsoft.Performance", checkId: "CA1813:AvoidUnsealedAttributes", Justification = "It must remain extensible")]
    [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class ResourceAuthorizeAttribute : AuthorizeAttribute, IAuthorizeData
    {
        private const string S_CONTROLLER_KEY = "Owin.AuthorizationController";

        /// <inheritdoc />
        public string Policy { get; set; }

        /// <inheritdoc />
        public string ActiveAuthenticationSchemes { get; set; }

        /// <inheritdoc />
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException(paramName: nameof(filterContext));
            }

            if (!filterContext.HttpContext.Items.Contains(key: S_CONTROLLER_KEY))
            {
                filterContext.HttpContext.Items.Add(key: S_CONTROLLER_KEY, value: filterContext.Controller);
            }

            base.OnAuthorization(filterContext: filterContext);
        }

        /// <inheritdoc />
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(paramName: nameof(httpContext));
            }

            var controller = httpContext.Items[key: S_CONTROLLER_KEY] as IAuthorizationController;
            var user = (ClaimsPrincipal) httpContext.User;
            var contextAccessor = new HttpContextBaseOwinContextAccessor(httpContextBase: httpContext);
            var authorizationHelper = new AuthorizationHelper(owinContextAccessor: contextAccessor);
            return authorizationHelper.IsAuthorizedAsync(controller: controller, user: user, authorizeAttribute: this).Result;
        }
    }
}