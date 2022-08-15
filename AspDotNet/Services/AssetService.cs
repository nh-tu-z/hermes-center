using HermesCenter.Interfaces;
using HermesCenter.AssetDiscovery;
using HermesCenter.Common;

namespace HermesCenter.Services
{
    public class AssetService
    {
        private readonly ICosmosDbService _cosmosDbService;

        public AssetService(ICosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService ?? throw new ArgumentNullException(nameof(cosmosDbService));
        }

        public IList<MLAsset> GetChildren(string parentId)
        {
            var query = $"SELECT * FROM c WHERE c.ParentId = '{parentId}'";
            return _cosmosDbService.GetMultipleAsync<MLAsset>(query, Constants.CosmosContainers.Asset).Result.ToList();
        }

        public MLAsset GetWorkspaceAsset(string subscriptionId, string resourceGroup, string workspace)
        {
            var query = $"SELECT TOP 1 * FROM c WHERE c.AssetType = 'Workspace' AND c.AssetName = '{workspace}' AND c.Data.ResourceGroupId = '{resourceGroup}' AND c.Data.SubscriptionId = '{subscriptionId}' ORDER BY c.LastModifiedAt DESC";
            return _cosmosDbService.GetMultipleAsync<MLAsset>(query, CosmosContainers.Asset).Result.FirstOrDefault();
        }

        public MLAsset GetLatestAssetByAssetId(string assetType, string assetId)
        {
            var query = $"SELECT TOP 1 * FROM c WHERE c.AssetType = '{assetType}' AND c.AssetId = '{assetId}' ORDER BY c.LastModifiedAt DESC";
            return _cosmosDbService.GetMultipleAsync<MLAsset>(query, CosmosContainers.Asset).Result.FirstOrDefault();
        }

        public MLAsset GetEndpointAsset(string modelIds)
        {
            var query = $"SELECT TOP 1 * FROM c WHERE c.AssetType = 'Endpoint' AND c.ModelIds = '{modelIds}' ORDER BY c.LastModifiedAt DESC";
            return _cosmosDbService.GetMultipleAsync<MLAsset>(query, CosmosContainers.Asset).Result.FirstOrDefault();
        }

        public static MLAsset CreateDeleteAsset(MLAsset asset)
        {
            asset.Event = Constants.AzureEvent.Delete;
            asset.LastModifiedBy = "";
            asset.LastModifiedAt = DateTime.UtcNow;
            asset.HadSync = false;
            asset.LastSync = null;
            return asset;
        }
    }
}
