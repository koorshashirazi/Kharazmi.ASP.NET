﻿using System;
using System.Web;
using Microsoft.Owin;

namespace Kharazmi.AspNetMvc.Core.Managers.Authorization
{
    /// <summary>
    ///     Allows for easy access of the <see cref="IOwinContext" /> in a <see cref="System.Web" /> environment.
    /// </summary>
    public class HttpContextBaseOwinContextAccessor : IOwinContextAccessor
    {
        private readonly HttpContextBase _httpContextBase;

        /// <summary>
        ///     Creates a new instance of <see cref="HttpContextBaseOwinContextAccessor" />.
        /// </summary>
        /// <param name="httpContextBase">The <see cref="HttpContextBase" /> to be used to get an <see cref="IOwinContext" />.</param>
        public HttpContextBaseOwinContextAccessor(HttpContextBase httpContextBase)
        {
            if (httpContextBase == null) throw new ArgumentNullException(nameof(httpContextBase));

            _httpContextBase = httpContextBase;
        }

        /// <summary>
        ///     Gets an <see cref="IOwinContext" />.
        /// </summary>
        public IOwinContext Context => _httpContextBase.GetOwinContext();
    }
}