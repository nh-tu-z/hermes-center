namespace HermesCenter.AssetDiscovery
{
    public class MLAsset : BaseEntity
    {
        public string AssetType { get; set; }
        public string AssetId { get; set; }
        public string AssetName { get; set; }
        public string ParentId { get; set; }
        public object Data { get; set; }
        public string Event { get; set; }
        public string EventTrigger { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public bool IsHierarchy { get; set; } = false;
        public bool HadSync { get; set; } = false;
        public DateTime? LastSync { get; set; }
        public string IntegrationId { get; set; }
        public int TenantId { get; set; }

        public bool IsEmpty()
        {
            return Guid.Empty.ToString() == Id;
        }
        public bool IsUpdated(MLAsset exist)
        {
            //In case there is no value of LastModifiedAt, skip update
            if (exist.LastModifiedAt == null)
            {
                return false;
            }
            return exist.Id == Id && LastModifiedAt > exist.LastModifiedAt;
        }

    }
}