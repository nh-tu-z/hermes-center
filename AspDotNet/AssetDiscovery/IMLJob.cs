namespace HermesCenter.AssetDiscovery
{
    public interface IMLJob
    {
        public string Id { get; set; }
        public Trigger Trigger { get; set; }

        public IList<MLAsset> Execute(object input = null);
    }
}
