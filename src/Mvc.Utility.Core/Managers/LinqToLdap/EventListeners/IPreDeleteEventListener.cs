using System.DirectoryServices.Protocols;

namespace Mvc.Utility.Core.Managers.LinqToLdap.EventListeners
{
    /// <summary>
    ///     The event raised before a delete occurs.
    /// </summary>
    public interface IPreDeleteEventListener : IPreEventListener<string, DeleteRequest>, IDeleteEventListener
    {
    }
}