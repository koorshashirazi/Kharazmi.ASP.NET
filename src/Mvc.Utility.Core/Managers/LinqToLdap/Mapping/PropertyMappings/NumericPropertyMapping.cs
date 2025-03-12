﻿using System;
using System.DirectoryServices.Protocols;

namespace Mvc.Utility.Core.Managers.LinqToLdap.Mapping.PropertyMappings
{
    internal class NumericPropertyMapping<T> : PropertyMappingGeneric<T> where T : class
    {
        public NumericPropertyMapping(PropertyMappingArguments<T> arguments) : base(arguments)
        {
        }

        public override string FormatValueToFilter(object value)
        {
            return value.ToString();
        }

        public override DirectoryAttributeModification GetDirectoryAttributeModification(object instance)
        {
            var modification = new DirectoryAttributeModification
            {
                Name = AttributeName,
                Operation = DirectoryAttributeOperation.Replace
            };
            var value = (string) GetValueForDirectory(instance);

            if (value != null) modification.Add(value);

            return modification;
        }

        public override object GetValueForDirectory(object instance)
        {
            var value = GetValue(instance);

            return value == null ? value : value.ToString();
        }

        public override object FormatValueFromDirectory(DirectoryAttribute value, string dn)
        {
            if (value != null && value.Count > 0)
            {
                var num = value[0];
                try
                {
                    return Convert.ChangeType(num, UnderlyingType);
                }
                catch (Exception ex)
                {
                    ThrowMappingException(num, dn, ex);
                }
            }

            AssertNullable(dn);

            return null;
        }
    }
}