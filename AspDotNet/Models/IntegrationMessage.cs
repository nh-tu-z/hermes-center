using Newtonsoft.Json;

namespace HermesCenter.Models
{
    public class IntegrationMessage
    {
        [JsonProperty(PropertyName = "messageTypeId")]
        public int MessageTypeId { get; set; }
        [JsonProperty(PropertyName = "itemId")]
        public Guid ItemId { get; set; }
        [JsonProperty(PropertyName = "tokenId")]
        public Guid TokenId { get; set; }
        [JsonProperty(PropertyName = "tenantId")]
        public int TenantId { get; set; }
        [JsonProperty(PropertyName = "actionTypeId")]
        public int ActionTypeId { get; set; }
        [JsonProperty(PropertyName = "state")]
        public int State { get; set; }
        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; } = string.Empty;
    }
}
