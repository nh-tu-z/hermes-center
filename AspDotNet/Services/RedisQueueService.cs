using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using HermesCenter.Common;
using HermesCenter.Common.Configuration;
using HermesCenter.Interfaces;

namespace HermesCenter.Services
{
    public class RedisQueueService : IRedisQueueService
    {
        private readonly QueueConfig _queueConfig;
        private readonly ConnectionMultiplexer _connectionMultiplexer;
        private readonly IDatabase _database;

        public RedisQueueService(IOptions<QueueConfig> queueConfig)
        {
            _queueConfig = queueConfig.Value;
            _connectionMultiplexer = ConnectionMultiplexer.Connect(_queueConfig.ConnectionString);
            _database = _connectionMultiplexer.GetDatabase();
        }

        public async Task InsertMessageAsync(string message)
        {
            Dictionary<RedisValue, RedisValue> parametersDictionary = new()
            {
                { Constants.QueueMessagesColumns.Id, Guid.NewGuid().ToString() },
                { Constants.QueueMessagesColumns.Message, message }
            };

            var id = await _database.StringIncrementAsync($"{_queueConfig.MessageName}:messageid");
            var key = $"{_queueConfig.MessageName}:{id}";

            await _database.HashSetAsync(key, parametersDictionary.Select(entries => new HashEntry(entries.Key, entries.Value)).ToArray());

            await _database.ListLeftPushAsync($"{_queueConfig.MessageName}:{Constants.QueueMessage}", key);

            var eventType = JArray.Parse(message).FirstOrDefault()["eventType"].ToString();
            await _connectionMultiplexer.GetSubscriber().PublishAsync(eventType, message);
        }
    }
}
