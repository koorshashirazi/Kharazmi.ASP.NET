using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;

namespace Kharazmi.Net.Http.HttpManager
{
    public class HttpClientFactory : IHttpClientFactory
    {
        private const int CONNECTION_LEASE_TIMEOUT = 60 * 1000; // 1 minute

        private readonly ConcurrentDictionary<Uri, Lazy<HttpClient>> _httpClients =
            new ConcurrentDictionary<Uri, Lazy<HttpClient>>();

        public HttpClientFactory()
        {
            ServicePointManager.DnsRefreshTimeout = (int) TimeSpan.FromMinutes(value: 1).TotalMilliseconds;
            ServicePointManager.DefaultConnectionLimit = 1024;
        }

        public HttpClient GetOrCreate(string baseAddress, IDictionary<string, string> defaultRequestHeaders = null,
            TimeSpan? timeout = null, long? maxResponseContentBufferSize = null, HttpMessageHandler handler = null)
        {
            var baseAddressUrl = new Uri(baseAddress);

            return _httpClients.GetOrAdd(baseAddressUrl,
                uri => new Lazy<HttpClient>(() =>
                    {
                        var client = handler == null
                            ? new HttpClient {BaseAddress = baseAddressUrl}
                            : new HttpClient(handler, disposeHandler: false) {BaseAddress = baseAddressUrl};

                        SetRequestTimeout(timeout, client);
                        SetMaxResponseBufferSize(maxResponseContentBufferSize, client);
                        SetDefaultHeaders(defaultRequestHeaders, client);
                        SetConnectionLeaseTimeout(baseAddressUrl, client);
                        return client;
                    },
                    LazyThreadSafetyMode.ExecutionAndPublication)).Value;
        }

        public void Dispose()
        {
            foreach (var httpClient in _httpClients.Values) httpClient.Value.Dispose();
        }


        private static void SetConnectionLeaseTimeout(Uri baseAddress, HttpClient client)
        {
            client.DefaultRequestHeaders.ConnectionClose = false;
            ServicePointManager.FindServicePoint(baseAddress).ConnectionLeaseTimeout = CONNECTION_LEASE_TIMEOUT;
        }

        private static void SetDefaultHeaders(IDictionary<string, string> defaultRequestHeaders, HttpClient client)
        {
            if (defaultRequestHeaders == null) return;

            foreach (var item in defaultRequestHeaders) client.DefaultRequestHeaders.Add(item.Key, item.Value);
        }

        private static void SetMaxResponseBufferSize(long? maxResponseContentBufferSize, HttpClient client)
        {
            if (maxResponseContentBufferSize.HasValue)
                client.MaxResponseContentBufferSize = maxResponseContentBufferSize.Value;
        }

        private static void SetRequestTimeout(TimeSpan? timeout, HttpClient client)
        {
            if (timeout.HasValue) client.Timeout = timeout.Value;
        }
    }
}