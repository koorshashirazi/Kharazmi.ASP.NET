using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Kharazmi.AspNetMvc.Core.Constraints;
using Kharazmi.AspNetMvc.Core.Extensions;
using Mvc.Utility.Core;

namespace Kharazmi.AspNetMvc.Core.Managers
{
    public class RazorManager : RazorViewEngine
    {
        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            return base.CreatePartialView(controllerContext, GetGlobalizationViewPath(controllerContext, partialPath));
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return base.CreateView(controllerContext, GetGlobalizationViewPath(controllerContext, viewPath),
                masterPath);
        }

        public static string RenderViewToString<TModel>(ControllerContext context, string viewPath, TModel model,
            bool partial = true)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var viewEngineResult = partial
                ? ViewEngines.Engines.FindPartialView(context, viewPath)
                : ViewEngines.Engines.FindView(context, viewPath, null);

            if (viewEngineResult == null) throw new FileNotFoundException($"View : [{viewPath}] cannot be found.");

            var view = viewEngineResult.View;
            context.Controller.ViewData.Model = model;

            string result;

            using (var stringWriter = new StringWriter())
            {
                var viewContext =
                    new ViewContext(context, view, context.Controller.ViewData, context.Controller.TempData,
                        stringWriter);
                view.Render(viewContext, stringWriter);
                result = stringWriter.ToString();
            }

            return result;
        }

        public static string RenderToString(PartialViewResult partialView, ControllerContext controllerContext)
        {
            if (controllerContext == null)
                throw new NotSupportedException(
                    "An ControllerContext is required to render the partial view to a string");


            return partialView.RenderToString(controllerContext);
        }

        public static string RenderToString(PartialViewResult partialView)
        {
            var httpContext = HttpContext.Current;

            if (httpContext == null)
                throw new NotSupportedException("An HTTP context is required to render the partial view to a string");

            return partialView.RenderToString();
        }

        public static void SaveToFile(PartialViewResult partialView, string path, FormatFileType formatFile)
        {
            if (partialView == null) throw new ArgumentNullException(nameof(partialView));

            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException(ShareResources.ErrorPathFileNull, nameof(path));

            partialView.SaveToFile(path, formatFile);
        }

        private static string GetGlobalizationViewPath(ControllerContext controllerContext, string viewPath)
        {
            var request = controllerContext.HttpContext.Request;
            var lang = request.Cookies[Constraint.APPLICATION_LANGUAGE_COOKIE_NAME];

            if (lang == null || string.IsNullOrEmpty(lang.Value) || lang.Value == "en-US") return viewPath;

            var localizedViewPath = Regex.Replace(viewPath, "^~/Views/", $"~/Views/Globalization/{lang.Value}/");
            if (File.Exists(request.MapPath(localizedViewPath))) viewPath = localizedViewPath;

            return viewPath;
        }
    }
}