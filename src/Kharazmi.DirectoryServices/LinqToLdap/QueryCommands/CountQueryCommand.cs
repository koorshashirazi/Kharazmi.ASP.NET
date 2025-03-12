using System.DirectoryServices.Protocols;
using Kharazmi.DirectoryServices.Ldap.LinqToLdap.Logging;
using Kharazmi.DirectoryServices.Ldap.LinqToLdap.Mapping;
using Kharazmi.DirectoryServices.Ldap.LinqToLdap.QueryCommands.Options;

namespace Kharazmi.DirectoryServices.Ldap.LinqToLdap.QueryCommands
{
    internal class CountQueryCommand : QueryCommand
    {
        public CountQueryCommand(IQueryCommandOptions options, IObjectMapping mapping)
            : base(options, mapping, false)
        {
        }

        public override object Execute(DirectoryConnection connection, SearchScope scope, int maxPageSize,
            bool pagingEnabled, ILinqToLdapLogger log = null, string namingContext = null)
        {
            if (Options.YieldNoResults) return 0;

            SetDistinguishedName(namingContext);
            SearchRequest.Scope = scope;

            var index = -1;
            if (pagingEnabled && !Options.WithoutPaging)
            {
                var pagedRequest = GetControl<PageResultRequestControl>(SearchRequest.Controls);
                if (pagedRequest != null) index = SearchRequest.Controls.IndexOf(pagedRequest);

                if (pagedRequest == null)
                {
                    pagedRequest = new PageResultRequestControl(maxPageSize);
                    index = SearchRequest.Controls.Add(pagedRequest);
                }
            }

            SearchRequest.TypesOnly = true;
            SearchRequest.Attributes.Add("distinguishedname");

            if (log != null && log.TraceEnabled) log.Trace(SearchRequest.ToLogString());
            var response = connection.SendRequest(SearchRequest) as SearchResponse;
            response.AssertSuccess();

            var count = response.Entries.Count;
            if (pagingEnabled && !Options.WithoutPaging)
            {
                var pageResultResponseControl = GetControl<PageResultResponseControl>(response.Controls);
                var hasResults = pageResultResponseControl != null && pageResultResponseControl.Cookie.Length > 0;
                while (hasResults)
                {
                    SearchRequest.Controls[index] = new PageResultRequestControl(pageResultResponseControl.Cookie);

                    if (log != null && log.TraceEnabled) log.Trace(SearchRequest.ToLogString());
                    response = connection.SendRequest(SearchRequest) as SearchResponse;
                    response.AssertSuccess();
                    pageResultResponseControl = GetControl<PageResultResponseControl>(response.Controls);
                    hasResults = pageResultResponseControl != null && pageResultResponseControl.Cookie.Length > 0;
                    count += response.Entries.Count;
                }
            }

            if (Options.IsLongCount) return (long) count;

            return count;
        }
    }
}