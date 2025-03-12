﻿using System;
using System.DirectoryServices.Protocols;
using System.Security.Cryptography.X509Certificates;

namespace Kharazmi.DirectoryServices.Ldap.LinqToLdap.Mapping.PropertyMappings
{
    internal class X509Certificate2PropertyMapping<T> : PropertyMappingGeneric<T> where T : class
    {
        public X509Certificate2PropertyMapping(PropertyMappingArguments<T> arguments) : base(arguments)
        {
        }

        public override string FormatValueToFilter(object value)
        {
            return value != null
                ? ((X509Certificate) value).GetRawCertData().ToStringOctet()
                : null;
        }

        public override DirectoryAttributeModification GetDirectoryAttributeModification(object instance)
        {
            var modification = new DirectoryAttributeModification
            {
                Name = AttributeName,
                Operation = DirectoryAttributeOperation.Replace
            };
            var value = (byte[]) GetValueForDirectory(instance);

            if (value != null) modification.Add(value);

            return modification;
        }

        public override object GetValueForDirectory(object instance)
        {
            var value = GetValue(instance);

            return value == null ? null : ((X509Certificate) value).GetRawCertData();
        }

        public override object FormatValueFromDirectory(DirectoryAttribute value, string dn)
        {
            if (value != null)
            {
                var bytes = value.GetValues(typeof(byte[]))[0] as byte[];
                if (bytes == null) ThrowMappingException(dn);

                try
                {
                    return new X509Certificate2(bytes);
                }
                catch (Exception ex)
                {
                    ThrowMappingException(value, dn, ex);
                }
            }

            AssertNullable(dn);

            return null;
        }
    }
}