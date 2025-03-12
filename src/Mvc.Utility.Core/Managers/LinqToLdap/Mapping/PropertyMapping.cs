using System;
using System.DirectoryServices.Protocols;

namespace Mvc.Utility.Core.Managers.LinqToLdap.Mapping
{
    internal abstract class PropertyMapping : IPropertyMapping
    {
        protected PropertyMapping(Type propertyType, string propertyName, string attributeName, bool isStoreGenerated,
            bool isDistinguishedName, bool isReadOnly)
        {
            IsDistinguishedName = isDistinguishedName;
            IsReadOnly = isReadOnly || isDistinguishedName;
            IsStoreGenerated = isStoreGenerated;
            PropertyType = propertyType;
            PropertyName = propertyName;
            AttributeName = attributeName;

            IsNullable = DetermineIfNullable();
            UnderlyingType = IsNullable && PropertyType.IsValueType
                ? Nullable.GetUnderlyingType(PropertyType)
                : PropertyType;

            DefaultValue = GetType()
                .GetMethod("GetDefault")
                .MakeGenericMethod(PropertyType).Invoke(this, null);
        }

        public bool IsNullable { get; }
        protected object DefaultValue { get; }
        public Type UnderlyingType { get; }

        public Type PropertyType { get; }
        public bool IsStoreGenerated { get; }
        public bool IsDistinguishedName { get; }
        public bool IsReadOnly { get; }

        public string PropertyName { get; }
        public string AttributeName { get; }

        public abstract object GetValue(object instance);

        public abstract void SetValue(object instance, object value);

        public virtual object Default()
        {
            return DefaultValue;
        }

        public abstract object FormatValueFromDirectory(DirectoryAttribute value, string dn);

        public abstract string FormatValueToFilter(object value);

        public abstract DirectoryAttributeModification GetDirectoryAttributeModification(object instance);

        public virtual DirectoryAttribute GetDirectoryAttribute(object instance)
        {
            return GetDirectoryAttributeModification(instance);
        }

        public virtual bool IsEqual(object instance, object value, out DirectoryAttributeModification modification)
        {
            if (!Equals(GetValue(instance), value))
            {
                modification = GetDirectoryAttributeModification(instance);
                return false;
            }

            modification = null;
            return true;
        }

        private bool DetermineIfNullable()
        {
            if (!PropertyType.IsValueType) return true;
            return Nullable.GetUnderlyingType(PropertyType) != null;
        }

        public abstract object GetValueForDirectory(object instance);

        public TType GetDefault<TType>()
        {
            return default;
        }
    }
}