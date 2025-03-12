using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using Mvc.Utility.Core.Managers.LinqToLdap.Collections;
using Mvc.Utility.Core.Managers.LinqToLdap.Mapping.PropertyMappingBuilders;

namespace Mvc.Utility.Core.Managers.LinqToLdap.Mapping
{
    public abstract partial class ClassMap<T>
    {
        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<bool> Map(Expression<Func<T, bool>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<bool>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<bool?> Map(Expression<Func<T, bool?>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<bool?>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<byte[][]> Map(Expression<Func<T, byte[][]>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<byte[][]>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<byte[]> Map(Expression<Func<T, byte[]>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<byte[]>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<ICollection<byte[]>> Map(Expression<Func<T, ICollection<byte[]>>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<ICollection<byte[]>>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<Collection<byte[]>> Map(Expression<Func<T, Collection<byte[]>>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<Collection<byte[]>>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IDateTimePropertyMappingBuilder<T, DateTime> Map(Expression<Func<T, DateTime>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map(new DateTimePropertyMappingBuilder<T, DateTime>(propertyInfo));
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IDateTimePropertyMappingBuilder<T, DateTime?> Map(Expression<Func<T, DateTime?>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map(new DateTimePropertyMappingBuilder<T, DateTime?>(propertyInfo));
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<Enum> Map(Expression<Func<T, Enum>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<Enum>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<TProperty> Map<TProperty>(Expression<Func<T, TProperty>> property)
            where TProperty : struct, IConvertible
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<TProperty>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<TProperty?> Map<TProperty>(Expression<Func<T, TProperty?>> property)
            where TProperty : struct, IConvertible
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<TProperty?>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<Guid> Map(Expression<Func<T, Guid>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<Guid>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<Guid?> Map(Expression<Func<T, Guid?>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<Guid?>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<short> Map(Expression<Func<T, short>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<short>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<short?> Map(Expression<Func<T, short?>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<short?>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<ushort> Map(Expression<Func<T, ushort>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<ushort>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<ushort?> Map(Expression<Func<T, ushort?>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<ushort?>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<int> Map(Expression<Func<T, int>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<int>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<int?> Map(Expression<Func<T, int?>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<int?>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<uint> Map(Expression<Func<T, uint>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<uint>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<uint?> Map(Expression<Func<T, uint?>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<uint?>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<long> Map(Expression<Func<T, long>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<long>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<long?> Map(Expression<Func<T, long?>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<long?>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<ulong> Map(Expression<Func<T, ulong>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<ulong>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<ulong?> Map(Expression<Func<T, ulong?>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<ulong?>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<byte> Map(Expression<Func<T, byte>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<byte>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<byte?> Map(Expression<Func<T, byte?>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<byte?>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<decimal> Map(Expression<Func<T, decimal>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<decimal>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<decimal?> Map(Expression<Func<T, decimal?>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<decimal?>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<double> Map(Expression<Func<T, double>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<double>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<double?> Map(Expression<Func<T, double?>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<double?>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<float> Map(Expression<Func<T, float>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<float>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<float?> Map(Expression<Func<T, float?>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<float?>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<sbyte> Map(Expression<Func<T, sbyte>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<sbyte>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<sbyte?> Map(Expression<Func<T, sbyte?>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<sbyte?>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<SecurityIdentifier> Map(Expression<Func<T, SecurityIdentifier>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<SecurityIdentifier>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<string[]> Map(Expression<Func<T, string[]>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<string[]>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<DateTime[]> Map(Expression<Func<T, DateTime[]>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<DateTime[]>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<DateTime?[]> Map(Expression<Func<T, DateTime?[]>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<DateTime?[]>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<Collection<string>> Map(Expression<Func<T, Collection<string>>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<Collection<string>>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<ICollection<string>> Map(Expression<Func<T, ICollection<string>>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<ICollection<string>>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<string> Map(Expression<Func<T, string>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<string>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<X509Certificate> Map(Expression<Func<T, X509Certificate>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<X509Certificate>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<X509Certificate2> Map(Expression<Func<T, X509Certificate2>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<X509Certificate2>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<X509Certificate[]> Map(Expression<Func<T, X509Certificate[]>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<X509Certificate[]>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<SecurityIdentifier[]> Map(Expression<Func<T, SecurityIdentifier[]>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<SecurityIdentifier[]>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<X509Certificate2[]> Map(Expression<Func<T, X509Certificate2[]>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<X509Certificate2[]>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<Collection<X509Certificate>> Map(
            Expression<Func<T, Collection<X509Certificate>>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<Collection<X509Certificate>>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<ICollection<X509Certificate>> Map(
            Expression<Func<T, ICollection<X509Certificate>>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<ICollection<X509Certificate>>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<Collection<X509Certificate2>> Map(
            Expression<Func<T, Collection<X509Certificate2>>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<Collection<X509Certificate2>>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected IPropertyMapperGeneric<ICollection<X509Certificate2>> Map(
            Expression<Func<T, ICollection<X509Certificate2>>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            return Map<ICollection<X509Certificate2>>(propertyInfo);
        }

        /// <summary>
        ///     Create a property mapping to catch all attributes.
        /// </summary>
        /// <param name="property">Property to map</param>
        /// <example>
        ///     Map(x => x.Name);
        /// </example>
        protected void CatchAll(Expression<Func<T, IDirectoryAttributes>> property)
        {
            var propertyInfo = GetPropertyInfo(property.Body);

            CatchAll(propertyInfo);
        }
    }
}