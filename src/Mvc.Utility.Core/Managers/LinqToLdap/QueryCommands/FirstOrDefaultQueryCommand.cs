﻿using System;
using System.DirectoryServices.Protocols;
using Mvc.Utility.Core.Managers.LinqToLdap.Logging;
using Mvc.Utility.Core.Managers.LinqToLdap.Mapping;
using Mvc.Utility.Core.Managers.LinqToLdap.QueryCommands.Options;

namespace Mvc.Utility.Core.Managers.LinqToLdap.QueryCommands
{
    internal class FirstOrDefaultQueryCommand : QueryCommand
    {
        public FirstOrDefaultQueryCommand(IQueryCommandOptions options, IObjectMapping mapping)
            : base(options, mapping, true)
        {
        }

        public override object Execute(DirectoryConnection connection, SearchScope scope, int maxPageSize,
            bool pagingEnabled, ILinqToLdapLogger log = null, string namingContext = null)
        {
            if (Options.YieldNoResults) return Options.GetTransformer().Default();

            SetDistinguishedName(namingContext);
            SearchRequest.Scope = scope;
            if (Options.SortingOptions != null)
            {
                if (GetControl<SortRequestControl>(SearchRequest.Controls) != null)
                    throw new InvalidOperationException("Only one sort request control can be sent to the server");

                SearchRequest.Controls.Add(new SortRequestControl(Options.SortingOptions.Keys) {IsCritical = false});
            }

            if (GetControl<PageResultRequestControl>(SearchRequest.Controls) != null)
                throw new InvalidOperationException("Only one page request control can be sent to the server.");
            if (pagingEnabled && !Options.WithoutPaging) SearchRequest.Controls.Add(new PageResultRequestControl(1));

            if (log != null && log.TraceEnabled) log.Trace(SearchRequest.ToLogString());
            var response = connection.SendRequest(SearchRequest) as SearchResponse;

            response.AssertSuccess();

            return response.Entries.Count > 0
                ? Options.GetTransformer().Transform(response.Entries[0])
                : Options.GetTransformer().Default();
        }
    }
}