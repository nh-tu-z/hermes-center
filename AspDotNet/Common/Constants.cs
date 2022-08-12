﻿namespace HermesCenter.Common
{
    public class Constants
    {
        public const string QueueMessage = "messages";

        public static class CosmosContainers
        {
            public const string Asset = "Asset";
            public const string RunLog = "RunLog";
            public const string Integration = "Integration";
        }

        public static class QueueMessagesColumns
        {
            public const string Id = "id";
            public const string Key = "key";
            public const string Active = "active";
            public const string FailedCount = "failedCount";
            public const string Message = "message";
        }
    }
}
