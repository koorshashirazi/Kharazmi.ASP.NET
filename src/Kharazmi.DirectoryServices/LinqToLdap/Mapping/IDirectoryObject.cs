﻿using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using Kharazmi.DirectoryServices.Ldap.LinqToLdap.Collections;

namespace Kharazmi.DirectoryServices.Ldap.LinqToLdap.Mapping
{
    /// <summary>
    ///     Interface for updatable directory objects.
    /// </summary>
    public interface IDirectoryObject
    {
        /// <summary>
        ///     The original property values loaded form the directory for this object.
        /// </summary>
        OriginalValuesCollection OriginalValues { get; set; }

        /// <summary>
        ///     Gets the changes to send to the directory.
        /// </summary>
        /// <param name="mapping">The mapping for the object.</param>
        /// <returns></returns>
        IEnumerable<DirectoryAttributeModification> GetChanges(IObjectMapping mapping);
    }
}