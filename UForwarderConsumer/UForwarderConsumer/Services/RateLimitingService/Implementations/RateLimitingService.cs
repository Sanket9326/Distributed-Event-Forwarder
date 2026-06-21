using StackExchange.Redis;

namespace UForwarderConsumer.Services.RateLimitingService.Implementations
{
    public class RateLimitingService : IRateLimitingService
    {
        private readonly ILogger<RateLimitingService> logger;
        private readonly IDatabase redis;

        public RateLimitingService(IConnectionMultiplexer redis, ILogger<RateLimitingService> logger)
        {
            this.redis = redis.GetDatabase();
            this.logger = logger;
        }

        public async Task<bool> CanProcessRequest(List<String> services)
        {
            try
            {
                var keys = services
                    .Select(x => (RedisKey)$"rate_limit:{x}")
                    .ToArray();

                var script = @"
                       for i = 1, #KEYS do
                          local tokens = tonumber(redis.call('GET', KEYS[i]))

                          if not tokens or tokens <= 0 then
                             return 0
                          end
                        end

                        for i = 1, #KEYS do
                           redis.call('DECR', KEYS[i])
                        end

                        return 1";

                var result = (int)await redis.ScriptEvaluateAsync(
                    script,
                    keys);

                return result == 1;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while checking rate limit.");
                throw;
            }
        }
    }
}
