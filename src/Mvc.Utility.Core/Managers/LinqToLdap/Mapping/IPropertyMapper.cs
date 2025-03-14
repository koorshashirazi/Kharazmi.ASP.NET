﻿using System;

namespace Mvc.Utility.Core.Managers.LinqToLdap.Mapping
{
    /// <summary>
    ///     Interface for mapping properties
    /// </summary>
    public interface IPropertyMapperGeneric<in TProperty>
    {
        /// <summary>
        ///     Specify an attribute name for a mapped property.
        /// </summary>
        /// <param name="attributeName">Attribute name in the directory</param>
        /// <returns></returns>
        IPropertyMapperGeneric<TProperty> Named(string attributeName);

        /// <summary>
        ///     Specify the format of the DateTime in the directory.  Use null if the DateTime is stored in file time.
        ///     Default format is yyyyMMddHHmmss.0Z.  If the property is not a <see cref="DateTime" /> then this setting will be
        ///     ignored.
        /// </summary>
        /// <example>
        ///     <para>Common formats</para>
        ///     <para>
        ///         Active Directory: yyyyMMddHHmmss.0Z
        ///     </para>
        ///     <para>
        ///         eDirectory: yyyyMMddHHmmssZ or yyyyMMddHHmmss00Z
        ///     </para>
        /// </example>
        /// <param name="format">The format of the DateTime in the directory.</param>
        /// <returns></returns>
        IPropertyMapperGeneric<TProperty> DateTimeFormat(string format);

        /// <summary>
        ///     Speicfy that a <see cref="Enum" /> is stored as an int.  The default is string.
        ///     If the property is not a <see cref="Enum" /> then this setting will be ignored.
        /// </summary>
        /// <returns></returns>
        IPropertyMapperGeneric<TProperty> EnumStoredAsInt();

        /// <summary>
        ///     Indicates that the property is generated by the directory.
        /// </summary>
        /// <returns></returns>
        IPropertyMapperGeneric<TProperty> StoreGenerated();

        /// <summary>
        ///     Indicates if the property should never be considered for updating.
        /// </summary>
        /// <returns></returns>
        IPropertyMapperGeneric<TProperty> ReadOnly();
    }

    /// <summary>
    ///     Interface for mapping properties
    /// </summary>
    public interface IPropertyMapper
    {
        /// <summary>
        ///     Specify an attribute name for a mapped property.
        /// </summary>
        /// <param name="attributeName">Attribute name in the directory</param>
        /// <returns></returns>
        IPropertyMapper Named(string attributeName);

        /// <summary>
        ///     Specify the format of the DateTime in the directory.  Use null if the DateTime is stored in file time.
        ///     Default format is yyyyMMddHHmmss.0Z.  If the property is not a <see cref="DateTime" /> then this setting will be
        ///     ignored.
        /// </summary>
        /// <example>
        ///     <para>Common formats</para>
        ///     <para>
        ///         Active Directory: yyyyMMddHHmmss.0Z
        ///     </para>
        ///     <para>
        ///         eDirectory: yyyyMMddHHmmssZ or yyyyMMddHHmmss00Z
        ///     </para>
        /// </example>
        /// <param name="format">The format of the DateTime in the directory.</param>
        /// <returns></returns>
        IPropertyMapper DateTimeFormat(string format);

        /// <summary>
        ///     Specify that a <see cref="Enum" /> is stored as an int.  The default is string.
        ///     If the property is not a <see cref="Enum" /> then this setting will be ignored.
        /// </summary>
        /// <returns></returns>
        IPropertyMapper EnumStoredAsInt();

        /// <summary>
        ///     Indicates that the property is generated by the directory.
        /// </summary>
        /// <returns></returns>
        IPropertyMapper StoreGenerated();

        /// <summary>
        ///     Indicates if the property should never be considered for updating.
        /// </summary>
        /// <returns></returns>
        IPropertyMapper ReadOnly();
    }
}