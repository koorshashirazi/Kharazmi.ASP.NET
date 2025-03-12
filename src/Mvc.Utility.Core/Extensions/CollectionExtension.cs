using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization.Formatters.Binary;

namespace Mvc.Utility.Core.Extensions
{
    public static partial class Common
    {

        public static bool IsNullOrEmpty<T>(this ICollection<T> source)
        {
            return source == null || !source.Any();
        }

        /// <summary>
        ///     Merges two dictionaries
        /// </summary>
        /// <param name="explicitValues">The explicit values.</param>
        /// <param name="additionalValues">The additional values.</param>
        /// <returns></returns>
        public static Dictionary<string, string> Merge(this Dictionary<string, string> explicitValues,
            Dictionary<string, string> additionalValues = null)
        {
            var merged = explicitValues;

            if (additionalValues != null)
                merged =
                    explicitValues.Concat(additionalValues.Where(add => !explicitValues.ContainsKey(add.Key)))
                        .ToDictionary(final => final.Key, final => final.Value);

            return merged;
        }

        /// <summary>
        ///     Merges the key/value pairs from d2 into d1, without overwriting those already set in d1.
        /// </summary>
        public static void Merge<TKey, TValue>(this IDictionary<TKey, TValue> d1, IDictionary<TKey, TValue> d2)
        {
            foreach (var kv in d2.Where(x => !d1.ContainsKey(x.Key)).ToList()) d1[kv.Key] = kv.Value;
        }

        public static Dictionary<string, string> ToDictionary(this object obj)
        {
            switch (obj)
            {
                case null:
                    return null;

                case Dictionary<string, string> dictionary1:
                    return dictionary1;
            }

            var dictionary2 = new Dictionary<string, string>();
            foreach (var runtimeProperty in obj.GetType().GetRuntimeProperties())
            {
                var str = runtimeProperty.GetValue(obj) as string;
                if (!string.IsNullOrEmpty(str)) dictionary2.Add(runtimeProperty.Name, str);
            }

            return dictionary2;
        }

        public static string DictionaryToBase64String(this IDictionary<string, object> values)
        {
            using (var memStream = new MemoryStream())
            {
                memStream.Seek(0, SeekOrigin.Begin);
                IRemotingFormatter formatter = new BinaryFormatter();
                formatter.Serialize(memStream, values);
                memStream.Seek(0, SeekOrigin.Begin);
                var bytes = memStream.ToArray();
                return Convert.ToBase64String(bytes);
            }
        }

        public static IEnumerable<Tuple<T, int>> WithIndex<T>(this IEnumerable<T> self)
        {
            return self.Select((item, index) => new Tuple<T, int>(item, index));
        }
    }
}