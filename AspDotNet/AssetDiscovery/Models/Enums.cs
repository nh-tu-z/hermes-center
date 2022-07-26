namespace HermesCenter.AssetDiscovery
{
    public enum TriggerType
    {
        Event,
        Initialize,
        Schedule

    }
    public enum TriggerFrequency
    {
        Always,
        Once

    }

    public static class EventType
    {
        public const string AssetCreated = "asset:created";
        public const string AssetUpdated = "asset:updated";
        public const string AssetDeleted = "asset:deleted";
        public const string UserCreated = "user:created";
        public const string UserUpdated = "user:updated";
        public const string UserDeleted = "user:deleted";
        public const string RetrieveExperiment = "retrieve:experiment";
        /// <summary>
        /// Raised when a new Model or Model version has been successfully registered.
        /// </summary>
        public const string ModelRegistered = "Microsoft.MachineLearningServices.ModelRegistered";
        /// <summary>
        /// Raised when Model(s) have been successfully deployed to an Endpoint.
        /// </summary>
        public const string ModelDeployed = "Microsoft.MachineLearningServices.ModelDeployed";
        /// <summary>
        /// Raised when a Run has been successfully completed.
        /// </summary>
        public const string RunCompleted = "Microsoft.MachineLearningServices.RunCompleted";
        /// <summary>
        /// Raised when a Dataset drift monitor detects drift.
        /// </summary>
        public const string DatasetDriftDetected = "Microsoft.MachineLearningServices.DatasetDriftDetected";
        /// <summary>
        /// Raised when a run status changes.
        /// </summary>
        public const string RunStatusChanged = "Microsoft.MachineLearningServices.RunStatusChanged";
    }
    public static class CosmosContainers
    {
        public const string Asset = "Asset";
        public const string RunLog = "RunLog";
    }
    public static class RunStatus
    {
        public const string Success = "Success";
        public const string Failed = "Failed";
    }
    public static class AssetType
    {
        public const string Subscription = "Subscription";
        public const string ResourceGroup = "ResourceGroup";
        public const string Workspace = "Workspace";
        public const string Model = "Model";
        public const string Dataset = "Dataset";
        public const string Experiment = "Experiment";
        public const string Deployment = "Deployment";
        public const string Endpoint = "Endpoint";
        public const string User = "User";
        public const string RunJob = "RunJob";
    }

}