namespace HermesCenter.AssetDiscovery
{
    public class ListSubscription : IMLJob
    {
        public string Id { get; set; } = nameof(ListSubscription);
        public Trigger Trigger { get; set; } = new Trigger()
        {
            Type = TriggerType.Initialize,
            Frequency = TriggerFrequency.Always
        };

        public ListSubscription(IServiceProvider serviceProvider)
        {

        }

        public IList<MLAsset> Execute(object input = null)
        {
            var assets = new List<MLAsset>();
            //var armClient = new ArmClient(new DefaultAzureCredential());

            //var subscriptions = armClient.GetSubscriptions().ToList();

            //foreach (var subscription in subscriptions)
            //{
            //    //Load subscriptions
            //    Subscription sub = SubscriptionMapper.Map(subscription);
            //    assets.Add(AzureMLAssetFactory.CreateSubscription(sub));
            //};

            //if (!assets.Any())
            //{
            //    // Return empty asset with type for handling deleted assets in DiscoveryEngine
            //    assets.Add(new AzureMLAsset() { AssetType = AssetType.Subscription, ParentId = string.Empty, Id = Guid.Empty.ToString() });
            //}

            return assets;
        }
    }
}
