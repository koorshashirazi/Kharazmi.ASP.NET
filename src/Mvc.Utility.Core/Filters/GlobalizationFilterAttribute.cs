using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Mvc.Utility.Core.Constraints;
using Mvc.Utility.Core.Extensions;
using Mvc.Utility.Core.Helpers;

namespace Mvc.Utility.Core.Filters
{
    public class GlobalizationFilterAttribute : ActionFilterAttribute
    {
        private readonly string _cookieName;
        private readonly DateTime _expires;

        public GlobalizationFilterAttribute(int expireTime, ExpiresTimeType expiresTimeType, string cookieName)
        {
            if (string.IsNullOrWhiteSpace(cookieName)) throw new ArgumentException("message", nameof(cookieName));

            _expires = expireTime.GetDateTime(expiresTimeType);
            _cookieName = cookieName;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var currentRequest = filterContext.HttpContext.Request;
            var currentResponse = filterContext.HttpContext.Response;

            var userLanguage = currentRequest.UserLanguages;

            var cookie = currentRequest.Cookies[_cookieName];
            CultureInfo languageToSet = null;
            if (cookie != null)
            {
                var cookieValues = cookie.Value.Split('/');
                languageToSet = CultureHelper.GetValidCulture(cookieValues.Any() ? cookieValues[0] : "");
            }
            else
            {
                languageToSet = userLanguage != null && userLanguage.Length > 0
                    ? CultureHelper.GetValidCulture(userLanguage[0])
                    : CultureHelper.GetDefaultCulture;

                var httpCookie = new HttpCookie(_cookieName, $"{languageToSet.Name}/{languageToSet.DisplayName}")
                {
                    Expires = _expires
                };
                currentResponse.SetCookie(httpCookie);
            }

            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(languageToSet.Name);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(languageToSet.Name);
        }
    }
}