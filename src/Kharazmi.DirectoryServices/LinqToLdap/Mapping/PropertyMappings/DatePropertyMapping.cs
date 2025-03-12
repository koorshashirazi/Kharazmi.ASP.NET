﻿using System;
using System.DirectoryServices.Protocols;
using Kharazmi.DirectoryServices.Ldap.LinqToLdap.Collections;

namespace Kharazmi.DirectoryServices.Ldap.LinqToLdap.Mapping.PropertyMappings
{
    internal class DatePropertyMapping<T> : PropertyMappingGeneric<T> where T : class
    {
        private readonly string _dateFormat;
        private readonly bool _isFileTimeFormat;

        public DatePropertyMapping(PropertyMappingArguments<T> arguments, string dateFormat)
            : base(arguments)
        {
            _isFileTimeFormat = dateFormat == null;

            _dateFormat = dateFormat;
        }

        public override object FormatValueFromDirectory(DirectoryAttribute value, string dn)
        {
            string str;
            if (value != null && value.Count > 0 && (str = value[0] as string) != null && !str.IsNullOrEmpty())
                try
                {
                    if (DirectoryValueMappings != null && DirectoryValueMappings.ContainsKey(str))
                        return DirectoryValueMappings[str];
                    var dateTime = _isFileTimeFormat
                        ? DateTime.FromFileTime(long.Parse(str))
                        : str.FormatLdapDateTime(_dateFormat);

                    return dateTime;
                }
                catch (Exception ex)
                {
                    ThrowMappingException(value, dn, ex);
                }

            if (DirectoryValueMappings != null && DirectoryValueMappings.ContainsKey(string.Empty))
                return DirectoryValueMappings[string.Empty];

            AssertNullable(dn);

            return null;
        }

        public override string FormatValueToFilter(object value)
        {
            var date = (DateTime) value;

            return _isFileTimeFormat
                ? date.ToFileTime().ToString()
                : date.FormatLdapDateTime(_dateFormat);
        }

        public override DirectoryAttributeModification GetDirectoryAttributeModification(object instance)
        {
            var modification = new DirectoryAttributeModification
            {
                Name = AttributeName,
                Operation = DirectoryAttributeOperation.Replace
            };
            var value = (string) GetValueForDirectory(instance);

            if (!string.IsNullOrEmpty(value)) modification.Add(value);

            return modification;
        }

        public override object GetValueForDirectory(object instance)
        {
            var value = GetValue(instance);

            if (value == null)
                return InstanceValueMappings != null && InstanceValueMappings.ContainsKey(Nothing.Value)
                    ? InstanceValueMappings[Nothing.Value]
                    : value;

            if (InstanceValueMappings != null && InstanceValueMappings.ContainsKey(value))
                return InstanceValueMappings[value];

            return _isFileTimeFormat
                ? ((DateTime) value).ToFileTime().ToString()
                : ((DateTime) value).FormatLdapDateTime(_dateFormat);
        }
    }
}