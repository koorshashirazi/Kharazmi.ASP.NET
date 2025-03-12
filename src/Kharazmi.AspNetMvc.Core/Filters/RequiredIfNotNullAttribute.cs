using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Mvc.Utility.Core;

namespace Kharazmi.AspNetMvc.Core.Filters
{
    public class RequiredIfNotNullAttribute : ValidationAttribute, IClientValidatable
    {
        private readonly string _otherProperty;

        public RequiredIfNotNullAttribute(string otherProperty)
        {
            _otherProperty = otherProperty;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata,
            ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "requiredif"
            };
            rule.ValidationParameters.Add("other", _otherProperty);
            yield return rule;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var errorList = new List<string>();

            var other = _otherProperty.Substring(_otherProperty.LastIndexOf('.') + 1,
                _otherProperty.Length - _otherProperty.LastIndexOf('.') - 1);

            var otherProperty = validationContext.ObjectType.GetProperty(other);
            if (otherProperty == null)
            {
                errorList.Add(string.Format(CultureInfo.CurrentCulture, ShareResources.UnknownProperty, other));
                return new ValidationResult(string.Join(" ", errorList));
            }

            var otherPropertyValue = otherProperty.GetValue(validationContext.ObjectInstance, null);

            if (value != null && otherPropertyValue == null || otherPropertyValue as string == string.Empty)
                errorList.Add(string.Format(CultureInfo.CurrentCulture,
                    FormatErrorMessage(validationContext.DisplayName), other));

            return errorList.Any() ? new ValidationResult(string.Join(" ", errorList)) : ValidationResult.Success;
        }
    }
}