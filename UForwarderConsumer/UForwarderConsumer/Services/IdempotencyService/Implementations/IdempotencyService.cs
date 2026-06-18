using StackExchange.Redis;
using UForwarderConsumer.Enums;

namespace UForwarderConsumer.Services.IdempotencyService.Implementations
{
    public class IdempotencyService : IIdempotencyService
    {
        private readonly IDatabase redisServer;
        private readonly ILogger<IdempotencyService> logger;

        public IdempotencyService(IConnectionMultiplexer redis, ILogger<IdempotencyService> logger)
        {
            this.redisServer = redis.GetDatabase();
            this.logger = logger;
        }

        public async Task<bool> IsMessageAvailableToProcess(string messageId)
        {
            try
            {
                var exists = await redisServer.StringGetAsync(messageId);
                return !exists.IsNullOrEmpty;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error checking message id in Redis");
                return false;
            }
        }

        public async Task<bool> MarkMessageAsProcessing(string messageId)
        {
            try
            {
                this.logger.LogInformation("Marking message with id {MessageId} as processing in Redis", messageId);
                var res = await redisServer.StringSetAsync(messageId, Status.PROCESSING.ToString(), TimeSpan.FromMinutes(1), When.NotExists);
                this.logger.LogInformation("Message with id {MessageId} marked as processing in Redis", messageId);
                return res;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error marking message as processing in Redis");
                throw;
            }
        }

        public async Task<bool> MarkMessageAsCompleted(string messageId)
        {
            try
            {
                this.logger.LogInformation("Marking message with id {MessageId} as completed in Redis", messageId);
                await this.DeletedMessage(messageId);
                var res = await redisServer.StringSetAsync(messageId, Status.COMPLETED.ToString(), TimeSpan.FromDays(7), When.NotExists);
                this.logger.LogInformation("Message with id {MessageId} marked as completed in Redis", messageId);
                return res;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error marking message as completed in Redis");
                throw;
            }
        }

        public async Task<bool> DeletedMessage(string messageId)
        {
            try
            {
                this.logger.LogInformation("Deleting message with id {MessageId} from Redis", messageId);
                var res = await redisServer.KeyDeleteAsync(messageId);
                this.logger.LogInformation("Message with id {MessageId} deleted from Redis", messageId);
                return res;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error deleting message from Redis");
                throw;
            }
        }
    }
}
