using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace Kharazmi.AspNetMvc.Core.Managers.Authorization
{
    /// <summary>
    ///     Specifies that the class or method that this attribute is applied to requires the specified authorization.
    /// </summary>
    [SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes", Justification =
        "It must remain extensible")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
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
            if (filterContext == null) throw new ArgumentNullException(nameof(filterContext));

            if (!filterContext.HttpContext.Items.Contains(S_CONTROLLER_KEY))
                filterContext.HttpContext.Items.Add(S_CONTROLLER_KEY, filterContext.Controller);

            base.OnAuthorization(filterContext);
        }

        /// <inheritdoc />
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));

            var controller = httpContext.Items[S_CONTROLLER_KEY] as IAuthorizationController;
            var user = (ClaimsPrincipal) httpContext.User;
            var contextAccessor = new HttpContextBaseOwinContextAccessor(httpContext);
            var authorizationHelper = new AuthorizationHelper(contextAccessor);
            return authorizationHelper.IsAuthorizedAsync(controller, user, this).Result;
        }
    }
}