using System;
using System.Globalization;
using Mvc.Utility.Core.Constraints;

namespace Mvc.Utility.Core.Extensions
{
    public static partial class Common
    {
        public static string ToStringDefaultDateFormat(this DateTime currentDateTime)
        {
            return currentDateTime.ToString("dd_MMM_yyyy");
        }

        public static string ToStringDefaultDateTimeFormat(this DateTime currentDateTime)
        {
            return currentDateTime.ToString("dd-MMM-yyyy HH:mm:ss");
        }

        public static string ToStringFormat(this DateTime currentDateTime, string formatter)
        {
            return currentDateTime.ToString(formatter);
        }

        public static string ToString(this TimeSpan timeSpan)
        {
            return timeSpan.ToString();
        }

        public static string ToString(this TimeSpan timeSpan, string format)
        {
            return timeSpan.ToString(format, CultureInfo.InvariantCulture);
        }

        public static TimeSpan ToTimeSpan(this string timeString)
        {
            if (string.IsNullOrWhiteSpace(timeString)) throw new ArgumentNullException(nameof(timeString));

            TimeSpan.TryParse(timeString, out var result);
            return result;
        }

        public static bool IsExpired(this string timeString)
        {
            if (!string.IsNullOrWhiteSpace(timeString)) return false;

            DateTime.TryParse(timeString, out var dateTime);
            return dateTime.AddSeconds(-60).ToUniversalTime() < DateTime.UtcNow;
        }

        public static string GetSeconds(this int value)
        {
            return TimeSpan.FromSeconds(value).ToString();
        }

        public static string GetMinutes(this int value)
        {
            return TimeSpan.FromMinutes(value).ToString();
        }

        public static string GetHours(this int value)
        {
            return TimeSpan.FromHours(value).ToString();
        }

        public static string GetDay(this int value)
        {
            return TimeSpan.FromDays(value).ToString();
        }

        public static DateTimeOffset TimeOffset(this int value, ExpiresTimeType expiresTimeType)
        {
            switch (expiresTimeType)
            {
                case ExpiresTimeType.Second:
                    return DateTimeOffset.UtcNow.AddSeconds(value);

                case ExpiresTimeType.Minutes:
                    return DateTimeOffset.UtcNow.AddMinutes(value);

                case ExpiresTimeType.Hours:
                    return DateTimeOffset.UtcNow.AddHours(value);

                case ExpiresTimeType.Day:
                    return DateTimeOffset.UtcNow.AddDays(value);

                case ExpiresTimeType.Months:
                    return DateTimeOffset.UtcNow.AddMonths(value);

                case ExpiresTimeType.Years:
                    return DateTimeOffset.UtcNow.AddYears(value);

                default:
                    return DateTimeOffset.UtcNow.AddSeconds(value);
            }
        }

        public static DateTime GetDateTime(this int value, ExpiresTimeType expiresTimeType)
        {
            switch (expiresTimeType)
            {
                case ExpiresTimeType.Second:
                    return DateTime.UtcNow.AddSeconds(value);

                case ExpiresTimeType.Minutes:
                    return DateTime.UtcNow.AddMinutes(value);

                case ExpiresTimeType.Hours:
                    return DateTime.UtcNow.AddHours(value);

                case ExpiresTimeType.Day:
                    return DateTime.UtcNow.AddDays(value);

                case ExpiresTimeType.Months:
                    return DateTime.UtcNow.AddMonths(value);

                case ExpiresTimeType.Years:
                    return DateTime.UtcNow.AddYears(value);

                default:
                    return DateTime.UtcNow.AddSeconds(value);
            }
        }

        /// <summary>
        ///     Converts the given date value to epoch time.
        /// </summary>
        public static long ToEpochTime(this DateTime dateTime)
        {
            var date = dateTime.ToUniversalTime();
            var ticks = date.Ticks - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).Ticks;
            var ts = ticks / TimeSpan.TicksPerSecond;
            return (int) ts;
        }

        /// <summary>
        ///     Converts the given epoch time to a <see cref="DateTime" /> with <see cref="DateTimeKind.Utc" /> kind.
        /// </summary>
        public static DateTime ToDateTimeFromEpoch(this long intDate)
        {
            var timeInTicks = intDate * TimeSpan.TicksPerSecond;
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddTicks(timeInTicks);
        }
    }
}