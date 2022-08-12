namespace HermesCenter.Common.Configuration
{
    public class QueueConfig
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string MessageName { get; set; } = string.Empty;
        public int TimesRetry { get; set; }
        public int RetryAfer { get; set; }
    }
}
