using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Mvc.Utility.Core.Helpers;

namespace Mvc.Utility.Core.Extensions
{
    public static partial class Common
    {
        /// <summary>
        ///     Creates a SHA256 hash of the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>A hash</returns>
        public static string ToSha256(this string input)
        {
            if (input.IsMissing()) return string.Empty;

            using (var sha = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                var hash = sha.ComputeHash(bytes);

                return Convert.ToBase64String(hash);
            }
        }

        /// <summary>
        ///     Creates a SHA512 hash of the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>A hash</returns>
        public static string ToSha512(this string input)
        {
            if (input.IsMissing()) return string.Empty;

            using (var sha = SHA512.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                var hash = sha.ComputeHash(bytes);

                return Convert.ToBase64String(hash);
            }
        }

        public static string ToChallengeCode(this string codeVerifier)
        {
            using (var sha256 = SHA256.Create())
            {
                var challengeBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(codeVerifier));
                var codeChallenge = Convert.ToBase64String(challengeBytes)
                    .TrimEnd('=')
                    .Replace('+', '-')
                    .Replace('/', '_');

                return codeChallenge;
            }
        }

        /// <summary>
        ///     Checks two strings for equality without leaking timing information.
        /// </summary>
        /// <param name="s1">string 1.</param>
        /// <param name="s2">string 2.</param>
        /// <returns>
        ///     <c>true</c> if the specified strings are equal; otherwise, <c>false</c>.
        /// </returns>
        [MethodImpl(MethodImplOptions.NoOptimization)]
        public static bool IsEqual(this string s1, string s2)
        {
            if (s1 == null && s2 == null) return true;

            if (s1 == null || s2 == null) return false;

            if (s1.Length != s2.Length) return false;

            var s1Chars = s1.ToCharArray();
            var s2Chars = s2.ToCharArray();

            var hits = 0;
            for (var i = 0; i < s1.Length; i++)
                if (s1Chars[i].Equals(s2Chars[i]))
                    hits += 2;
                else
                    hits += 1;

            var same = hits == s1.Length * 2;

            return same;
        }

        [DebuggerStepThrough]
        public static bool IsMissing(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        [DebuggerStepThrough]
        public static bool IsPresent(this string value)
        {
            return !value.IsMissing();
        }

        [DebuggerStepThrough]
        public static string EnsureTrailingSlash(this string url)
        {
            if (!url.EndsWith("/")) return url + "/";

            return url;
        }

        [DebuggerStepThrough]
        public static string RemoveTrailingSlash(this string url)
        {
            if (url != null && url.EndsWith("/")) url = url.Substring(0, url.Length - 1);

            return url;
        }

        public static bool ToBoolean(this string value)
        {
            switch (value.ToLower())
            {
                case "true":
                    return true;

                case "1":
                    return true;

                case "0":
                    return false;

                case "false":
                    return false;

                default:
                    throw ExceptionHelper.ThrowException<InvalidCastException>(
                        ResourceHelper.ReadResourceValue(typeof(ShareResources), nameof(InvalidCastException), "bool"));
            }
        }

        public static byte[] ToBytes(this string s)
        {
            return Encoding.ASCII.GetBytes(s);
        }

        public static bool IsDigit(this char c)
        {
            if (c >= 48) return c <= 57;

            return false;
        }

        public static bool IsLower(this char c)
        {
            if (c >= 97) return c <= 122;

            return false;
        }

        public static bool IsUpper(this char c)
        {
            if (c >= 65) return c <= 90;

            return false;
        }

        public static bool IsLetterOrDigit(this char c)
        {
            if (!IsUpper(c) && !IsLower(c)) return IsDigit(c);

            return true;
        }

        /// <summary>
        ///     Splits at the first occurence of the given separator.
        /// </summary>
        /// <param name="s">The string to split.</param>
        /// <param name="separator">The separator to split on.</param>
        /// <returns>Array of at most 2 strings. (1 if separator is not found.)</returns>
        public static string[] SplitOnFirstOccurence(this string s, char separator)
        {
            // Needed because full PCL profile doesn't support Split(char[], int) (#119)
            if (string.IsNullOrEmpty(s)) return new[] { s };

            var i = s.IndexOf(separator);
            return i == -1 ? new[] { s } : new[] { s.Substring(0, i), s.Substring(i + 1) };
        }

        public static string ToString(this string[] stringArray, string splitChar)
        {
            return string.Join(splitChar, stringArray);
        }

        /// <summary>
        ///     Returns a string that represents the current object, using CultureInfo.InvariantCulture where possible.
        ///     Dates are represented in IS0 8601.
        /// </summary>
        public static string ToInvariantString(this object obj)
        {
            // inspired by: http://stackoverflow.com/a/19570016/62600
            return
                obj == null ? null :
                obj is DateTime dt ? dt.ToString("o", CultureInfo.InvariantCulture) :
                obj is DateTimeOffset dto ? dto.ToString("o", CultureInfo.InvariantCulture) :
#if !NETSTANDARD1_0
                obj is IConvertible c ? c.ToString(CultureInfo.InvariantCulture) :
#endif
                obj is IFormattable f ? f.ToString(null, CultureInfo.InvariantCulture) :
                obj.ToString();
        }

        /// <summary>
        ///     Strips any single quotes or double quotes from the beginning and end of a string.
        /// </summary>
        public static string StripQuotes(this string s)
        {
            return Regex.Replace(s, "^\\s*['\"]+|['\"]+\\s*$", "");
        }

        public const string CarriageReturnLineFeed = "\r\n";
        public const string Empty = "";
        public const char CarriageReturn = '\r';
        public const char LineFeed = '\n';
        public const char Tab = '\t';

        #region Char extensions

        [DebuggerStepThrough]
        public static int ToInt(this char value)
        {
            if (value >= '0' && value <= '9')
                return value - '0';
            if (value >= 'a' && value <= 'f')
                return value - 'a' + 10;
            if (value >= 'A' && value <= 'F')
                return value - 'A' + 10;
            return -1;
        }

        #endregion

        #region String extensions

        //public static string GetSummaryFromHtml(this string html, int max)
        //{
        //    var summaryHtml = string.Empty;
        //    var words = html.CleanTags().Split(new[] {' '});
        //    var builder = new StringBuilder();
        //    builder.Append(summaryHtml);

        //    for (var i = 0; i < max; i++)
        //        builder.Append(words[i]);
        //    summaryHtml = builder.ToString();

        //    return summaryHtml;
        //}

        public static string GetSummaryFromText(this string text, int max)
        {
            var summaryHtml = string.Empty;
            var words = text.Split(' ');
            var builder = new StringBuilder();
            builder.Append(summaryHtml);

            for (var i = 0; i < max; i++)
                builder.Append(words[i]);
            summaryHtml = builder.ToString();

            return summaryHtml;
        }

        [DebuggerStepThrough]
        public static T ToEnum<T>(this string value, T defaultValue)
        {
            if (!value.HasValue())
                return defaultValue;
            try
            {
                return (T)Enum.Parse(typeof(T), value, true);
            }
            catch (ArgumentException)
            {
                return defaultValue;
            }
        }

        [DebuggerStepThrough]
        public static string ToSafe(this string value, string defaultValue = null)
        {
            if (!string.IsNullOrEmpty(value))
                return value;
            return defaultValue ?? string.Empty;
        }

        [DebuggerStepThrough]
        public static string EmptyNull(this string value)
        {
            return (value ?? string.Empty).Trim();
        }

        [DebuggerStepThrough]
        public static string NullEmpty(this string value)
        {
            return string.IsNullOrEmpty(value) ? null : value;
        }

        /// <summary>
        ///     Formats a string to an invariant culture
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="objects">The objects.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string FormatInvariant(this string format, params object[] objects)
        {
            return string.Format(CultureInfo.InvariantCulture, format, objects);
        }

        /// <summary>
        ///     Formats a string to the current culture.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="objects">The objects.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string FormatCurrent(this string format, params object[] objects)
        {
            return string.Format(CultureInfo.CurrentCulture, format, objects);
        }

        /// <summary>
        ///     Formats a string to the current UI culture.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="objects">The objects.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string FormatCurrentUI(this string format, params object[] objects)
        {
            return string.Format(CultureInfo.CurrentUICulture, format, objects);
        }

        [DebuggerStepThrough]
        public static string FormatWith(this string format, params object[] args)
        {
            return FormatWith(format, CultureInfo.CurrentCulture, args);
        }

        [DebuggerStepThrough]
        public static string FormatWith(this string format, IFormatProvider provider, params object[] args)
        {
            return string.Format(provider, format, args);
        }

        /// <summary>
        ///     Determines whether this instance and another specified System.String object have the same value.
        /// </summary>
        /// <param name="value">The string to check equality.</param>
        /// <param name="comparing">The comparing with string.</param>
        /// <returns>
        ///     <c>true</c> if the value of the comparing parameter is the same as this string; otherwise, <c>false</c>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool IsCaseSensitiveEqual(this string value, string comparing)
        {
            return string.CompareOrdinal(value, comparing) == 0;
        }

        /// <summary>
        ///     Determines whether this instance and another specified System.String object have the same value.
        /// </summary>
        /// <param name="value">The string to check equality.</param>
        /// <param name="comparing">The comparing with string.</param>
        /// <returns>
        ///     <c>true</c> if the value of the comparing parameter is the same as this string; otherwise, <c>false</c>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool IsCaseInsensitiveEqual(this string value, string comparing)
        {
            return string.Compare(value, comparing, StringComparison.OrdinalIgnoreCase) == 0;
        }

        /// <summary>
        ///     Determines whether the string is null, empty or all whitespace.
        /// </summary>
        /// <param name="value">todo: describe value parameter on IsEmpty</param>
        [DebuggerStepThrough]
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }


        /// <summary>
        ///     Determines whether the string is null, null or all whitespace.
        /// </summary>
        /// <param name="value">todo: describe value parameter on IsEmpty</param>
        [DebuggerStepThrough]
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }


        /// <summary>
        ///     Determines whether the string is all white space. Empty string will return false.
        /// </summary>
        /// <param name="value">The string to test whether it is all white space.</param>
        /// <returns>
        ///     <c>true</c> if the string is all white space; otherwise, <c>false</c>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool IsWhiteSpace(this string value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            if (value.Length == 0)
                return false;

            return value.All(char.IsWhiteSpace);
        }

        [DebuggerStepThrough]
        public static bool HasValue(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }


        /// <summary>
        ///     Mask by replacing characters with asterisks.
        /// </summary>
        /// <param name="value">The string</param>
        /// <param name="length">Number of characters to leave untouched.</param>
        /// <returns>The mask string</returns>
        [DebuggerStepThrough]
        public static string Mask(this string value, int length)
        {
            if (value.HasValue())
                return value.Substring(0, length) + new string('*', value.Length - length);
            return value;
        }


        /// <summary>
        ///     Ensures that a string only contains numeric values
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Input string with only numeric values, empty string if input is null or empty</returns>
        [DebuggerStepThrough]
        public static string EnsureNumericOnly(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            return new string(str.Where(c => char.IsDigit(c)).ToArray());
        }


        [DebuggerStepThrough]
        public static string Truncate(this string value, int maxLength, string suffix = "")
        {
            if (suffix == null) throw new ArgumentNullException(nameof(suffix));

            var subStringLength = maxLength - suffix.Length;

            if (subStringLength <= 0)
                throw new ArgumentException("Length of suffix string is greater or equal to maximumLength",
                    nameof(maxLength));

            if (value != null && value.Length > maxLength)
            {
                var truncatedString = value.Substring(0, subStringLength);
                // in case the last character is a space
                truncatedString = truncatedString.Trim();
                truncatedString += suffix;

                return truncatedString;
            }
            return value;
        }

        /// <summary>
        ///     Ensure that a string starts with a string.
        /// </summary>
        /// <param name="value">The target string</param>
        /// <param name="startsWith">The string the target string should start with</param>
        /// <returns>The resulting string</returns>
        [DebuggerStepThrough]
        public static string EnsureStartsWith(this string value, string startsWith)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (startsWith == null) throw new ArgumentNullException(nameof(startsWith));

            return value.StartsWith(startsWith) ? value : startsWith + value;
        }

        /// <summary>
        ///     Ensures the target string ends with the specified string.
        /// </summary>
        /// <param name="endWith">The target.</param>
        /// <param name="value">The value.</param>
        /// <returns>The target string with the value string at the end.</returns>
        [DebuggerStepThrough]
        public static string EnsureEndsWith(this string value, string endWith)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (endWith == null) throw new ArgumentNullException(nameof(endWith));

            if (value.Length < endWith.Length) return value + endWith;

            if (
                string.Compare(value, value.Length - endWith.Length, endWith, 0, endWith.Length,
                    StringComparison.OrdinalIgnoreCase) == 0)
                return value;

            var trimmedString = value.TrimEnd(null);

            if (
                string.Compare(trimmedString, trimmedString.Length - endWith.Length, endWith, 0, endWith.Length,
                    StringComparison.OrdinalIgnoreCase) == 0)
                return value;

            return value + endWith;
        }

        [DebuggerStepThrough]
        public static string UrlEncode(this string value)
        {
            return HttpUtility.UrlEncode(value);
        }

        [DebuggerStepThrough]
        public static string UrlDecode(this string value)
        {
            return HttpUtility.UrlDecode(value);
        }

        [DebuggerStepThrough]
        public static string AttributeEncode(this string value)
        {
            return HttpUtility.HtmlAttributeEncode(value);
        }

        [DebuggerStepThrough]
        public static string HtmlEncode(this string value)
        {
            return HttpUtility.HtmlEncode(value);
        }

        [DebuggerStepThrough]
        public static string HtmlDecode(this string value)
        {
            return HttpUtility.HtmlDecode(value);
        }


        /// <summary>
        ///     Replaces pascal casing with spaces. For example "CustomerId" would become "Customer Id".
        ///     Strings that already contain spaces are ignored.
        /// </summary>
        /// <param name="value">String to split</param>
        /// <returns>The string after being split</returns>
        [DebuggerStepThrough]
        public static string SplitPascalCase(this string value)
        {
            //return Regex.Replace(input, "([A-Z][a-z])", " $1", RegexOptions.Compiled).Trim();
            var sb = new StringBuilder();
            var ca = value.ToCharArray();
            sb.Append(ca[0]);
            for (var i = 1; i < ca.Length - 1; i++)
            {
                var c = ca[i];
                if (char.IsUpper(c) && (char.IsLower(ca[i + 1]) || char.IsLower(ca[i - 1])))
                    sb.Append(" ");
                sb.Append(c);
            }
            if (ca.Length > 1)
                sb.Append(ca[ca.Length - 1]);

            return sb.ToString();
        }

        [DebuggerStepThrough]
        public static string[] SplitSafe(this string value, string separator)
        {
            return string.IsNullOrEmpty(value)
                ? new string[0]
                : value.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
        }

        [DebuggerStepThrough]
        [SuppressMessage("ReSharper", "StringIndexOfIsCultureSpecific.1")]
        public static bool SplitToPair(this string value, out string strLeft, out string strRight, string delimiter)
        {
            int idx;
            if (value.IsEmpty() || delimiter.IsEmpty() || (idx = value.IndexOf(delimiter)) == -1)
            {
                strLeft = value;
                strRight = "";
                return false;
            }
            strLeft = value.Substring(0, idx);
            strRight = value.Substring(idx + delimiter.Length);
            return true;
        }


        [DebuggerStepThrough]
        public static bool IsEnclosedIn(this string value, string enclosedIn)
        {
            return value.IsEnclosedIn(enclosedIn, StringComparison.CurrentCulture);
        }

        [DebuggerStepThrough]
        public static bool IsEnclosedIn(this string value, string enclosedIn, StringComparison comparisonType)
        {
            if (string.IsNullOrEmpty(enclosedIn))
                return false;

            if (enclosedIn.Length == 1)
                return value.IsEnclosedIn(enclosedIn, enclosedIn, comparisonType);

            if (enclosedIn.Length % 2 == 0)
            {
                var len = enclosedIn.Length / 2;
                return value.IsEnclosedIn(
                    enclosedIn.Substring(0, len),
                    enclosedIn.Substring(len, len),
                    comparisonType);
            }

            return false;
        }

        [DebuggerStepThrough]
        public static bool IsEnclosedIn(this string value, string start, string end)
        {
            return value.IsEnclosedIn(start, end, StringComparison.CurrentCulture);
        }

        [DebuggerStepThrough]
        public static bool IsEnclosedIn(this string value, string start, string end, StringComparison comparisonType)
        {
            return value.StartsWith(start, comparisonType) && value.EndsWith(end, comparisonType);
        }

        public static string RemoveEncloser(this string value, string encloser)
        {
            return value.RemoveEncloser(encloser, StringComparison.CurrentCulture);
        }

        public static string RemoveEncloser(this string value, string encloser, StringComparison comparisonType)
        {
            if (value.IsEnclosedIn(encloser, comparisonType))
            {
                var len = encloser.Length / 2;
                return value.Substring(
                    len,
                    value.Length - len * 2);
            }

            return value;
        }

        public static string RemoveEncloser(this string value, string start, string end)
        {
            return value.RemoveEncloser(start, end, StringComparison.CurrentCulture);
        }

        public static string RemoveEncloser(this string value, string start, string end,
            StringComparison comparisonType)
        {
            if (value.IsEnclosedIn(start, end, comparisonType))
                return value.Substring(
                    start.Length,
                    value.Length - (start.Length + end.Length));

            return value;
        }

        /// <summary>Debug.WriteLine</summary>
        [DebuggerStepThrough]
        public static void Dump(this string value, bool appendMarks = false)
        {
            Debug.WriteLine(value);
            Debug.WriteLineIf(appendMarks, "------------------------------------------------");
        }

        /// <summary>Smart way to create a HTML attribute with a leading space.</summary>
        /// <param name="value">Name of the attribute.</param>
        /// <param name="name"></param>
        /// <param name="htmlEncode"></param>
        [SuppressMessage("ReSharper", "StringCompareIsCultureSpecific.3")]
        public static string ToAttribute(this string value, string name, bool htmlEncode = true)
        {
            if (name.IsEmpty())
                return "";

            if (value == "" && name != nameof(value) && !name.StartsWith("data"))
                return "";

            if (name == "maxlength" && (value == "" || value == "0"))
                return "";

            if (name == "checked" || name == "disabled" || name == "multiple")
            {
                if (value == "" || string.Compare(value, "false", true) == 0)
                    return "";
                value = string.Compare(value, "true", true) == 0 ? name : value;
            }

            if (name.StartsWith("data"))
                name = name.Insert(4, "-");

            return string.Format(" {0}=\"{1}\"", name, htmlEncode ? HttpUtility.HtmlEncode(value) : value);
        }

        /// <summary>Appends grow and uses delimiter if the string is not empty.</summary>
        [DebuggerStepThrough]
        public static string Grow(this string value, string grow, string delimiter)
        {
            if (string.IsNullOrEmpty(value))
                return string.IsNullOrEmpty(grow) ? "" : grow;

            if (string.IsNullOrEmpty(grow))
                return string.IsNullOrEmpty(value) ? "" : value;

            return string.Format("{0}{1}{2}", value, delimiter, grow);
        }

        /// <summary>Returns n/a if string is empty else self.</summary>
        [DebuggerStepThrough]
        public static string NaIfEmpty(this string value)
        {
            return value.HasValue() ? value : "n/a";
        }

        /// <summary>Replaces substring with position x1 to x2 by replaceBy.</summary>
        [DebuggerStepThrough]
        public static string Replace(this string value, int x1, int x2, string replaceBy = null)
        {
            if (value.HasValue() && x1 > 0 && x2 > x1 && x2 < value.Length)
                return value.Substring(0, x1) + (replaceBy == null ? "" : replaceBy) + value.Substring(x2 + 1);
            return value;
        }


        [DebuggerStepThrough]
        public static string TrimSafe(this string value)
        {
            return value.HasValue() ? value.Trim() : value;
        }


        [DebuggerStepThrough]
        public static bool IsMatch(this string input, string pattern,
            RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Multiline)
        {
            return Regex.IsMatch(input, pattern, options);
        }

        [DebuggerStepThrough]
        public static bool IsMatch(this string input, string pattern, out Match match,
            RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Multiline)
        {
            match = Regex.Match(input, pattern, options);
            return match.Success;
        }

        public static string RegexRemove(this string input, string pattern,
            RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Multiline)
        {
            return Regex.Replace(input, pattern, string.Empty, options);
        }

        public static string RegexReplace(this string input, string pattern, string replacement,
            RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Multiline)
        {
            return Regex.Replace(input, pattern, replacement, options);
        }

        [DebuggerStepThrough]
        public static string ToValidFileName(this string input, string replacement = "-")
        {
            return input.ToValidPathInternal(false, replacement);
        }

        [DebuggerStepThrough]
        public static string ToValidPath(this string input, string replacement = "-")
        {
            return input.ToValidPathInternal(true, replacement);
        }

        private static string ToValidPathInternal(this string input, bool isPath, string replacement)
        {
            var result = input.ToSafe();

            var invalidChars = isPath ? Path.GetInvalidPathChars() : Path.GetInvalidFileNameChars();

            foreach (var c in invalidChars)
                result = result.Replace(c.ToString(), replacement ?? "-");

            return result;
        }

        [DebuggerStepThrough]
        public static int[] ToIntArray(this string s)
        {
            return Array.ConvertAll(s.SplitSafe(","), v => int.Parse(v.Trim()));
        }

        [DebuggerStepThrough]
        public static bool ToIntArrayContains(this string s, int value, bool defaultValue)
        {
            if (s == null)
                return defaultValue;
            var arr = s.ToIntArray();
            if (arr == null || arr.Count() <= 0)
                return defaultValue;
            return arr.Contains(value);
        }

        [DebuggerStepThrough]
        public static string RemoveInvalidXmlChars(this string s)
        {
            if (s.IsEmpty())
                return s;

            return Regex.Replace(s, @"[^\u0009\u000A\u000D\u0020-\uD7FF\uE000-\uFFFD]", "", RegexOptions.Compiled);
        }

        [DebuggerStepThrough]
        public static string ReplaceCsvChars(this string s)
        {
            if (s.IsEmpty())
                return "";

            s = s.Replace(';', ',');
            s = s.Replace('\r', ' ');
            s = s.Replace('\n', ' ');
            return s.Replace("'", "");
        }

        #endregion

    }
}