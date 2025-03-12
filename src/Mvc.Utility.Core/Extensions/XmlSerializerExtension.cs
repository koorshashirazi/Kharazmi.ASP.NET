using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Mvc.Utility.Core.Helpers;

namespace Mvc.Utility.Core.Extensions
{
    public static partial class Common
    {
        public static Dictionary<string, string> XmlToDictionary(this XElement baseElm, string key, string value)
        {
            var dict = new Dictionary<string, string>();

            foreach (var elm in baseElm.Elements())
            {
                var dictKey = elm.Attribute(key)?.Value;
                var dictVal = elm.Attribute(value)?.Value;

                dict.Add(
                    dictKey ?? throw ExceptionHelper.ThrowException<NullReferenceException>(
                        ShareResources.NullReferenceException, key), dictVal);
            }

            return dict;
        }

        public static XElement DictionaryToXml(this Dictionary<string, string> inputDict, string elmName,
            string valuesName)
        {
            var outElm = new XElement(elmName);

            var keys = inputDict.Keys;

            var inner = new XElement(valuesName);

            foreach (var key in keys)
            {
                inner.Add(new XAttribute("key", key));
                inner.Add(new XAttribute("value", inputDict[key]));
            }

            outElm.Add(inner);

            return outElm;
        }
    }
}