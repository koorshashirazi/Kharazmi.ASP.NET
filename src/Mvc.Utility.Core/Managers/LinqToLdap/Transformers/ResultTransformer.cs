using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using Mvc.Utility.Core.Managers.LinqToLdap.Collections;
using Mvc.Utility.Core.Managers.LinqToLdap.Mapping;

namespace Mvc.Utility.Core.Managers.LinqToLdap.Transformers
{
    internal class ResultTransformer : IResultTransformer
    {
        private readonly bool _setOriginalValues;
        protected readonly IObjectMapping ObjectMapping;
        protected readonly IDictionary<string, string> QueriedProperties;

        public ResultTransformer(IDictionary<string, string> queriedProperties, IObjectMapping mapping,
            bool setOriginalValues = true)
        {
            QueriedProperties = queriedProperties;
            ObjectMapping = mapping;
            _setOriginalValues = setOriginalValues;
        }

        public virtual object Transform(SearchResultEntry entry)
        {
            object instance;
            if (!ObjectMapping.IsForAnonymousType)
            {
                instance = ObjectMapping.HasSubTypeMappings
                    ? ObjectMapping.Create(null, entry.Attributes["objectClass"]?.GetValues(typeof(string)))
                    : ObjectMapping.Create();
                var isDirectoryObject = instance is IDirectoryObject;
                var originalValues = isDirectoryObject && _setOriginalValues
                    ? new List<SerializableKeyValuePair<string, object>>(ObjectMapping.Properties.Count)
                    : null;

                var enumeration = ObjectMapping.HasCatchAllMapping
                    ? entry.Attributes.AttributeNames.Cast<string>().ToDictionary(x => x)
                    : QueriedProperties;

                var catchAll = ObjectMapping.GetCatchAllMapping();
                catchAll?.SetValue(instance, new DirectoryAttributes(entry));

                foreach (var queriedProperty in enumeration)
                {
                    var property = ObjectMapping.HasCatchAllMapping
                        ? ObjectMapping.GetPropertyMappingByAttribute(queriedProperty.Key, instance.GetType())
                        : ObjectMapping.GetPropertyMapping(queriedProperty.Key, instance.GetType());

                    if (property == null || property == catchAll) continue;

                    if (property.IsDistinguishedName)
                    {
                        var value = entry.DistinguishedName;
                        property.SetValue(instance, value);
                        originalValues?.Add(new SerializableKeyValuePair<string, object>(property.PropertyName, value));
                    }
                    else
                    {
                        if (entry.Attributes.Contains(ObjectMapping.HasCatchAllMapping
                            ? queriedProperty.Key
                            : queriedProperty.Value))
                        {
                            property.SetValue(instance, GetValue(entry, property));
                            originalValues?.Add(new SerializableKeyValuePair<string, object>(property.PropertyName,
                                GetValue(entry, property)));
                        }
                        else
                        {
                            originalValues?.Add(
                                new SerializableKeyValuePair<string, object>(property.PropertyName,
                                    property.Default()));
                        }
                    }
                }

                if (originalValues != null)
                    // ReSharper disable PossibleNullReferenceException
                    (instance as IDirectoryObject).OriginalValues = new OriginalValuesCollection(originalValues);
// ReSharper restore PossibleNullReferenceException
            }
            else
            {
                var parameters = new object[ObjectMapping.Properties.Count];
                var count = 0;
                foreach (var property in ObjectMapping.GetPropertyMappings())
                {
                    object value;
                    if (property.IsDistinguishedName)
                        value = entry.DistinguishedName;
                    else
                        value = entry.Attributes.Contains(property.AttributeName)
                            ? GetValue(entry, property)
                            : property.Default();

                    parameters[count++] = value;
                }

                instance = ObjectMapping.Create(parameters);
            }

            return instance;
        }

        public virtual object Default()
        {
            return null;
        }

        protected static object GetValue(SearchResultEntry entry, IPropertyMapping property)
        {
            var value = entry.Attributes[property.AttributeName];

            return property.FormatValueFromDirectory(value, entry.DistinguishedName);
        }
    }
}