namespace HermesCenter.AssetDiscovery.Azure
{
    public class AzureMLAsset : MLAsset
    {
        public static IList<MLAsset> EmptyList(string type, string parentId)
        {
            return new List<MLAsset>() {
                new AzureMLAsset() { AssetType = type, ParentId = parentId, Id = Guid.Empty.ToString() }
            };
        }

        public string SubscriptionId { get; set; }
        public string ResourceGroupId { get; set; }
        public string WorkspaceId { get; set; }
    }
}
