using HermesCenter.Models;
using HermesCenter.AssetDiscovery;

namespace HermesCenter.Interfaces
{
    public interface IApiManagementService
    {
        Task<HttpResponseMessage> CheckIntegrationStatusAsync(IntegrationMessage message);
        Task<HttpResponseMessage> SyncAssetAsync(string token, MLAsset asset);
    }
}
