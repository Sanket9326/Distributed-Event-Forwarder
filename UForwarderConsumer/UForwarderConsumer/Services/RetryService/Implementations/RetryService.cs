using StackExchange.Redis;
using System.Text.Json;
using UForwarderConsumer.Dtos;

namespace UForwarderConsumer.Services.RetryService.Implementations
{
    public class RetryService : IRetryService
    {
        private readonly IDatabase redisServer;
        private readonly ILogger<RetryService> logger;

        public RetryService(IConnectionMultiplexer redis, ILogger<RetryService> logger)
        {
            this.redisServer = redis.GetDatabase();
            this.logger = logger;
        }

        public async Task<bool> AddForRetry(ForwarderMessage message)
        {
            try
            {
                var nextRetryTime = await GetNextRetryTime(message);
                var retryMessage = new RetryMessage
                {
                    MessageId = message.MessageId ?? Guid.NewGuid().ToString(),
                    NextRetryAt = nextRetryTime,
                    RetryCount = message.RetryCount + 1,
                    EventType = message.EventType,
                    Payload = message.Payload
                };

                await AddToRetryQueue(retryMessage);
                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error adding message for retry");
                return false;
            }
        }

        /// <summary>
        /// Calculates the next retry time based on the current retry count of the message.
        /// </summary>
        /// <param name="message">The message for which to calculate the next retry time.</param>
        /// <returns>The next retry time as a DateTime.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the maximum retry attempts are exceeded.</exception>
        private async Task<DateTime> GetNextRetryTime(ForwarderMessage message)
        {
            try
            {
                int currentRetryCount = message.RetryCount;
                DateTime currTime = DateTime.UtcNow;

                switch (currentRetryCount)
                {
                    case 0:
                        return currTime.AddSeconds(10);
                    case 1:
                        return currTime.AddMinutes(1);
                    case 2:
                        return currTime.AddMinutes(1);
                    case 3:
                        return currTime.AddMinutes(1);
                    default:
                        throw new InvalidOperationException("Maximum retry attempts exceeded.");
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error calculating next retry time");
                throw;
            }
        }

        /// <summary>
        /// Adds the given message to the retry queue in Redis with a score based on the next retry time.
        /// </summary>
        /// <param name="message">The message to add to the retry queue.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task AddToRetryQueue(RetryMessage message)
        {
            try
            {
                string serialized = JsonSerializer.Serialize(message);

                double score =
                     new DateTimeOffset(message.NextRetryAt)
                    .ToUnixTimeSeconds();

                await redisServer.SortedSetAddAsync(
                     "retry_queue",
                      serialized,
                      score);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error adding message to retry queue");
                throw;
            }
        }
    }
}
