namespace HermesCenter.AssetDiscovery
{
    public class Trigger
    {
        public TriggerType Type { get; set; }
        public TriggerFrequency Frequency { get; set; }
        public string Schedule { get; set; } // '0 * * * * *' CRON spec
        public string[] Events { get; set; } // events that the job will subscribe to.
        public Func<MLAsset, bool> Filter { get; set; }
    }
}