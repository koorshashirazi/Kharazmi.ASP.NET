using System.Linq;
using Kharazmi.AspNetMvc.Core.Extensions;

namespace Kharazmi.AspNetMvc.Core.Managers.UrlManager
{
    public class RequestUrl
    {
        private readonly string _baseUrl;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RequestUrl" /> class.
        /// </summary>
        /// <param name="baseUrl">The authorize endpoint.</param>
        public RequestUrl(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        /// <summary>
        ///     Creates URL based on key/value input pairs.
        /// </summary>
        /// <param name="values">The values (either as a Dictionary of string/string or as a type with properties).</param>
        /// <returns></returns>
        public string Create(object values)
        {
            var dictionary = values.ToDictionary();
            if (dictionary == null || !dictionary.Any()) return _baseUrl;

            return _baseUrl.AddQueryString(dictionary);
        }
    }
}