using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Kharazmi.Net.Http.HttpManager
{
    public interface IHttpClientFactory : IDisposable
    {
        HttpClient GetOrCreate(
            string baseAddress,
            IDictionary<string, string> defaultRequestHeaders = null,
            TimeSpan? timeout = null,
            long? maxResponseContentBufferSize = null,
            HttpMessageHandler handler = null);
    }
}