using System;
using System.Net.Http;
using Microsoft.Owin;

namespace Kharazmi.Security.Authorization.WebApi
{
    /// <summary>
    ///     Allows for easy access of the <see cref="IOwinContext" /> in a <see cref="System.Net.Http" /> environment.
    /// </summary>
    public class HttpRequestMessageOwinContextAccessor : IOwinContextAccessor
    {
        private readonly WeakReference<HttpRequestMessage> _httpRequestMessage;

        /// <summary>
        ///     Creates a new <see cref="HttpRequestMessageOwinContextAccessor" /> instance.
        /// </summary>
        /// <param name="httpRequestMessage">
        ///     The <see cref="HttpRequestMessage" /> to use in retrieving an
        ///     <see cref="IOwinContext" />.
        /// </param>
        public HttpRequestMessageOwinContextAccessor(HttpRequestMessage httpRequestMessage)
        {
            if (httpRequestMessage == null)
            {
                throw new ArgumentNullException(paramName: nameof(httpRequestMessage));
            }

            _httpRequestMessage = new WeakReference<HttpRequestMessage>(target: httpRequestMessage);
        }

        /// <summary>
        ///     Gets an <see cref="IOwinContext" />.
        /// </summary>
        public IOwinContext Context
        {
            get
            {
                HttpRequestMessage request;
                if (_httpRequestMessage.TryGetTarget(target: out request))
                {
                    return request.GetOwinContext();
                }

                return null;
            }
        }
    }
}