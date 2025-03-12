using System.DirectoryServices.Protocols;

namespace Kharazmi.DirectoryServices.Ldap.LinqToLdap.EventListeners
{
    /// <summary>
    ///     The event raised before a delete occurs.
    /// </summary>
    public interface IPreDeleteEventListener : IPreEventListener<string, DeleteRequest>, IDeleteEventListener
    {
    }
}