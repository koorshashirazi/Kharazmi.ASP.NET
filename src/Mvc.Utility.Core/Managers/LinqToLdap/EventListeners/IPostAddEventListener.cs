using System.DirectoryServices.Protocols;

namespace Mvc.Utility.Core.Managers.LinqToLdap.EventListeners
{
    /// <summary>
    ///     The event raised after an add occurs.
    /// </summary>
    public interface IPostAddEventListener : IPostEventListener<object, AddRequest, AddResponse>, IAddEventListener
    {
    }
}