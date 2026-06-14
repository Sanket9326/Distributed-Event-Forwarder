using RetryWorker.Dtos;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace RetryWorker.Services.RetryService.Implementations
{
    public class RetryService : IRetryService
    {
        private readonly ILogger logger;
        private readonly IDatabase redisServer;

        public RetryService(IConnectionMultiplexer redis, ILogger<RetryService> logger)
        {
            this.redisServer = redis.GetDatabase();
            this.logger = logger;
        }

        public async Task<List<RetryMessage>> GetAllMessagesToRetry()
        {
            try
            {
                logger.LogInformation("Retrieving messages to retry from Redis...");

                var messagesToRetry = new List<RetryMessage>();
                double now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                var messages = await redisServer.SortedSetRangeByScoreAsync(
                    "retry_queue",
                    stop: now);

                foreach (var message in messages)
                {
                    try
                    {
                        var retryMessage = JsonSerializer.Deserialize<RetryMessage>(message.ToString());
                        if (retryMessage != null)
                        {
                            messagesToRetry.Add(retryMessage);
                        }
                    }
                    catch (JsonException ex)
                    {
                        this.logger.LogError(ex, "Error deserializing message from retry queue");
                    }
                }

                logger.LogInformation($"Retrieved {messagesToRetry.Count} messages to retry from Redis.");

                return messagesToRetry;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error retrieving messages to retry");
                return new List<RetryMessage>();
            }
        }

        public async Task RemoveFromRetryQueue(RetryMessage message)
        {
            string serialized =
                JsonSerializer.Serialize(message);

            await redisServer.SortedSetRemoveAsync(
                "retry_queue",
                serialized);
        }
    }
}
