using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using Kharazmi.DirectoryServices.Ldap.LinqToLdap.Collections;

namespace Kharazmi.DirectoryServices.Ldap.LinqToLdap.Mapping
{
    /// <summary>
    ///     Base class for updatable directory objects.
    /// </summary>
    public abstract class DirectoryObjectBase : IDirectoryObject
    {
        /// <summary>
        ///     Gets the changes to send to the directory.
        /// </summary>
        /// <param name="mapping">The mapping for the object.</param>
        /// <returns></returns>
        public IEnumerable<DirectoryAttributeModification> GetChanges(IObjectMapping mapping)
        {
            return GetChanges(this, mapping, OriginalValues);
        }

        /// <summary>
        ///     The original property values loaded form the directory for this object.
        /// </summary>
        public OriginalValuesCollection OriginalValues { get; set; }

        /// <summary>
        ///     Method for identifying changes.  Simplifies implementing <see cref="IDirectoryObject" /> manually.
        /// </summary>
        public static IEnumerable<DirectoryAttributeModification> GetChanges(object instance, IObjectMapping mapping,
            OriginalValuesCollection collection)
        {
            if (collection == null || collection.Count == 0)
                foreach (var property in mapping.GetUpdateablePropertyMappings())
                    yield return property.GetDirectoryAttributeModification(instance);
            else
                foreach (var updateablePropertyMapping in mapping.GetUpdateablePropertyMappings())
                {
                    var propertyName = updateablePropertyMapping.PropertyName;
                    var value = collection[propertyName];
                    DirectoryAttributeModification modification;
                    if (!updateablePropertyMapping.IsEqual(instance, value, out modification))
                        yield return modification;
                }
        }
    }
}