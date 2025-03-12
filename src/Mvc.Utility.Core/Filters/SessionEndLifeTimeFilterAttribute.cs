using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Mvc.Utility.Core.Managers.JsonManager;

namespace Mvc.Utility.Core.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class SessionEndLifeTimeFilterAttribute : ActionFilterAttribute
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Key { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var httpContext = HttpContext.Current;
            var session = httpContext.Session;

            if (session != null)
            {
                var user = session[Key];
                if (user == null && !session.IsNewSession || session.IsNewSession)
                {
                    var sessionCookie = httpContext.Request.Headers["Cookie"];
                    if (null != sessionCookie &&
                        sessionCookie.IndexOf("ASP.NET_SessionId", StringComparison.Ordinal) >= 0)
                    {
                        if (filterContext.HttpContext.Request.IsAjaxRequest())
                            filterContext.Result = new Json
                            {
                                Data = new JsonModel
                                {
                                    RedirectToUrl =
                                        new UrlHelper(filterContext.RequestContext).Action(Action, Controller, null),
                                    Success = false
                                },
                                ContentEncoding = Encoding.UTF8,
                                JsonRequestBehavior = JsonRequestBehavior.AllowGet
                            };
                        else
                            filterContext.Result = new RedirectToRouteResult(
                                new RouteValueDictionary
                                {
                                    {"Controller", Controller}, {"Action", Action}
                                });
                    }
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}