﻿using Microsoft.Owin;

namespace Kharazmi.AspNetMvc.Core.Managers.Authorization
{
    /// <summary>
    ///     A class implementing this interface can enforce authorization without an <see cref="IOwinContext" />.
    /// </summary>
    public interface IAuthorizationController
    {
        /// <summary>
        ///     Gets the <see cref="AuthorizationOptions" />.
        /// </summary>
        AuthorizationOptions AuthorizationOptions { get; }
    }
}