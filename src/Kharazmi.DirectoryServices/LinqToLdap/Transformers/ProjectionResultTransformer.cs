using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using Kharazmi.DirectoryServices.Ldap.LinqToLdap.Mapping;

namespace Kharazmi.DirectoryServices.Ldap.LinqToLdap.Transformers
{
    internal class ProjectionResultTransformer : ResultTransformer
    {
        private readonly SelectProjection _selectProjection;

        public ProjectionResultTransformer(SelectProjection selectProjection,
            IDictionary<string, string> queriedProperties, IObjectMapping mapping)
            : base(queriedProperties, mapping,
                queriedProperties.Count == mapping.Properties.Count && selectProjection.ReturnType == mapping.Type)
        {
            _selectProjection = selectProjection;
        }

        public override object Transform(SearchResultEntry entry)
        {
            var transformed = _selectProjection.Projection.DynamicInvoke(base.Transform(entry));

            return transformed;
        }

        public override object Default()
        {
            var type = _selectProjection.ReturnType;
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}