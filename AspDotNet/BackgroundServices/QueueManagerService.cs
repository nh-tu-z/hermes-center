using HermesCenter.Logger;

namespace HermesCenter.BackgroundServices
{
    /// <summary>
    /// Ref for backgroud services: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-6.0&tabs=visual-studio 
    /// This implementation hosts service that activates a scoped service. The scope service ca use dependency injection (DI)
    /// 
    /// TODO - add information about primitive project
    /// </summary>
    public class QueueManagerService : BackgroundService
    {
        private readonly ILogManager _logManager;

        public QueueManagerService(IServiceProvider serviceProvider /*IOption to config queue*/)
        {
            var scope = serviceProvider.CreateScope().ServiceProvider;
            _logManager = scope.GetRequiredService<ILogManager>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logManager.Information("Queue Manager Start!", "HermesCenter");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(10000, stoppingToken);
                    _logManager.Information("Queue Manager Running...", "HermesCenter");
                }
                catch (Exception ex)
                {
                    _logManager.Error(ex.Message, "HermesCenter");
                }
            }
        }
    }
}
