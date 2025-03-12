using System.DirectoryServices.Protocols;

namespace Mvc.Utility.Core.Managers.LinqToLdap
{
    /// <summary>
    ///     Interface for constructing <see cref="LdapConnection" />s and managing them in a pool.
    /// </summary>
    public interface IPooledLdapConnectionFactory : ILdapConnectionFactory
    {
        /// <summary>
        ///     Reinitializes the pool.
        /// </summary>
        void Reinitialize();
    }
}