using System.Threading.Tasks;

namespace Kharazmi.AspNetMvc.Core.Managers.Authorization
{
    /// <summary>
    ///     Classes implementing this interface are able to make a decision if authorization is allowed.
    /// </summary>
    public interface IAuthorizationHandler
    {
        /// <summary>
        ///     Makes a decision if authorization is allowed.
        /// </summary>
        /// <param name="context">The authorization information.</param>
        Task HandleAsync(AuthorizationHandlerContext context);
    }
}