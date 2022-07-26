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

        public AssetDiscoveryEngine(ILogManager logManager, IMLConnector connector, IServiceProvider serviceProvider)
        {
            _logManager = logManager;
            _connector = connector;
            _serviceProvider = serviceProvider;
            redis = ConnectionMultiplexer.Connect("localhost");
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
                //_backgroundJobClient.Enqueue(() => RunInitializeJob(job.Id));
            }
            else if (job.Trigger.Frequency == TriggerFrequency.Once)
            {
                //if (!IsRun(job.Id))
                //{
                //    _backgroundJobClient.Enqueue(() => RunInitializeJob(job.Id));
                //}
            }
        }

        private void HandleScheduleJob(IMLJob job)
        {

        }

        private void HandleEventJob(IMLJob job)
        {

        }

        public void RunInitialJob()
        {

        }

        public void RunJob(string jobId, object input)
        {

        }
    }
}
