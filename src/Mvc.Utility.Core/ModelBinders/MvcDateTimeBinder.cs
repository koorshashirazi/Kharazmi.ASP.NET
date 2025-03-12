using System;
using System.Web.Mvc;
using Mvc.Utility.Core.Timing;

namespace Mvc.Utility.Core.ModelBinders
{
    public class MvcDateTimeBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var date = base.BindModel(controllerContext, bindingContext) as DateTime?;
            if (date == null)
            {
                return null;
            }

            if (bindingContext.ModelMetadata.ContainerType != null)
            {
                if (bindingContext.ModelMetadata.ContainerType.IsDefined(typeof(DisableDateTimeNormalizationAttribute), true))
                {
                    return date.Value;
                }

                var property = bindingContext.ModelMetadata.ContainerType.GetProperty(bindingContext.ModelName);

                if (property != null && property.IsDefined(typeof(DisableDateTimeNormalizationAttribute), true))
                {
                    return date.Value;
                }
            }


            // Note: currently DisableDateTimeNormalizationAttribute is not supported for MVC action parameters.
            return Clock.Normalize(date.Value);
        }
    }
}