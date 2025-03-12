using System;

namespace Kharazmi.DirectoryServices.Ldap.LinqToLdap.Helpers
{
    /// <summary>
    ///     Static class for parsing distinguished name information
    /// </summary>
    public static class DnParser
    {
        /// <summary>
        ///     Parses the first name of an entry without the RDN prefix (CN, OU, etc.) from <paramref name="distinguishedName" />
        ///     and returns that value.
        /// </summary>
        /// <param name="distinguishedName">The distinguished name to parse.</param>
        /// <exception cref="ArgumentException">
        ///     Thrown if <paramref name="distinguishedName" /> is null, empty, white space, or not
        ///     a valid distinguished name.
        /// </exception>
        /// <returns></returns>
        public static string ParseName(string distinguishedName)
        {
            string name;
            if (distinguishedName.IsNullOrEmpty())
                throw new ArgumentException(string.Format("Invalid distinguished name '{0}'.", distinguishedName),
                    "distinguishedName");
            var firstEquals = distinguishedName.IndexOf('=');
            if (firstEquals <= 0)
                throw new ArgumentException(
                    string.Format("Common name could not be parsed from distinguished name '{0}'.", distinguishedName),
                    "distinguishedName");

            var secondEquals = distinguishedName.IndexOf('=', firstEquals + 1);

            if (secondEquals <= 0)
            {
                name = distinguishedName.Substring(firstEquals + 1);
                return name;
            }

            var sub = distinguishedName.Substring(firstEquals, secondEquals);
            var lastComma = sub.LastIndexOf(',');
            if (lastComma <= 0)
                throw new ArgumentException(
                    string.Format("Common name could not be parsed from distinguished name '{0}'.", distinguishedName),
                    "distinguishedName");

            name = distinguishedName.Substring(firstEquals + 1, lastComma - 1);
            return name;
        }

        /// <summary>
        ///     Parses the first RDN attribute type.
        /// </summary>
        /// <param name="distinguishedName">The distinguished name.</param>
        /// <example>
        ///     OU=Test,DC=local returns OU
        /// </example>
        /// <returns></returns>
        /// <exception cref="ArgumentException">
        ///     Thrown if <paramref name="distinguishedName" /> is null, empty, white space, or has
        ///     an invalid format.
        /// </exception>
        public static string ParseRDN(string distinguishedName)
        {
            if (distinguishedName.IsNullOrEmpty())
                throw new ArgumentException(string.Format("Invalid distinguished name '{0}'.", distinguishedName),
                    "distinguishedName");
            var startIndex = distinguishedName.IndexOf('=');
            if (startIndex <= 0)
                throw new ArgumentException(
                    string.Format("Name could not be parsed from distinguished name '{0}'.", distinguishedName),
                    "distinguishedName");

            return distinguishedName.Substring(0, startIndex);
        }

        internal static string GetEntryName(string distinguishedName)
        {
            if (distinguishedName.IsNullOrEmpty())
                throw new ArgumentException(string.Format("Invalid distinguished name '{0}'.", distinguishedName),
                    "distinguishedName");
            var firstEquals = distinguishedName.IndexOf('=');
            if (firstEquals <= 0)
                throw new ArgumentException(
                    string.Format("Common name could not be parsed from distinguished name '{0}'.", distinguishedName),
                    "distinguishedName");

            var secondEquals = distinguishedName.IndexOf('=', firstEquals + 1);

            if (secondEquals <= 0) return distinguishedName;

            var sub = distinguishedName.Substring(firstEquals, secondEquals);
            var lastComma = sub.LastIndexOf(',');
            if (lastComma <= 0)
                throw new ArgumentException(
                    string.Format("Common name could not be parsed from distinguished name '{0}'.", distinguishedName),
                    "distinguishedName");
            return distinguishedName.Substring(0, firstEquals + lastComma);
        }

        internal static string GetEntryContainer(string distinguishedName)
        {
            if (distinguishedName.IsNullOrEmpty())
                throw new ArgumentException(string.Format("Invalid distinguished name '{0}'.", distinguishedName),
                    "distinguishedName");
            var firstEquals = distinguishedName.IndexOf('=');
            if (firstEquals <= 0)
                throw new ArgumentException(
                    string.Format("Common name could not be parsed from distinguished name '{0}'.", distinguishedName),
                    "distinguishedName");

            var secondEquals = distinguishedName.IndexOf('=', firstEquals + 1);

            if (secondEquals <= 0) return distinguishedName;

            var sub = distinguishedName.Substring(firstEquals, secondEquals);
            var lastComma = sub.LastIndexOf(',');
            if (lastComma <= 0)
                throw new ArgumentException(
                    string.Format("Common name could not be parsed from distinguished name '{0}'.", distinguishedName),
                    "distinguishedName");
            return distinguishedName.Substring(firstEquals + lastComma + 1);
        }

        internal static string FormatName(string name, string currentDistinguishedName)
        {
            if (name.IndexOf("=", StringComparison.Ordinal) >= 0) return name;

            var index = currentDistinguishedName.IndexOf("=", StringComparison.Ordinal);

            return currentDistinguishedName.Substring(0, index + 1) + name;
        }
    }
}