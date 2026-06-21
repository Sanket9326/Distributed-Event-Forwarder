using StackExchange.Redis;

namespace TokenRefiller.Services.TokenRefillService.Implementations
{
    public class TokenRefillService : ITokenRefillService
    {
        private readonly ILogger<TokenRefillService> logger;
        private readonly IDatabase redisServer;
        private readonly int MAX_TOKENS; // Maximum tokens allowed for each service
        private readonly int TOKENS_PER_REFILL; // Number of tokens to add during each refill

        public TokenRefillService(IConnectionMultiplexer redis, ILogger<TokenRefillService> logger, IConfiguration configuration)
        {
            this.redisServer = redis.GetDatabase();
            this.logger = logger;

            var tokenRefillConfig = configuration.GetSection("TokenRefill");
            MAX_TOKENS = tokenRefillConfig.GetValue<int>("MaxTokens");
            TOKENS_PER_REFILL = tokenRefillConfig.GetValue<int>("RefillAmount");
        }

        public async Task RefillTokensForServices()
        {
            try
            {
                logger.LogInformation("Starting token refill process for services.");

                var services = await GetAllServicesToRefill();
                await Task.WhenAll(
                        services.Select(async service =>
                        {
                           var tokens = await RefillTokens(service);

                           logger.LogInformation(
                             "Refilled tokens for service {Service}. Current token count: {Tokens}",
                              service,
                              tokens);
                        }));

                logger.LogInformation("Token refill process completed for all services.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while refilling tokens.");
                throw;
            }
        }

        /// <summary>
        /// Refills tokens for a specific service in Redis. This method uses a Lua script to ensure atomicity of the token refill operation. It checks the current token count, adds the specified number of tokens, and ensures that the total does not exceed the maximum allowed tokens.
        /// </summary>
        /// <param name="service">The name of the service for which to refill tokens.</param>
        /// <returns>The updated token count after the refill operation.</returns>
        private async Task<int> RefillTokens(string service)
        {
            var key = (RedisKey)$"rate_limit:{service}";

            const string script = @"
        local tokens = tonumber(redis.call('GET', KEYS[1]))

        if not tokens then
            tokens = ARGV[1]
        end

        tokens = math.min(
            tonumber(ARGV[1]),
            tokens + tonumber(ARGV[2])
        )

        redis.call('SET', KEYS[1], tokens)

        return tokens
    ";

            var result = await redisServer.ScriptEvaluateAsync(
                script,
                new RedisKey[] { key },
                new RedisValue[]
                {
            MAX_TOKENS,
            TOKENS_PER_REFILL
                });

            return (int)result;
        }

        /// <summary>
        /// Retrieves a list of all services that require token refilling. In a real-world application, this method would likely query a configuration or database to determine which services need token management. For demonstration purposes, it returns a hardcoded list of service names.
        /// </summary>
        /// <returns>A list of service names that require token refilling.</returns>
        private async Task<List<string>> GetAllServicesToRefill()
        {
            return new List<string> { "Inventory", "Email" };
        }
    }
}
