using System;
using System.Collections.Generic;
using Mvc.Utility.Core.Managers.LinqToLdap.Transformers;

namespace Mvc.Utility.Core.Managers.LinqToLdap.QueryCommands.Options
{
    internal class ListAttributesQueryCommandOptions : QueryCommandOptions
    {
        public ListAttributesQueryCommandOptions(Dictionary<string, string> attributes)
            : base(attributes ?? new Dictionary<string, string>())
        {
        }

        public override Type GetEnumeratorReturnType()
        {
            return typeof(KeyValuePair<string, IEnumerable<KeyValuePair<string, object>>>);
        }

        public override IResultTransformer GetTransformer()
        {
            return new RawEntryResultTransformer();
        }
    }
}