using Microsoft.Azure.Cosmos;
using HermesCenter.Interfaces;

namespace HermesCenter.Services
{
    public class CosmosDbService : ICosmosDbService
    {
        private readonly CosmosClient _cosmosDbClient;
        private readonly string _databaseName;
        private readonly Container _container;
        public CosmosDbService(CosmosClient cosmosDbClient, string databaseName, string containerName)
        {
            _container = cosmosDbClient.GetContainer(databaseName, containerName);
            _cosmosDbClient = cosmosDbClient;
            _databaseName = databaseName;
        }

        public async Task<T?> GetByIdAsync<T>(string id, string containerName = null) where T : class
        {
            try
            {
                var response = await GetContainer(containerName).ReadItemAsync<T>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException)
            {
                return default;
            }
        }

        public async Task DeleteAsync<T>(string id, string containerName = null) where T : class
        {
            await GetContainer(containerName).DeleteItemAsync<T>(id, new PartitionKey(id));
        }

        public async Task<T> AddAsync<T>(T entity, string containerName = null) where T : class
        {
            var response = await GetContainer(containerName).CreateItemAsync(entity);
            return response.Resource;
        }

        public async Task<IEnumerable<T>> GetMultipleAsync<T>(string queryString, string containerName = null) where T : class
        {
            var query = GetContainer(containerName).GetItemQueryIterator<T>(new QueryDefinition(queryString));
            var results = new List<T>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task<T> UpdateAsync<T>(T entity, string id, string containerName = null) where T : class
        {
            var response = await GetContainer(containerName).UpsertItemAsync(entity, new PartitionKey(id));
            return response.Resource;
        }

        private Container GetContainer(string containerName)
        {
            return (containerName == null) ? _container : _cosmosDbClient.GetContainer(_databaseName, containerName);
        }
    }
}
