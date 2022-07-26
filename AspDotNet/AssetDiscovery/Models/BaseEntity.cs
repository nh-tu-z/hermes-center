using Newtonsoft.Json;

namespace HermesCenter.AssetDiscovery
{
    public class BaseEntity
    {

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
    }
}