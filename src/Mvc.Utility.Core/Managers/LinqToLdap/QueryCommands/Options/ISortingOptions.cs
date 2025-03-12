using System.DirectoryServices.Protocols;

namespace Mvc.Utility.Core.Managers.LinqToLdap.QueryCommands.Options
{
    /// <summary>
    ///     Interface the contains <see cref="SortKey" />s for a query.
    /// </summary>
    public interface ISortingOptions
    {
        /// <summary>
        ///     The keys containing sort information.
        /// </summary>
        SortKey[] Keys { get; }
    }
}