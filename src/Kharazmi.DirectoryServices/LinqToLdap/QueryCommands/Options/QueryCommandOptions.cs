using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using Kharazmi.DirectoryServices.Ldap.LinqToLdap.Collections;
using Kharazmi.DirectoryServices.Ldap.LinqToLdap.Transformers;

namespace Kharazmi.DirectoryServices.Ldap.LinqToLdap.QueryCommands.Options
{
    internal abstract class QueryCommandOptions : IQueryCommandOptions
    {
        protected QueryCommandOptions(SelectProjection selectProjection)
        {
            SelectProjection = selectProjection;
            AttributesToLoad = selectProjection.SelectedProperties;
        }

        protected QueryCommandOptions(IDictionary<string, string> queriedAttributes)
        {
            AttributesToLoad = queriedAttributes;
        }

        protected SelectProjection SelectProjection { get; }

        public IDictionary<string, string> AttributesToLoad { get; }

        public string Filter { get; set; }

        public bool IsLongCount { get; set; }

        public bool WithoutPaging { get; set; }

        public IPagingOptions PagingOptions { get; set; }

        public ISortingOptions SortingOptions { get; set; }

        public IEnumerable<DirectoryControl> Controls { get; set; }

        public int? PageSize { get; set; }

        public int? TakeSize { get; set; }

        public int? SkipSize { get; set; }

        public bool YieldNoResults { get; set; }

        public object GetEnumerator(SearchResultEntryCollection results)
        {
            return Activator.CreateInstance(
                typeof(SearchResponseEnumerable<>).MakeGenericType(GetEnumeratorReturnType()), results,
                GetTransformer());
        }

        public object GetEnumerator()
        {
            return Activator.CreateInstance(
                typeof(SearchResponseEnumerable<>).MakeGenericType(GetEnumeratorReturnType()), null, GetTransformer());
        }

        public object GetEnumerator(List<SearchResultEntry> results)
        {
            return Activator.CreateInstance(
                typeof(SearchResponseEnumerable<>).MakeGenericType(GetEnumeratorReturnType()), GetTransformer(),
                results);
        }

        public abstract Type GetEnumeratorReturnType();
        public abstract IResultTransformer GetTransformer();
    }
}