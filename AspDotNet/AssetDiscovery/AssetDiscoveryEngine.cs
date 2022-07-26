using HermesCenter.Logger;

namespace HermesCenter.AssetDiscovery
{
    public class AssetDiscoveryEngine : IAssetDiscoveryEngine
    {
        private readonly ILogManager _logManager;

        public AssetDiscoveryEngine(ILogManager logManager)
        {
            _logManager = logManager;
        }

        public void Start()
        {

        }
    }
}
