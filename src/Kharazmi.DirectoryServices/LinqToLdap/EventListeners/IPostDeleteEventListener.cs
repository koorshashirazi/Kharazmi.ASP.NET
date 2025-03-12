using System.DirectoryServices.Protocols;

namespace Kharazmi.DirectoryServices.Ldap.LinqToLdap.EventListeners
{
    /// <summary>
    ///     The event raised after a delete occurs.
    /// </summary>
    public interface IPostDeleteEventListener : IPostEventListener<string, DeleteRequest, DeleteResponse>,
        IDeleteEventListener
    {
    }
}