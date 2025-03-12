using System;
using System.Globalization;
using System.Web.Mvc;
using Kharazmi.AspNetMvc.Core.Extensions;
using Mvc.Utility.Core;

namespace Kharazmi.AspNetMvc.Core.ModelBinders
{
    public class DecimalModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueResult = bindingContext.ValueProvider
                .GetValue(bindingContext.ModelName);
            var modelState = new ModelState {Value = valueResult};
            if (valueResult.AttemptedValue == null) return null;

            object actualValue = null;
            try
            {
                var value = valueResult.AttemptedValue.GetEnglishNumber();
                actualValue = decimal.Parse(value, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                modelState.Errors.Add(ShareResources.ErrorMessageDecimalNumber);
            }

            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            return actualValue;
        }
    }
}