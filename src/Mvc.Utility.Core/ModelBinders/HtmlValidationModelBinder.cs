using System.Globalization;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Mvc.Utility.Core.ModelBinders
{
    public class HtmlValidationModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            try
            {
                return base.BindModel(controllerContext, bindingContext);
            }
            catch (HttpRequestValidationException)
            {
                var modelState = new ModelState();
                modelState.Errors.Add(ShareResources.ErrorAllowHtmlTag);
                var key = bindingContext.ModelName;
                var value = controllerContext.RequestContext.HttpContext.Request.Unvalidated().Form[key];
                modelState.Value = new ValueProviderResult(value, value, CultureInfo.InvariantCulture);
                bindingContext.ModelState.Add(key, modelState);
            }

            return null;
        }
    }
}