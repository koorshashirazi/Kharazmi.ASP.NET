using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mvc.Utility.Core.Managers.JsonManager
{
    public class JsonConfigurationFileParser
    {
        private readonly Stack<string> _context = new Stack<string>();

        private readonly IDictionary<string, string> _data =
            new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        private string _currentPath;
        private JsonTextReader _reader;

        public IDictionary<string, string> Parse(Stream input)
        {
            _data.Clear();
            _reader = new JsonTextReader(new StreamReader(input))
            {
                DateParseHandling = DateParseHandling.None
            };
            VisitJObject(JObject.Load(_reader));
            return _data;
        }

        private void VisitJObject(JObject jObject)
        {
            foreach (var property in jObject.Properties())
            {
                EnterContext(property.Name);
                VisitProperty(property);
                ExitContext();
            }
        }

        private void VisitProperty(JProperty property)
        {
            VisitToken(property.Value);
        }

        private void VisitToken(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    VisitJObject(token.Value<JObject>());
                    break;

                case JTokenType.Array:
                    VisitArray(token.Value<JArray>());
                    break;

                case JTokenType.Integer:
                case JTokenType.Float:
                case JTokenType.String:
                case JTokenType.Boolean:
                case JTokenType.Null:
                case JTokenType.Raw:
                case JTokenType.Bytes:
                    VisitPrimitive(token);
                    break;

                default:
                    throw new FormatException(Resources.FormatError_UnsupportedJSONToken(_reader.TokenType,
                        _reader.Path, _reader.LineNumber, _reader.LinePosition));
            }
        }

        private void VisitArray(JArray array)
        {
            for (var index = 0; index < array.Count; ++index)
            {
                EnterContext(index.ToString());
                VisitToken(array[index]);
                ExitContext();
            }
        }

        private void VisitPrimitive(JToken data)
        {
            var currentPath = _currentPath;
            if (_data.ContainsKey(currentPath))
                throw new FormatException(Resources.FormatError_KeyIsDuplicated(currentPath));

            _data[currentPath] = data.ToString();
        }

        private void EnterContext(string context)
        {
            _context.Push(context);
            _currentPath = ConfigurationPath.Combine(_context.Reverse());
        }

        private void ExitContext()
        {
            _context.Pop();
            _currentPath = ConfigurationPath.Combine(_context.Reverse());
        }
    }
}