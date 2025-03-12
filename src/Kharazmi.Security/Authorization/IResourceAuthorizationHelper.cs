using System.Security.Claims;
using System.Threading.Tasks;

namespace Kharazmi.Security.Authorization
{
    /// <summary>
    ///     Classes implementing this interface are able to authorize with or without owin.
    /// </summary>
    public interface IResourceAuthorizationHelper
    {
        /// <summary>
        ///     Determines if a user is authorized.
        /// </summary>
        /// <param name="controller">The controller from which <see cref="AuthorizationOptions" /> may be obtained.</param>
        /// <param name="user"></param>
        /// <param name="authorizeAttribute">
        ///     The user to evaluate the authorize data against.
        ///     The &lt;see cref=&quot;IAuthorizeData&quot; /&gt; to evaluate.
        /// </param>
        /// <returns>
        ///     A flag indicating whether authorization has succeeded.
        ///     This value is
        ///     <value>true</value>
        ///     when the <paramref name="authorizeAttribute" /> fulfills the <paramref name="authorizeAttribute" />; otherwise
        ///     <value>false</value>
        ///     .
        /// </returns>
        Task<bool> IsAuthorizedAsync(IAuthorizationController controller, ClaimsPrincipal user, IAuthorizeData authorizeAttribute);
    }
}