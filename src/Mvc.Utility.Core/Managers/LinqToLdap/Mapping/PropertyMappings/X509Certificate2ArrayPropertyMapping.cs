﻿using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Mvc.Utility.Core.Managers.LinqToLdap.Mapping.PropertyMappings
{
    internal class X509Certificate2ArrayPropertyMapping<T> : PropertyMappingGeneric<T> where T : class
    {
        private readonly bool _isX5092;

        public X509Certificate2ArrayPropertyMapping(PropertyMappingArguments<T> arguments)
            : base(arguments)
        {
            _isX5092 = PropertyType.GetElementType() == typeof(X509Certificate2);
        }

        public override string FormatValueToFilter(object value)
        {
            if (!(value is X509Certificate))
                throw new NotSupportedException("X509Certificate[] cannot be used in filters.");

            return (value as X509Certificate).GetRawCertData().ToStringOctet();
        }

        public override DirectoryAttributeModification GetDirectoryAttributeModification(object instance)
        {
            var modification = new DirectoryAttributeModification
            {
                Name = AttributeName,
                Operation = DirectoryAttributeOperation.Replace
            };
            var value = (IEnumerable<X509Certificate>) GetValueForDirectory(instance);

            if (value != null)
                foreach (var c in value)
                    modification.Add(c.GetRawCertData());

            return modification;
        }

        public override object GetValueForDirectory(object instance)
        {
            var value = GetValue(instance);
            return value;
        }

        public override object FormatValueFromDirectory(DirectoryAttribute value, string dn)
        {
            if (value != null)
            {
                if (_isX5092)
                {
                    var certs = Array.ConvertAll(value.GetValues(typeof(byte[])),
                        obj =>
                            new X509Certificate2((byte[]) obj));
                    return certs;
                }
                else
                {
                    var certs = Array.ConvertAll(value.GetValues(typeof(byte[])),
                        obj =>
                            new X509Certificate((byte[]) obj));
                    return certs;
                }
            }

            AssertNullable(dn);

            return null;
        }

        public override bool IsEqual(object instance, object value, out DirectoryAttributeModification modification)
        {
            var propertyValue = GetValue(instance) as X509Certificate[];
            var certificates = value as X509Certificate[];
            if ((propertyValue == null || propertyValue.Length == 0) &&
                (certificates == null || certificates.Length == 0))
            {
                modification = null;
                return true;
            }

            if (propertyValue == null || certificates == null || certificates.Length != propertyValue.Length)
            {
                modification = GetDirectoryAttributeModification(instance);
                return false;
            }

            if (!certificates.SequenceEqual(propertyValue))
            {
                modification = GetDirectoryAttributeModification(instance);
                return false;
            }

            modification = null;
            return true;
        }
    }
}