using Microsoft.AspNetCore.Mvc;
using HermesCenter.Interfaces;
using HermesCenter.Models;
using HermesCenter.General.Events;

namespace HermesCenter.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class LogCollectorController : ControllerBase
    {
        private readonly IIntegrationService _integrationService;
        public LogCollectorController(IIntegrationService integrationService)
        {
            _integrationService = integrationService ?? throw new ArgumentException(nameof(integrationService));
        }

        /// <summary>
        /// Create log async
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateLogAsync(Integration integration)
        {
            var result = await _integrationService.CreateIntegrationAsync(integration);

            return Ok(new Response<Integration>(result));
        }

        /// <summary>
        /// Get log by Id async
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLogByIdAsync([FromRoute] string id)
        {
            var result = await _integrationService.GetIntegrationByIdAsync(id);

            return Ok(new Response<Integration>(result));
        }

        /// <summary>
        /// delete log by Id async
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLogByIdAsync(string id)
        {
            await _integrationService.DeleteIntegrationAsync(id);

            return Ok();
        }

        /// <summary>
        /// Get key vault for demo
        /// </summary>
        /// <returns></returns>
        [HttpGet("key/{id}")]
        public async Task<IActionResult> GetKeyVaultByIdAsync(string id)
        {
            //var secretKeyVault = await _secretClient.GetSecretAsync(id);

            return Ok();
        }
    }
}
