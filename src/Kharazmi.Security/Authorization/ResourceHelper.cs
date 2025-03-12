using System.Globalization;

namespace Kharazmi.Security.Authorization
{
    internal static class ResourceHelper
    {
        /// <summary>
        ///     The AuthorizationPolicy named: '{0}' was not found.
        /// </summary>
        internal static string FormatException_AuthorizationPolicyNotFound(object p0) { return string.Format(CultureInfo.CurrentCulture, ShareResources.Exception_AuthorizationPolicyNotFound, p0); }
    }
}