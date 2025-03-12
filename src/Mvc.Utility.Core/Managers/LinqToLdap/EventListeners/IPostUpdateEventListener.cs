using System.DirectoryServices.Protocols;

namespace Mvc.Utility.Core.Managers.LinqToLdap.EventListeners
{
    /// <summary>
    ///     The event raised after an update occurs.
    /// </summary>
    public interface IPostUpdateEventListener : IPostEventListener<object, ModifyRequest, ModifyResponse>,
        IUpdateEventListener
    {
    }
}