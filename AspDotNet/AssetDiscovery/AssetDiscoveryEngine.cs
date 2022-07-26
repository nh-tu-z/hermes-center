using Hangfire;
using StackExchange.Redis;
using HermesCenter.Logger;

namespace HermesCenter.AssetDiscovery
{
    public class AssetDiscoveryEngine : IAssetDiscoveryEngine
    {
        private readonly ILogManager _logManager;
        private readonly IMLConnector _connector;
        private readonly IServiceProvider _serviceProvider;
        private readonly ConnectionMultiplexer redis;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IRecurringJobManager _recurringJobManager;

        public AssetDiscoveryEngine(ILogManager logManager, 
            IMLConnector connector,
            IServiceProvider serviceProvider,
            IRecurringJobManager recurringJobManager,
            IBackgroundJobClient backgroundJobClient)
        {
            _logManager = logManager;
            _connector = connector;
            _serviceProvider = serviceProvider;
            redis = ConnectionMultiplexer.Connect("localhost");
            _recurringJobManager = recurringJobManager;
            _backgroundJobClient = backgroundJobClient;
        }

        public void Start()
        {
            IList<IMLJob> jobs = _connector.GetJobs();
            foreach (var job in jobs)
            {
                if (job.Trigger.Type == TriggerType.Initialize)
                {
                    _logManager.Information("Trigger Initialization", "HermesCenter");
                    HandleInitialJob(job);
                }
                else if (job.Trigger.Type == TriggerType.Schedule)
                {
                    _logManager.Information("Trigger Schedule", "HermesCenter");
                    HandleScheduleJob(job);
                }
                else if (job.Trigger.Type == TriggerType.Event)
                {
                    _logManager.Information("Trigger Event", "HermesCenter");
                    HandleEventJob(job);
                }
            }
        }

        private void HandleInitialJob(IMLJob job)
        {
            // Check run once or always
            if (job.Trigger.Frequency == TriggerFrequency.Always)
            {
                _backgroundJobClient.Enqueue(() => RunInitialJob(job.Id));
            }
            else if (job.Trigger.Frequency == TriggerFrequency.Once)
            {
                if (!IsRun(job.Id))
                {
                    _backgroundJobClient.Enqueue(() => RunInitialJob(job.Id));
                }
            }
        }

        private void HandleScheduleJob(IMLJob job)
        {
            // Add a schedule
            _recurringJobManager.AddOrUpdate(
                job.Id,
                () => RunScheduledJob(job.Id),
                job.Trigger.Schedule
                );
        }

        private void HandleEventJob(IMLJob job)
        {

        }

        public void RunInitialJob(string jobId)
        {

        }

        public void RunScheduledJob(string jobId)
        {

        }

        public void RunJob(string jobId, object input)
        {

        }

        private bool IsRun(string jobId)
        {
            return true;
        }
    }
}
