﻿using System;
using System.Collections.Generic;
using Mvc.Utility.Core.Managers.LinqToLdap.Exceptions;
using Mvc.Utility.Core.Managers.LinqToLdap.Mapping.PropertyMappings;

namespace Mvc.Utility.Core.Managers.LinqToLdap.Mapping
{
    internal abstract class PropertyMappingGeneric<T> : PropertyMapping where T : class
    {
        protected PropertyMappingGeneric(PropertyMappingArguments<T> arguments)
            : base(arguments.PropertyType, arguments.PropertyName, arguments.AttributeName, arguments.IsStoreGenerated,
                arguments.IsDistinguishedName, arguments.IsReadOnly)
        {
            Getter = arguments.Getter;
            Setter = arguments.Setter;

            DirectoryValueMappings = arguments.DirectoryMappings;
            InstanceValueMappings = arguments.InstanceMappings;
        }

        protected Dictionary<string, object> DirectoryValueMappings { get; }
        protected Dictionary<object, string> InstanceValueMappings { get; }
        public Action<T, object> Setter { get; }
        public Func<T, object> Getter { get; }

        public override object GetValue(object instance)
        {
            if (Getter == null)
                throw new NotSupportedException(
                    string.Format("No getter has been defined for {0}.", typeof(T).FullName));

            return Getter(instance as T);
        }

        public override void SetValue(object instance, object value)
        {
            if (Setter == null)
                throw new NotSupportedException(
                    string.Format("No setter has been defined for {0}.", typeof(T).FullName));

            Setter(instance as T, value);
        }

        protected void ThrowMappingException(object value, string dn, Exception ex = null)
        {
            throw new MappingException(
                string.Format(
                    "Value '{0}' returned from directory cannot be converted to {1} for '{2}' on '{3}' - '{4}'",
                    value, PropertyType.FullName, PropertyName, typeof(T).FullName, dn), ex);
        }

        protected void ThrowMappingException(string dn, Exception ex = null)
        {
            throw new MappingException(
                string.Format("Value returned from directory cannot be converted to {0} for '{1}' on '{2}' - '{3}'",
                    PropertyType.FullName, PropertyName, typeof(T).FullName, dn), ex);
        }

        protected void AssertNullable(string dn)
        {
            if (!IsNullable)
                throw new MappingException(
                    string.Format(
                        "Null or empty returned from directory for non nullable type '{0}' for '{1}' on '{2}' - '{3}'",
                        PropertyType.FullName, PropertyName, typeof(T).FullName, dn));
        }
    }
}