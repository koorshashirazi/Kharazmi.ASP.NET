using System;
using System.Web.Mvc;

namespace Mvc.Utility.Core.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class PreventDuplicateRequestAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var ctx = filterContext.HttpContext;

            if (ctx.Request["__RequestVerificationToken"] == null) return;

            var currentToken = ctx.Request["__RequestVerificationToken"];

            if (ctx.Session["LastProcessedToken"] == null)
            {
                ctx.Session["LastProcessedToken"] = currentToken;
                return;
            }

            lock (ctx.Session["LastProcessedToken"])
            {
                var lastToken = ctx.Session["LastProcessedToken"].ToString();

                if (lastToken == currentToken)
                {
                    filterContext.Controller.ViewData.ModelState.AddModelError("", ShareResources.DuplicateRequest);
                    return;
                }

                ctx.Session["LastProcessedToken"] = currentToken;
            }
        }
    }
}