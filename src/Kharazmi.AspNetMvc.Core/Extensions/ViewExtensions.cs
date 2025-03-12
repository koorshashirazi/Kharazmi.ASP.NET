using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Kharazmi.AspNetMvc.Core.Constraints;
using Mvc.Utility.Core;

namespace Kharazmi.AspNetMvc.Core.Extensions
{
    public static partial class Common
    {
        public static string RenderToString(this PartialViewResult partialView, ControllerContext controllerContext)
        {
            if (controllerContext == null)
                throw new NotSupportedException(
                    "An ControllerContext is required to render the partial view to a string");

            var view = ViewEngines.Engines.FindPartialView(controllerContext, partialView.ViewName)
                .View;

            var sb = new StringBuilder();

            using (var sw = new StringWriter(sb))
            {
                using (var tw = new HtmlTextWriter(sw))
                {
                    view.Render(
                        new ViewContext(controllerContext, view, partialView.ViewData, partialView.TempData, tw), tw);
                }
            }

            return sb.ToString();
        }

        public static string RenderToString(this PartialViewResult partialView)
        {
            var httpContext = HttpContext.Current;

            if (httpContext == null)
                throw new NotSupportedException("An HTTP context is required to render the partial view to a string");

            var controllerName = httpContext.Request.RequestContext.RouteData.Values["controller"].ToString();

            var controller = (ControllerBase) ControllerBuilder.Current.GetControllerFactory()
                .CreateController(
                    httpContext
                        .Request.RequestContext,
                    controllerName);

            var controllerContext = new ControllerContext(httpContext.Request.RequestContext, controller);

            var view = ViewEngines.Engines.FindPartialView(controllerContext, partialView.ViewName)
                .View;

            var sb = new StringBuilder();

            using (var sw = new StringWriter(sb))
            {
                using (var tw = new HtmlTextWriter(sw))
                {
                    view.Render(
                        new ViewContext(controllerContext, view, partialView.ViewData, partialView.TempData, tw), tw);
                }
            }

            return sb.ToString();
        }

        public static void SaveToFile(this PartialViewResult partialView, string pathFile, FormatFileType formatFile)
        {
            if (partialView == null) throw new ArgumentNullException(nameof(partialView));

            if (string.IsNullOrWhiteSpace(pathFile))
                throw new ArgumentException(ShareResources.ErrorPathFileNull, nameof(pathFile));

            var startTagHtml = string.Empty;
            var endTagHtml = string.Empty;

            switch (formatFile)
            {
                case FormatFileType.Js:
                    startTagHtml = "<script>";
                    endTagHtml = "</script>";
                    break;

                case FormatFileType.Css:
                    startTagHtml = "<style>";
                    endTagHtml = "</style>";
                    break;

                case FormatFileType.Text:
                    break;
            }

            var byteArray =
                Encoding.ASCII.GetBytes(
                    partialView.RenderToString()
                        .Replace(startTagHtml, "")
                        .Replace(endTagHtml, ""));
            byteArray.WriteToFile(pathFile);
        }
    }
}