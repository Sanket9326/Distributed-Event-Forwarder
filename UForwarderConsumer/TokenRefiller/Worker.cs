using TokenRefiller.Services.TokenRefillService;

namespace TokenRefiller
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;
        private readonly IConfiguration configuration;
        private readonly ITokenRefillService tokenRefillService;

        public Worker(ILogger<Worker> logger, IConfiguration configuration, ITokenRefillService tokenRefillService)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.tokenRefillService = tokenRefillService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                this.logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                var refillInterval = configuration.GetValue<int>("TokenRefill:RefillIntervalSeconds");

                this.logger.LogInformation("Refill interval set to: {interval} seconds", refillInterval);

                await tokenRefillService.RefillTokensForServices();

                this.logger.LogInformation("Token refill completed. Next refill in {interval} seconds.", refillInterval);

                await Task.Delay(refillInterval * 1000, stoppingToken);
            }
        }
    }
}
