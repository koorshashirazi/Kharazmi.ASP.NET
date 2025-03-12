using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Kharazmi.AspNetMvc.Core.Extensions;
using Mvc.Utility.Core;

namespace Kharazmi.AspNetMvc.Core.Filters
{
    public class PasswordValidatorAttribute : ValidationAttribute, IClientValidatable
    {
        public int RequiredLength { get; set; }
        public bool IsRequireNonLetterOrDigit { get; set; }
        public bool IsRequireLowercase { get; set; }
        public bool IsRequireUppercase { get; set; }
        public bool IsRequireDigit { get; set; }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata,
            ControllerContext context)
        {
            var passwordRule = new ModelClientValidationRule
            {
                ErrorMessage = ErrorMessageString,
                ValidationType = "datapasswordvalidation"
            };
            yield return passwordRule;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var item = value == null ? string.Empty : value.ToString();

            var reasonList = new List<string>();

            if (string.IsNullOrWhiteSpace(item) || item.Length < RequiredLength)
                reasonList.Add(string.Format(ShareResources.ErrorMessageMinStringLength, ShareResources.Password,
                    RequiredLength));

            if (IsRequireNonLetterOrDigit && item.All(x => x.IsLetterOrDigit()))
                reasonList.Add(string.Format(ShareResources.ErrorMessageRequireNonLetterOrDigit));

            if (IsRequireDigit && item.All(c => !c.IsDigit())) reasonList.Add(ShareResources.ErrorMessageRequireDigit);

            if (IsRequireLowercase && item.All(c => !c.IsLower()))
                reasonList.Add(ShareResources.ErrorMessageRequireLowercase);

            if (IsRequireUppercase && item.All(c => !c.IsUpper()))
                reasonList.Add(ShareResources.ErrorMessageRequireUppercase);

            return reasonList.Count == 0
                ? ValidationResult.Success
                : new ValidationResult(string.Join(" ", reasonList));
        }
    }
}