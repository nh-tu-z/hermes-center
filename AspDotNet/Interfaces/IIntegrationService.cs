using HermesCenter.Models;

namespace HermesCenter.Interfaces
{
    public interface IIntegrationService
    {
        Task<Integration> GetIntegrationByIdAsync(string id);
        Task<Integration> CreateIntegrationAsync(Integration integration);
        Task DeleteIntegrationAsync(string id);
    }
}
