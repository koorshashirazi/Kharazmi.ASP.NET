using System.Diagnostics.CodeAnalysis;

namespace Kharazmi.AspNetMvc.Core.Managers.Authorization
{
    /// <summary>
    ///     Represents an authorization requirement.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1040:AvoidEmptyInterfaces", Justification =
        "Used as a pivot point for common functionality")]
    public interface IAuthorizationRequirement
    {
    }
}