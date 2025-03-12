using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using Mvc.Utility.Core.Managers.LinqToLdap.Collections;

namespace Mvc.Utility.Core.Managers.LinqToLdap.Transformers
{
    internal class RawEntryResultTransformer : IResultTransformer
    {
        public object Transform(SearchResultEntry entry)
        {
            return new KeyValuePair<string, IEnumerable<KeyValuePair<string, object>>>(
                entry.DistinguishedName,
                new DirectoryAttributes(entry));
        }

        public object Default()
        {
            return default(KeyValuePair<string, IEnumerable<KeyValuePair<string, object>>>);
        }
    }
}