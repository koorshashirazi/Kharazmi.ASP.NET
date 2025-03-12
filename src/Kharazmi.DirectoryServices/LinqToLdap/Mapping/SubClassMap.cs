﻿using System;
using System.Linq;
using System.Linq.Expressions;
using Kharazmi.DirectoryServices.Ldap.LinqToLdap.Exceptions;
using Kharazmi.DirectoryServices.Ldap.LinqToLdap.Visitors;

namespace Kharazmi.DirectoryServices.Ldap.LinqToLdap.Mapping
{
    /// <summary>
    ///     Defines a mapping for a directory entry. Derive from this class to create a mapping,
    ///     and use the constructor to control how your object is queryed.
    /// </summary>
    /// <example>
    ///     public class UserMap : SubClassMap&lt;User&gt;
    ///     {
    ///     public UserMap() : base(new SuperUserMap())
    ///     {
    ///     Map(x => x.Name)
    ///     .Named("displayname");
    ///     Map(x => x.Age);
    ///     }
    ///     }
    /// </example>
    /// <typeparam name="T">Type to map</typeparam>
    /// <typeparam name="TSuper">Super type for <typeparamref name="T" />.</typeparam>
    public abstract class SubClassMap<T, TSuper> : ClassMap<T> where T : class, TSuper where TSuper : class
    {
        /// <summary>
        ///     Adds the properties from the parent mapping to the sub class.
        /// </summary>
        /// <param name="parentMapping"></param>
        protected SubClassMap(ClassMap<TSuper> parentMapping)
        {
            var mapping = (ClassMap<TSuper>) parentMapping.PerformMapping();
            foreach (var property in mapping.PropertyMappings) PropertyMappings.Add(property);
        }

        /// <summary>
        ///     Removes the mapping for a property.
        /// </summary>
        /// <typeparam name="TValue">The type of the property</typeparam>
        /// <param name="property">The property to remove</param>
        protected void Ignore<TValue>(Expression<Func<TSuper, TValue>> property)
        {
            if (property == null) throw new ArgumentNullException("property");

            var memberExpression = new MemberVisitor().GetMember(property.Body) as MemberExpression;

            if (memberExpression == null)
                throw new ArgumentException("Expected MemberAccess expression but was " + property.Body.NodeType);

            var mappedProperty = PropertyMappings.FirstOrDefault(p => p.PropertyName == memberExpression.Member.Name);
            if (mappedProperty == null)
                throw new MappingException(memberExpression.Member.Name + " has not been mapped.");

            PropertyMappings.Remove(mappedProperty);
        }
    }
}