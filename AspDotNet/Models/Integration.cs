using Newtonsoft.Json;

namespace HermesCenter.Models
{
    public class Integration
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "itemId")]
        public string ItemId { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "tokenId")]
        public string TokenId { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "tenantId")]
        public int TenantId { get; set; }
        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "status")]
        public int Status { get; set; }
        [JsonProperty(PropertyName = "createdDate")]
        public DateTime CreatedDate { get; set; }
    }
}
