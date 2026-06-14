using RetryWorker.Dtos;
using RetryWorker.Services.KafkaProducerService;
using RetryWorker.Services.RetryService;

namespace RetryWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;
        private readonly IProducerService producerService;
        private readonly IRetryService retryService;

        public Worker(ILogger<Worker> logger,
            IProducerService producerService,
            IRetryService retryService)
        {
            this.logger = logger;
            this.producerService = producerService;
            this.retryService = retryService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    logger.LogInformation("Retry Scheduler Running...");

                    var messagesToRetry = await retryService.GetAllMessagesToRetry();

                    foreach (var retryMesg in messagesToRetry)
                    {
                        if (retryMesg == null) continue;

                        var forwarderMessage = new ForwarderMessage
                        {
                            MessageId = retryMesg.MessageId,
                            Payload = retryMesg.Payload,
                            RetryCount = retryMesg.RetryCount,
                            EventType = retryMesg.EventType
                        };

                        logger.LogInformation($"Retrying message with ID: {forwarderMessage.MessageId}, Retry Count: {forwarderMessage.RetryCount}");

                        bool isPublished = await producerService.PublishAsync(forwarderMessage);

                        logger.LogInformation($"Message with ID: {forwarderMessage.MessageId} published: {isPublished}");

                        if (isPublished)
                        {
                            await retryService.RemoveFromRetryQueue(retryMesg);
                        }
                    }

                    await Task.Delay(1000, stoppingToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred in the worker.");
                }
            }
        }
    }
}
