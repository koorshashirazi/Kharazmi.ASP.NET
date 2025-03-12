using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;

namespace Mvc.Utility.Core.Filters
{
    public class RequiredIfAttribute : ValidationAttribute, IClientValidatable
    {
        private readonly string _otherProperty;

        public RequiredIfAttribute(string otherProperty)
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
            var property = validationContext.ObjectType.GetProperty(_otherProperty);
            if (property == null)
                return new ValidationResult(string.Format(CultureInfo.CurrentCulture, ShareResources.UnknownProperty,
                    _otherProperty));

            var otherPropertyValue = property.GetValue(validationContext.ObjectInstance, null);

            if (!string.IsNullOrWhiteSpace(otherPropertyValue as string) &&
                !string.IsNullOrWhiteSpace(value as string) ||
                (otherPropertyValue == null || string.IsNullOrWhiteSpace(otherPropertyValue as string)) &&
                string.IsNullOrWhiteSpace(value as string)) return ValidationResult.Success;

            return new ValidationResult(string.Format(CultureInfo.CurrentCulture,
                FormatErrorMessage(validationContext.DisplayName), _otherProperty));
        }
    }
}