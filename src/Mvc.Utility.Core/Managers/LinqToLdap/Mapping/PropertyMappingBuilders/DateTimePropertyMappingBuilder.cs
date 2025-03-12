﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Mvc.Utility.Core.Managers.LinqToLdap.Collections;
using Mvc.Utility.Core.Managers.LinqToLdap.Helpers;
using Mvc.Utility.Core.Managers.LinqToLdap.Mapping.PropertyMappings;

namespace Mvc.Utility.Core.Managers.LinqToLdap.Mapping.PropertyMappingBuilders
{
    internal class DateTimePropertyMappingBuilder<T, TProperty> : IDateTimePropertyMappingBuilder<T, TProperty>,
        IPropertyMappingBuilder where T : class
    {
        private Dictionary<string, object> _directoryMappings;
        private string _directoryValue;
        private Dictionary<object, string> _instanceMappings;
        private object _instanceValue;

        public DateTimePropertyMappingBuilder(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null) throw new ArgumentNullException("propertyInfo");
            PropertyInfo = propertyInfo;

            IsDistinguishedName = false;
        }

        public string DateTimeFormat { get; private set; } = "yyyyMMddHHmmss.0Z";

        IDateTimePropertyMappingBuilder<T, TProperty> IDateTimePropertyMappingBuilder<T, TProperty>.Named(
            string attributeName)
        {
            AttributeName = attributeName.IsNullOrEmpty() ? null : attributeName;
            return this;
        }

        IDateTimePropertyMappingBuilder<T, TProperty> IDateTimePropertyMappingBuilder<T, TProperty>.DateTimeFormat(
            string format)
        {
            DateTimeFormat = format;
            return this;
        }

        IDateTimePropertyMappingBuilder<T, TProperty> IDateTimePropertyMappingBuilder<T, TProperty>.StoreGenerated()
        {
            IsStoreGenerated = true;
            return this;
        }

        IDateTimePropertyMappingBuilder<T, TProperty> IDateTimePropertyMappingBuilder<T, TProperty>.ReadOnly()
        {
            IsReadOnly = true;
            return this;
        }

        IDirectoryToConversion<IDateTimePropertyMappingBuilder<T, TProperty>, TProperty>
            IDirectoryValueConversionMapper<IDateTimePropertyMappingBuilder<T, TProperty>, TProperty>.DirectoryValue(
                string directoryValue)
        {
            if (string.IsNullOrEmpty(directoryValue))
                throw new ArgumentException(
                    "directoryValue cannot be null or empty.  Use DirectoryValueNotSetOrEmpty.");

            if (_directoryMappings == null)
                _directoryMappings = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            if (_directoryMappings.ContainsKey(directoryValue))
                throw new ArgumentException(directoryValue + " has already been mapped to a return value.");

            _directoryValue = directoryValue;
            return this;
        }

        IDirectoryToConversion<IDateTimePropertyMappingBuilder<T, TProperty>, TProperty>
            IDirectoryValueConversionMapper<IDateTimePropertyMappingBuilder<T, TProperty>, TProperty>.
            DirectoryValueNotSetOrEmpty()
        {
            if (_directoryMappings.ContainsKey(string.Empty))
                throw new ArgumentException("A not set or empty value has already been mapped to a return value.");

            _directoryValue = string.Empty;
            return this;
        }

        IDateTimePropertyMappingBuilder<T, TProperty>
            IDirectoryToConversion<IDateTimePropertyMappingBuilder<T, TProperty>, TProperty>.Returns(TProperty value)
        {
            _directoryMappings.Add(_directoryValue, value);
            _directoryValue = null;
            return this;
        }

        IInstanceToConversion<IDateTimePropertyMappingBuilder<T, TProperty>>
            IInstanceValueConversionMapper<IDateTimePropertyMappingBuilder<T, TProperty>, TProperty>.InstanceValue(
                TProperty instacneValue)
        {
            if (Equals(instacneValue, null))
                throw new ArgumentException("instacneValue cannot be null.  Use DirectoryValueNotSetOrEmpty.");

            if (_instanceMappings == null) _instanceMappings = new Dictionary<object, string>();

            if (_instanceMappings.ContainsKey(instacneValue))
                throw new ArgumentException(instacneValue + " has already been mapped to a return value.");

            _instanceValue = instacneValue;
            return this;
        }

        IInstanceToConversion<IDateTimePropertyMappingBuilder<T, TProperty>>
            IInstanceValueConversionMapper<IDateTimePropertyMappingBuilder<T, TProperty>, TProperty>.
            InstanceValueNullOrDefault()
        {
            if (_instanceMappings.ContainsKey(Nothing.Value))
                throw new ArgumentException("A null or default value has already been mapped to a return value.");

            _instanceValue = Nothing.Value;
            return this;
        }

        IDateTimePropertyMappingBuilder<T, TProperty>
            IInstanceToConversion<IDateTimePropertyMappingBuilder<T, TProperty>>.Sends(string value)
        {
            _instanceMappings.Add(_instanceValue, value);
            _instanceValue = null;
            return this;
        }

        public bool IsReadOnly { get; private set; }
        public bool IsDistinguishedName { get; }
        public bool IsStoreGenerated { get; private set; }
        public string AttributeName { get; private set; }
        public PropertyInfo PropertyInfo { get; }

        public IPropertyMapping ToPropertyMapping()
        {
            var type = typeof(T);
            var arguments = new PropertyMappingArguments<T>
            {
                PropertyName = PropertyInfo.Name,
                PropertyType = PropertyInfo.PropertyType,
                AttributeName = AttributeName ?? PropertyInfo.Name.Replace('_', '-'),
                Getter = DelegateBuilder.BuildGetter<T>(PropertyInfo),
                Setter = !type.IsAnonymous()
                    ? DelegateBuilder.BuildSetter<T>(PropertyInfo)
                    : null,
                IsStoreGenerated = IsStoreGenerated,
                IsDistinguishedName = IsDistinguishedName,
                IsReadOnly = IsReadOnly,
                DirectoryMappings = _directoryMappings,
                InstanceMappings = _instanceMappings
            };

            var mapping = new DatePropertyMapping<T>(arguments, DateTimeFormat);

            return mapping;
        }

        public string PropertyName => PropertyInfo.Name;
    }
}