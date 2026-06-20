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

        public async Task<bool> IsAvailableAsync(string messageId)
        {
            try
            {
                var exists = await redisServer.StringGetAsync($"Processing:{messageId}");
                var completed = await redisServer.StringGetAsync($"Completed:{messageId}");

                return exists.IsNullOrEmpty && completed.IsNullOrEmpty;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error checking message id in Redis");
                throw;
            }
        }

        public async Task<bool> AcquireLockAsync(string messageId)
        {
            try
            {
                this.logger.LogInformation("Marking message with id {MessageId} as processing in Redis", messageId);
                var res = await redisServer.StringSetAsync($"Processing:{messageId}", Status.PROCESSING.ToString(), TimeSpan.FromMinutes(1), When.NotExists);
                this.logger.LogInformation("Message with id {MessageId} marked as processing in Redis", messageId);
                return res;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error marking message as processing in Redis");
                throw;
            }
        }

        public async Task<bool> MarkCompletedAsync(string messageId)
        {
            try
            {
                this.logger.LogInformation("Marking message with id {MessageId} as completed in Redis", messageId);
                await this.ReleaseLockAsync(messageId);
                var res = await redisServer.StringSetAsync($"Completed:{messageId}", Status.COMPLETED.ToString(), TimeSpan.FromDays(7));
                this.logger.LogInformation("Message with id {MessageId} marked as completed in Redis", messageId);
                return res;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error marking message as completed in Redis");
                throw;
            }
        }

        public async Task<bool> ReleaseLockAsync(string messageId)
        {
            try
            {
                this.logger.LogInformation("Deleting message with id {MessageId} from Redis", messageId);
                var res = await redisServer.KeyDeleteAsync($"Processing:{messageId}");
                this.logger.LogInformation("Message with id {MessageId} deleted from Redis", messageId);
                return res;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error deleting message from Redis");
                throw;
            }
        }

        public async Task<bool> IsAlreadyCompleted(string messageId) {
            try
            {
                this.logger.LogInformation("Checking if message with id {MessageId} is already completed in Redis", messageId);
                var exists = await redisServer.StringGetAsync($"Completed:{messageId}");
                var isCompleted = !exists.IsNullOrEmpty;
                return isCompleted;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error checking if message is already completed in Redis");
                throw;
            }
        }
    }
}
