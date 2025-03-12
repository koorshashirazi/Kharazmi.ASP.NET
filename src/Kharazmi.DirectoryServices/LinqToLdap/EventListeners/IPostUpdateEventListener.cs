using System.DirectoryServices.Protocols;

namespace Kharazmi.DirectoryServices.Ldap.LinqToLdap.EventListeners
{
    /// <summary>
    ///     The event raised after an update occurs.
    /// </summary>
    public interface IPostUpdateEventListener : IPostEventListener<object, ModifyRequest, ModifyResponse>,
        IUpdateEventListener
    {
    }
}