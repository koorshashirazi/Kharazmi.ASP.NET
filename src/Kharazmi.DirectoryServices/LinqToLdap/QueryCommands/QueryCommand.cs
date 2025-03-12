﻿using System;
using System.DirectoryServices.Protocols;
using System.Linq;
using Kharazmi.DirectoryServices.Ldap.LinqToLdap.Logging;
using Kharazmi.DirectoryServices.Ldap.LinqToLdap.Mapping;
using Kharazmi.DirectoryServices.Ldap.LinqToLdap.QueryCommands.Options;

namespace Kharazmi.DirectoryServices.Ldap.LinqToLdap.QueryCommands
{
    internal abstract class QueryCommand : IQueryCommand
    {
        protected readonly IObjectMapping Mapping;
        protected readonly IQueryCommandOptions Options;
        protected readonly SearchRequest SearchRequest;

        protected QueryCommand(IQueryCommandOptions options, IObjectMapping mapping, bool initializeAttributes)
        {
            Options = options;
            Mapping = mapping;
            SearchRequest = new SearchRequest {Filter = options.Filter};
            if (Options.Controls != null) SearchRequest.Controls.AddRange(Options.Controls.ToArray());
            if (initializeAttributes) InitializeAttributes();
        }

        public abstract object Execute(DirectoryConnection connection, SearchScope scope, int maxPageSize,
            bool pagingEnabled, ILinqToLdapLogger log = null, string namingContext = null);

        private void InitializeAttributes()
        {
            if (!Mapping.HasCatchAllMapping)
            {
                var attributes = Mapping.HasSubTypeMappings
                    ? Options.AttributesToLoad.Values
                        .Union(new[] {"objectClass"}, StringComparer.OrdinalIgnoreCase)
                        .ToArray()
                    : Options.AttributesToLoad.Values.ToArray();
                SearchRequest.Attributes.AddRange(attributes);
            }
        }

        protected virtual T GetControl<T>(DirectoryControl[] controls) where T : class
        {
            if (controls == null || controls.Length == 0) return default;

            return controls.FirstOrDefault(c => c is T) as T;
        }

        protected virtual T GetControl<T>(DirectoryControlCollection controls) where T : class
        {
            if (controls == null || controls.Count == 0) return default;

            return controls.OfType<T>().FirstOrDefault();
        }

        protected void SetDistinguishedName(string namingContext)
        {
            SearchRequest.DistinguishedName = namingContext ?? Mapping.NamingContext;
        }

        public override string ToString()
        {
            return SearchRequest.ToLogString();
        }
    }
}