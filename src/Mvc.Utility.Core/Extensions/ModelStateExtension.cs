using System.Linq;
using System.Web.Mvc;

namespace Mvc.Utility.Core.Extensions
{
    public static partial class Common
    {
        public static void RemoveModelFromModelState<T>(this ModelStateDictionary modelState, T model,
            string nameOfPropertyModel) where T : class, new()
        {
            var properties = model.GetType().GetProperties();
            string key;
            foreach (var item in properties)
            {
                key = string.IsNullOrWhiteSpace(nameOfPropertyModel) ? item.Name : $"{nameOfPropertyModel}.{item.Name}";
                if (modelState.ContainsKey(key))
                    if (modelState[key].Errors.Any())
                        modelState.Remove(key);
            }
        }
    }
}