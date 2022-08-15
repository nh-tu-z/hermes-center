namespace HermesCenter.Interfaces
{
    public interface ICosmosDbService
    {
        Task<T?> GetByIdAsync<T>(string id, string containerName = null) where T : class;
        Task<T> AddAsync<T>(T entity, string containerName = null) where T : class;
        Task DeleteAsync<T>(string id, string containerName = null) where T : class;
        Task<IEnumerable<T>> GetMultipleAsync<T>(string queryString, string containerName = null) where T : class;
    }
}
