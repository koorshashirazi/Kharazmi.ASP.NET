using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace Mvc.Utility.Core.Managers.JsonManager
{
    internal static class Resources
    {
        private static readonly ResourceManager resource_manager =
            new ResourceManager("Mvc.Utility.Core.Managers.JsonManager.Resources",
                typeof(Resources).GetTypeInfo().Assembly);

        private static readonly List<string> list_of_strings = new List<string>();

        /// <summary>File path must be a non-empty string.</summary>
        internal static string ErrorInvalidFilePath =>
            GetString(nameof(ErrorInvalidFilePath), list_of_strings.ToArray());

        /// <summary>
        ///     Could not parse the JSON file. Error on line number '{0}': '{1}'.
        /// </summary>
        internal static string ErrorJsonParseError => GetString(nameof(ErrorJsonParseError), list_of_strings.ToArray());

        /// <summary>A duplicate key '{0}' was found.</summary>
        internal static string ErrorKeyIsDuplicated =>
            GetString(nameof(ErrorKeyIsDuplicated), list_of_strings.ToArray());

        /// <summary>
        ///     Unsupported JSON token '{0}' was found. Path '{1}', line {2} position {3}.
        /// </summary>
        internal static string ErrorUnsupportedJsonToken =>
            GetString(nameof(ErrorUnsupportedJsonToken), list_of_strings.ToArray());

        /// <summary>File path must be a non-empty string.</summary>
        internal static string FormatError_InvalidFilePath()
        {
            return GetString("Error_InvalidFilePath", list_of_strings.ToArray());
        }

        /// <summary>
        ///     Could not parse the JSON file. Error on line number '{0}': '{1}'.
        /// </summary>
        internal static string FormatError_JSONParseError(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture,
                GetString("Error_JSONParseError", list_of_strings.ToArray()), p0, p1);
        }

        /// <summary>A duplicate key '{0}' was found.</summary>
        internal static string FormatError_KeyIsDuplicated(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture,
                GetString("Error_KeyIsDuplicated", list_of_strings.ToArray()), p0);
        }

        /// <summary>
        ///     Unsupported JSON token '{0}' was found. Path '{1}', line {2} position {3}.
        /// </summary>
        internal static string FormatError_UnsupportedJSONToken(object p0, object p1, object p2, object p3)
        {
            return string.Format(CultureInfo.CurrentCulture,
                GetString("Error_UnsupportedJSONToken", list_of_strings.ToArray()), p0, p1, p2, p3);
        }

        private static string GetString(string name, params string[] formatterNames)
        {
            var str = resource_manager.GetString(name);
            if (formatterNames != null)
                for (var index = 0; index < formatterNames.Length; ++index)
                    str = str?.Replace("{" + formatterNames[index] + "}", "{" + index + "}");
            return str;
        }
    }
}