using System.Diagnostics.CodeAnalysis;

namespace Kharazmi.Security.Authorization
{
    /// <summary>
    ///     Represents an authorization requirement.
    /// </summary>
    [SuppressMessage(category: "Microsoft.Design", checkId: "CA1040:AvoidEmptyInterfaces", Justification = "Used as a pivot point for common functionality")]
    public interface IAuthorizationRequirement
    {
    }
}