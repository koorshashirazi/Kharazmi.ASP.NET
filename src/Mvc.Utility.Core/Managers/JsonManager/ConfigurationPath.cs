using System;
using System.Collections.Generic;

namespace Mvc.Utility.Core.Managers.JsonManager
{
    /// <summary>
    ///     Utility methods and constants for manipulating Configuration paths
    /// </summary>
    public static class ConfigurationPath
    {
        /// <summary>
        ///     The delimiter ":" used to separate individual keys in a path.
        /// </summary>
        public static readonly string KeyDelimiter = ":";

        /// <summary>Combines path segments into one path.</summary>
        /// <param name="pathSegments">The path segments to combine.</param>
        /// <returns>The combined path.</returns>
        public static string Combine(params string[] pathSegments)
        {
            if (pathSegments == null) throw new ArgumentNullException(nameof(pathSegments));

            return string.Join(KeyDelimiter, pathSegments);
        }

        /// <summary>Combines path segments into one path.</summary>
        /// <param name="pathSegments">The path segments to combine.</param>
        /// <returns>The combined path.</returns>
        public static string Combine(IEnumerable<string> pathSegments)
        {
            if (pathSegments == null) throw new ArgumentNullException(nameof(pathSegments));

            return string.Join(KeyDelimiter, pathSegments);
        }

        /// <summary>Extracts the last path segment from the path.</summary>
        /// <param name="path">The path.</param>
        /// <returns>The last path segment of the path.</returns>
        public static string GetSectionKey(string path)
        {
            if (string.IsNullOrEmpty(path)) return path;

            var num = path.LastIndexOf(KeyDelimiter, StringComparison.OrdinalIgnoreCase);
            if (num != -1) return path.Substring(num + 1);

            return path;
        }

        /// <summary>
        ///     Extracts the path corresponding to the parent node for a given path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        ///     The original path minus the last individual segment found in it. Null if the original path corresponds to a
        ///     top level node.
        /// </returns>
        public static string GetParentPath(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;

            var length = path.LastIndexOf(KeyDelimiter, StringComparison.OrdinalIgnoreCase);
            if (length != -1) return path.Substring(0, length);

            return null;
        }
    }
}