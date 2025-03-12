using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Mvc.Utility.Core.Managers.HttpManager
{
    public interface IHttpClientFactory : IDisposable
    {
        HttpClient GetOrCreate(string baseAddress,
            IDictionary<string, string> defaultRequestHeaders = null,
            TimeSpan? timeout = null,
            long? maxResponseContentBufferSize = null,
            HttpMessageHandler handler = null);
    }
}