namespace HermesCenter.Common
{
    public class Enums
    {
        public enum IntegrationState : int
        {
            Connected = 1,
            Disconnected = 2,
            Disabled = 3,
        }

        public enum IntegrationActionType
        {
            Enable = 1,
            Disable = 2,
            CheckConnection = 3,
            SyncIntegration = 4
        }

        public enum MessageType
        {
            Integration = 1,
        }
    }
}
