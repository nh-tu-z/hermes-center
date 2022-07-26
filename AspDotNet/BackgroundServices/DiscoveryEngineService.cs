using HermesCenter.Logger;
using HermesCenter.AssetDiscovery;

namespace HermesCenter.BackgroundServices
{
    public class DiscoveryEngineService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public DiscoveryEngineService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var scope = _serviceProvider.CreateScope().ServiceProvider;
            var logManager = scope.GetRequiredService<ILogManager>();
            var engine = scope.GetRequiredService<IAssetDiscoveryEngine>();

            logManager.Information("Background Discovery Engine Start!", "HermesCenter");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    engine.Start();
                    // TODO - ?
                    await Task.Delay(Timeout.Infinite, stoppingToken);
                }
                catch (Exception ex)
                {
                    logManager.Error(ex.Message, "HermesCenter");
                }
            }
        }
    }
}
