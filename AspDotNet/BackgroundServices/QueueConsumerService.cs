using StackExchange.Redis;
using HermesCenter.Logger;

namespace HermesCenter.BackgroundServices
{
    public class QueueConsumerService :BackgroundService
    {
        private readonly ConnectionMultiplexer _connectionMultiplexer;
        private readonly IDatabase _database;
        private readonly ILogManager _logManager;
        private CancellationToken _stoppingToken;
        private bool _receiving;
        
        public QueueConsumerService(IServiceProvider serviceProvider)
        {
            var scope = serviceProvider.CreateScope().ServiceProvider;
            _logManager = scope.GetRequiredService<ILogManager>();
            _connectionMultiplexer = ConnectionMultiplexer.Connect("localhost");
            _database = _connectionMultiplexer.GetDatabase();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logManager.Information("Queue Consumer Start!", "HermesCenter");
            _stoppingToken = stoppingToken;
            try
            {
                var sub = _connectionMultiplexer.GetSubscriber();
                sub.Subscribe($"redis-event:channel", async (channel, message) => await HandleNewMessageAsync());
            }
            catch (Exception ex)
            {
                _logManager.Error(ex.Message, "HermesCenter");
            }
        }


        /// <summary>
        /// We have received an indicator that new messages are available
        /// We process until we are out of messages.
        /// </summary>
        private async Task HandleNewMessageAsync()
        {
            if (_receiving) return;

            _receiving = true;

            var (message, key) = await GetMessageAsync();
            // If a valid message cannot be found, it will return an empty Dictionary
            while (message.Count != 0)
            {
                //TODO: Process message, for now save message to CosmosDB
                await ProccessMessagesAsync(message.GetValueOrDefault("message"));

                //Remove message after it's done
                await _database.ListRemoveAsync("redis-event:process", key);
                await _database.KeyDeleteAsync(key);

                // Get a new message if there is one
                (message, key) = await GetMessageAsync();
            }
            _receiving = false;
        }

        /// <summary>
        /// We have received an indicator that new messages are available
        /// We process until we are out of messages.
        /// </summary>
        private async Task<(Dictionary<RedisValue, RedisValue>, string key)> GetMessageAsync()
        {
            var value = new Dictionary<RedisValue, RedisValue>();
            string key = string.Empty;
            while (!_stoppingToken.IsCancellationRequested)
            {
                key = await _database.ListRightPopLeftPushAsync("redis-event:message", "redis-event:process");
                // If key is null, then nothing was there to get, so no value is available
                if (string.IsNullOrEmpty(key))
                {
                    value.Clear();
                    break;
                }

                await _database.HashSetAsync(key, "active", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                value = (await _database.HashGetAllAsync(key)).ToDictionary();

                // if Count is 0, remove it and check for the next message
                if (value.Count == 0)
                {
                    await _database.ListRemoveAsync("redis-event:process", key);
                    continue;
                }
                value.Add("key", key);

                break;
            }

            return (value, key);
        }

        private async Task ProccessMessagesAsync(RedisValue message)
        {
            //var log = new Log
            //{
            //    Id = Guid.NewGuid().ToString(),
            //    Name = message
            //};

            //await _cosmosDbService.AddAsync(log);
        }
    }
}
