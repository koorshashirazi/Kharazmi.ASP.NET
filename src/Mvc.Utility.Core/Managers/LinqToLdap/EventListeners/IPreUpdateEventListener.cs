using System.DirectoryServices.Protocols;

namespace Mvc.Utility.Core.Managers.LinqToLdap.EventListeners
{
    /// <summary>
    ///     The event raised before an update occurs.
    /// </summary>
    public interface IPreUpdateEventListener : IPreEventListener<object, ModifyRequest>, IUpdateEventListener
    {
    }
}