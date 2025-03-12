using System.Collections.Generic;

namespace Mvc.Utility.Core.Managers.LinqToLdap.Collections
{
    internal class VirtualListView<T> : List<T>, IVirtualListView<T>
    {
        public VirtualListView(int contentCount, byte[] contextId, int targetPosition, IEnumerable<T> view)
            : base(view)
        {
            ContentCount = contentCount;
            ContextId = contextId;
            TargetPosition = targetPosition;
        }

        public int ContentCount { get; }
        public byte[] ContextId { get; }
        public int TargetPosition { get; }
    }
}