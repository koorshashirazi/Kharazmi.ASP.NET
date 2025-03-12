﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Mvc.Utility.Core.Managers.LinqToLdap.Helpers;

namespace Mvc.Utility.Core.Managers.LinqToLdap.Mapping
{
    internal class StandardObjectMapping<T> : ObjectMapping where T : class
    {
        private readonly Ctor<T> _constructor;

#if NET35
        private readonly LinqToLdap.Collections.SafeDictionary<string, IObjectMapping> _fullObjectClassMappings =
 new LinqToLdap.Collections.SafeDictionary<string, IObjectMapping>();
#else
        private readonly ConcurrentDictionary<string, IObjectMapping> _fullObjectClassMappings =
            new ConcurrentDictionary<string, IObjectMapping>(StringComparer.OrdinalIgnoreCase);
#endif

        public StandardObjectMapping(string namingContext,
            IEnumerable<IPropertyMapping> propertyMappings, string objectCategory, bool includeObjectCategory,
            IEnumerable<string> objectClass, bool includeObjectClasses)
            : base(namingContext, propertyMappings, objectCategory, includeObjectCategory, objectClass,
                includeObjectClasses)
        {
            _constructor = DelegateBuilder.BuildCtor<T>();
        }

        public override bool IsForAnonymousType => false;
        public override Type Type => typeof(T);

        public override object Create(object[] parameters = null, object[] objectClasses = null)
        {
            if (HasSubTypeMappings && objectClasses != null)
            {
                IObjectMapping mapping;

#if NET35
                string joinedObjectClasses = string.Join(" ", objectClasses.Cast<string>().ToArray());
#else
                var joinedObjectClasses = string.Join(" ", objectClasses);
#endif

                if (_fullObjectClassMappings.TryGetValue(joinedObjectClasses, out mapping)) return mapping.Create();
                //Reverse the object classes in order of most specific to least specific. 
                //AD and openLDAP seem to retreive the classes in order of least to most specific so I assume it's the standard.
                if (objectClasses.Cast<string>().Reverse().Any(objectClass =>
                    SubTypeMappingsObjectClassDictionary.TryGetValue(objectClass, out mapping)))
                {
                    _fullObjectClassMappings.TryAdd(joinedObjectClasses, mapping);
                    return mapping.Create();
                }
            }

            return _constructor();
        }
    }
}