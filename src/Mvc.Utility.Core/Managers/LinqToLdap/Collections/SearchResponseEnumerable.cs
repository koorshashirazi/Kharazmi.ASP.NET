﻿using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using Mvc.Utility.Core.Managers.LinqToLdap.Transformers;

namespace Mvc.Utility.Core.Managers.LinqToLdap.Collections
{
    internal class SearchResponseEnumerable<T> : IEnumerable<T>
    {
        private readonly int _count;
        private SearchResultEntryCollection _entries;
        private List<SearchResultEntry> _enumerableEntries;
        private IResultTransformer _resultTransformer;

        public SearchResponseEnumerable(SearchResultEntryCollection entries, IResultTransformer resultTransformer)
        {
            _resultTransformer = resultTransformer;
            _entries = entries;
            _count = entries != null ? entries.Count : 0;
        }

        public SearchResponseEnumerable(IResultTransformer resultTransformer, List<SearchResultEntry> entries)
        {
            _resultTransformer = resultTransformer;
            _enumerableEntries = entries;
            _count = _enumerableEntries != null ? _enumerableEntries.Count : 0;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            if (_count == 0) yield break;

            if (_entries != null)
                foreach (SearchResultEntry entry in _entries)
                    yield return (T) _resultTransformer.Transform(entry);
            else
                foreach (var entry in _enumerableEntries)
                    yield return (T) _resultTransformer.Transform(entry);
        }

        public IEnumerator GetEnumerator()
        {
            if (_count == 0) yield break;

            if (_entries != null)
                foreach (SearchResultEntry entry in _entries)
                    yield return _resultTransformer.Transform(entry);
            else
                foreach (var entry in _enumerableEntries)
                    yield return _resultTransformer.Transform(entry);
        }

        public void Dispose()
        {
            _entries = null;
            _enumerableEntries = null;
            _resultTransformer = null;
        }
    }
}