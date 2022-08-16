using HermesCenter.AssetDiscovery;
using HermesCenter.Common;
using HermesCenter.Common.CommandText;
using HermesCenter.Interfaces;
using HermesCenter.Models;
using HermesCenter.Logger;

namespace HermesCenter.Services
{
    public class IntegrationService : IIntegrationService
    {
        private readonly ICosmosDbService _cosmosDbService;
        private readonly IApiManagementService _apiManagementService;
        private readonly ILogManager _logManager;
        private readonly string _integrationContainer = Constants.CosmosContainers.Integration;

        public IntegrationService(ICosmosDbService cosmosDbService, ILogManager logManager, IApiManagementService apiManagementService)
        {
            _cosmosDbService = cosmosDbService;
            _logManager = logManager;
            _apiManagementService = apiManagementService;
        }

        public async Task<Integration> CreateIntegrationAsync(Integration integration)
        {
            return await _cosmosDbService.AddAsync(integration, _integrationContainer);
        }

        public async Task<Integration> GetIntegrationByIdAsync(string id)
        {
            return await _cosmosDbService.GetByIdAsync<Integration>(id, _integrationContainer);
        }

        public async Task DeleteIntegrationAsync(string id)
            => await _cosmosDbService.DeleteAsync<Integration>(id, _integrationContainer);

        public async Task SyncAssetsAsync(Integration integration = null)
        {
            if (integration == null)
            {
                integration = (await _cosmosDbService.GetMultipleAsync<Integration>(IntegrationCommandText.GetAll, _integrationContainer)).FirstOrDefault();
            }

            if (integration != null && (await HealthCheckIntegration(integration)))
            {
                var assets = await _cosmosDbService.GetMultipleAsync<MLAsset>(string.Format(IntegrationCommandText.GetSyncItems, integration.TenantId), Constants.CosmosContainers.Asset);
                foreach (var asset in assets)
                {
                    asset.IntegrationId = integration.ItemId;
                    asset.TenantId = integration.TenantId;
                    var responce = await _apiManagementService.SyncAssetAsync(integration.Token, asset);
                    string content = responce.Content != null ? await responce.Content.ReadAsStringAsync() : string.Empty;

                    if (responce.IsSuccessStatusCode)
                    {
                        asset.HadSync = true;
                        asset.LastSync = DateTime.UtcNow;
                        await _cosmosDbService.UpdateAsync(asset, asset.Id, Constants.CosmosContainers.Asset);
                        _logManager.Information($"Succeed to sync asset {asset.AssetId}: {content}", GetType().Name);
                    }
                    else
                    {
                        _logManager.Error($"Failed to sync asset {asset.AssetId}: {content}", GetType().Name);
                    }
                }
            }
            else
            {
                _logManager.Information($"Missing integration or integration invalid!", GetType().Name);
            }
        }

        private async Task<bool> HealthCheckIntegration(Integration integration)
        {
            var message = new IntegrationMessage
            {
                TokenId = Guid.Parse(integration.TokenId),
                TenantId = integration.TenantId,
                State = (int)Enums.IntegrationState.Connected,
                MessageTypeId = (int)Enums.MessageType.Integration,
                ActionTypeId = (int)Enums.IntegrationActionType.CheckConnection,
                Token = integration.Token

            };

            var result = await _apiManagementService.CheckIntegrationStatusAsync(message);
            if (result.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
    }
}
