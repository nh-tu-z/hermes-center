namespace HermesCenter.Common.CommandText
{
    public class IntegrationCommandText
    {
        public const string GetAll = @"SELECT * FROM c";
        public const string GetByTokenId = @"SELECT * FROM c WHERE c.tokenId = '{0}' AND c.tenantId = {1}";
        public const string GetSyncItems = @"SELECT * FROM c WHERE c.HadSync = false OR c.TenantId <> {0}";
    }
}
