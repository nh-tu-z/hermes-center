using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using HermesCenter.Common.Configuration;
using HermesCenter.Interfaces;
using HermesCenter.Models;
using HermesCenter.AssetDiscovery;

namespace HermesCenter.Services
{
    public class ApiManagementService : IApiManagementService
    {
        private readonly ApiManagementConfig _apiManagementConfig;
        private readonly HttpClient _httpClient;
        private readonly string _authorization = "Authorization";

        public ApiManagementService(HttpClient httpClient, IOptions<ApiManagementConfig> apiManagementConfig)
        {
            _apiManagementConfig = apiManagementConfig.Value;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(_apiManagementConfig.BaseUri);
            _httpClient.Timeout = new TimeSpan(0, 5, 0);
        }

        public async Task<HttpResponseMessage> CheckIntegrationStatusAsync(IntegrationMessage message)
        {
            if (_httpClient.DefaultRequestHeaders.Contains(_authorization))
            {
                _httpClient.DefaultRequestHeaders.Remove(_authorization);
            }
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(_authorization, message.Token);

            var result = await _httpClient.PostAsJsonAsync(_apiManagementConfig.CheckIntegration, message);

            return result;
        }

        public async Task<HttpResponseMessage> SyncAssetAsync(string token, MLAsset asset)
        {
            if (_httpClient.DefaultRequestHeaders.Contains(_authorization))
            {
                _httpClient.DefaultRequestHeaders.Remove(_authorization);
            }
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(_authorization, token);
            var json = JsonConvert.SerializeObject(asset);
            var body = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await _httpClient.PostAsync(_apiManagementConfig.SyncAsset, body);

            return result;
        }
    }
}
