using HermesCenter.AssetDiscovery.Azure.Models;

namespace HermesCenter.AssetDiscovery.Azure
{

    /// <summary>
    /// TODO - add description
    /// </summary>
    public static class AzureMLAssetFactory
    {
        public static AzureMLAsset CreateSubscription(Subscription sub)
        {
            return new AzureMLAsset()
            {
                SubscriptionId = sub.SubscriptionId,
                AssetType = AssetType.Subscription,
                AssetName = sub.SubcriptionName,
                AssetId = sub.SubscriptionId,
                Data = sub,
                IsHierarchy = true
            };
        }
    }
}
