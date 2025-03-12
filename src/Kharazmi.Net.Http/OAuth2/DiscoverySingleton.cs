using Mvc.Utility.Core.Managers.HttpManager;
using Mvc.Utility.Core.Managers.InstanceManager;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace Kharazmi.Net.Http.OAuth2
{
    public class DiscoverySingleton : Singleton<DiscoverySingleton>
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private DiscoverySingleton()
        {
            _httpClientFactory = new HttpClientFactory();
        }

        public HttpClient HttpClient(string baseAddress)
        {
            return _httpClientFactory.GetOrCreate(baseAddress);
        }

        public async Task<DiscoveryResponse> GetDiscoveryResponseAsync(string baseAddress)
        {
            var disco = await HttpClient(baseAddress).GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = baseAddress,
                Policy =
                {
                    RequireHttps = false
                }
            });

            if (disco.IsError) throw new Exception(disco.Error);

            return disco;
        }

        public DiscoveryResponse GetDiscoveryResponse(string baseAddress)
        {
            var disco = HttpClient(baseAddress).GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = baseAddress,
                Policy =
                {
                    RequireHttps = false
                }
            }).ConfigureAwait(false).GetAwaiter().GetResult();

            if (disco.IsError) throw new Exception(disco.Error);

            return disco;
        }
    }
}