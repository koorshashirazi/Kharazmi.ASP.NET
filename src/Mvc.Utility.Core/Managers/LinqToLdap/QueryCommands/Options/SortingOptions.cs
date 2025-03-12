using System.Collections.Generic;
using System.DirectoryServices.Protocols;

namespace Mvc.Utility.Core.Managers.LinqToLdap.QueryCommands.Options
{
    internal class SortingOptions : ISortingOptions
    {
        private readonly List<SortKey> _sortKeys;
        private SortKey _sortKey;

        public SortingOptions()
        {
            _sortKeys = new List<SortKey>();
        }

        public SortKey[] Keys => _sortKeys.ToArray();

        public void AddSort(string attributeName, bool descending)
        {
            _sortKey = new SortKey {AttributeName = attributeName, ReverseOrder = descending};
            _sortKeys.Add(_sortKey);
        }

        public void SetMatchingRule(string matchingRule)
        {
            _sortKey.MatchingRule = matchingRule;
        }
    }
}