using NCrontab;
using HermesCenter.Logger;

namespace HermesCenter.BackgroundServices
{
    public class SyncAssetService : BackgroundService
    {
        private readonly CrontabSchedule _cronSchedule;
        private DateTime _nextRun;
        private readonly ILogManager _logManager;

        public SyncAssetService(IServiceProvider serviceProvider)
        {
            var scope = serviceProvider.CreateScope().ServiceProvider;
            _logManager = scope.GetRequiredService<ILogManager>();
            _cronSchedule = CrontabSchedule.Parse("*/1 * * * *");
            _nextRun = _cronSchedule.GetNextOccurrence(DateTime.Now);
            _logManager.Information($"Next time constructor {_nextRun.ToString()}", "HermesCenter");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logManager.Information("Sync Asset Start!", "HermesCenter");
            async void action()
            {
                do
                {
                    if (DateTime.Now > _nextRun)
                    {
                        _ = ProcessBackgroudJobAsync();
                        _nextRun = _cronSchedule.GetNextOccurrence(DateTime.Now);
                        _logManager.Information($"Next time Execute {_nextRun.ToString()}", "HermesCenter");
                    }

                    await Task.Delay(5000, stoppingToken);

                } while (!stoppingToken.IsCancellationRequested);
            }

            return Task.Run(action, stoppingToken);
        }

        private async Task ProcessBackgroudJobAsync()
        {
            try
            {
                _logManager.Information("Schedule Run!", "HermesCenter");
            }
            catch (Exception ex)
            {
                _logManager.Error(ex.Message, "HermesCenter");
            }
        }
    }
}
