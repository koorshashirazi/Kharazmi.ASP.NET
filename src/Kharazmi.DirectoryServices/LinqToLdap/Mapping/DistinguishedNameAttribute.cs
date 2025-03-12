using System;

namespace Kharazmi.DirectoryServices.Ldap.LinqToLdap.Mapping
{
    /// <summary>
    ///     Indicates that the property will contain the distinguished name for a class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DistinguishedNameAttribute : Attribute
    {
        /// <summary>
        ///     Maps a property to the specified <paramref name="attributeName" />.
        /// </summary>
        /// <param name="attributeName">Name of the attribute.</param>
        public DistinguishedNameAttribute(string attributeName = "distinguishedname")
        {
            AttributeName = attributeName;
        }

        /// <summary>
        ///     The mapped attribute name.
        /// </summary>
        public string AttributeName { get; }
    }
}