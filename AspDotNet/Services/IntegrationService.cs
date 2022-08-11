using HermesCenter.Interfaces;
using HermesCenter.Models;

namespace HermesCenter.Services
{
    public class IntegrationService : IIntegrationService
    {
        public IntegrationService()
        {

        }

        public async Task<Integration> CreateIntegrationAsync(Integration integration)
        {
            // TODO - implement
            return new Integration();
        }

        public async Task<Integration> GetIntegrationByIdAsync(string id)
        {
            // TODO - implement
            return new Integration();
        }

        public async Task DeleteIntegrationAsync(string id)
        {
            // TODO - implement
            //return new Integration();
        }
    }
}
