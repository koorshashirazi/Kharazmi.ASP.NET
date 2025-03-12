using System;
using System.Collections.Generic;
using Kharazmi.DirectoryServices.Ldap.LinqToLdap.Mapping;
using Kharazmi.DirectoryServices.Ldap.LinqToLdap.Transformers;

namespace Kharazmi.DirectoryServices.Ldap.LinqToLdap.QueryCommands.Options
{
    internal class StandardQueryCommandOptions : QueryCommandOptions
    {
        private readonly IObjectMapping _mapping;

        public StandardQueryCommandOptions(IObjectMapping mapping, IDictionary<string, string> queriedAttributes)
            : base(queriedAttributes)
        {
            _mapping = mapping;
        }

        public override Type GetEnumeratorReturnType()
        {
            return _mapping.Type;
        }

        public override IResultTransformer GetTransformer()
        {
            return new ResultTransformer(AttributesToLoad, _mapping);
        }
    }
}