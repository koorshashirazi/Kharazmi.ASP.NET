using System;
using System.DirectoryServices.Protocols;
using Kharazmi.DirectoryServices.Ldap.LinqToLdap.Logging;
using Kharazmi.DirectoryServices.Ldap.LinqToLdap.Mapping;
using Kharazmi.DirectoryServices.Ldap.LinqToLdap.QueryCommands.Options;

namespace Kharazmi.DirectoryServices.Ldap.LinqToLdap.QueryCommands
{
    internal class AnyQueryCommand : QueryCommand
    {
        public AnyQueryCommand(IQueryCommandOptions options, IObjectMapping mapping)
            : base(options, mapping, false)
        {
        }

        public override object Execute(DirectoryConnection connection, SearchScope scope, int maxPageSize,
            bool pagingEnabled, ILinqToLdapLogger log = null, string namingContext = null)
        {
            if (Options.YieldNoResults) return false;

            SetDistinguishedName(namingContext);
            SearchRequest.Scope = scope;
            if (GetControl<PageResultRequestControl>(SearchRequest.Controls) != null)
                throw new InvalidOperationException("Only one page request control can be sent to the server.");
            if (pagingEnabled && !Options.WithoutPaging) SearchRequest.Controls.Add(new PageResultRequestControl(1));

            SearchRequest.TypesOnly = true;
            SearchRequest.Attributes.Add("cn");

            if (log != null && log.TraceEnabled) log.Trace(SearchRequest.ToLogString());
            var response = connection.SendRequest(SearchRequest) as SearchResponse;

            response.AssertSuccess();

            return response.Entries.Count > 0;
        }
    }
}