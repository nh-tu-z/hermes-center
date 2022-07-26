namespace HermesCenter.AssetDiscovery.Azure.Models
{
    public class Subscription
    {
        public string Id { get; set; }
        public string SubscriptionId { get; set; }
        public List<string> TenantIds { get; set; } = new List<string>();
        public string SubcriptionName { get; set; }
    }
}
