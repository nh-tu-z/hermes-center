using HermesCenter.Interfaces;
using HermesCenter.Models;
using HermesCenter.Logger;
using HermesCenter.Common;

namespace HermesCenter.Services
{
    public class IntegrationService : IIntegrationService
    {
        private readonly ICosmosDbService _cosmosDbService;
        private readonly ILogManager _logManager;
        private readonly string _integrationContainer = Constants.CosmosContainers.Integration;

        public IntegrationService(ICosmosDbService cosmosDbService, ILogManager logManager)
        {
            _cosmosDbService = cosmosDbService;
            _logManager = logManager;
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
    }
}
