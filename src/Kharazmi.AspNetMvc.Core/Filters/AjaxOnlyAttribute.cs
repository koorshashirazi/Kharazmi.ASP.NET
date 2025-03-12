using System;
using System.Net;
using System.Web.Mvc;
using Kharazmi.AspNetMvc.Core.Helpers;

namespace Kharazmi.AspNetMvc.Core.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false)]
    public sealed class AjaxOnlyAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
                throw ExceptionHelper.ThrowException<UnauthorizedAccessException>(string.Empty);

            ErrorHandleAjaxRequest(filterContext);
            base.HandleUnauthorizedRequest(filterContext);
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            IsAjaxCall(filterContext);
        }

        private static void IsAjaxCall(AuthorizationContext filterContext)
        {
            var ctx = filterContext.HttpContext;

            if (ctx.Request.IsAjaxRequest()) return;

            ctx.Response.Clear();
            ctx.Response.StatusCode = (int) HttpStatusCode.Forbidden;
            throw ExceptionHelper.ThrowException<UnauthorizedAccessException>(string.Empty);
        }

        private static void ErrorHandleAjaxRequest(AuthorizationContext filterContext)
        {
            var ctx = filterContext.HttpContext;
            if (!ctx.Request.IsAjaxRequest()) return;

            ctx.Response.Clear();
            ctx.Response.StatusCode = (int) HttpStatusCode.Forbidden;
            throw ExceptionHelper.ThrowException<UnauthorizedAccessException>(string.Empty);
        }
    }
}