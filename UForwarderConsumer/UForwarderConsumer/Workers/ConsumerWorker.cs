using Confluent.Kafka;
using System.Text.Json;
using UForwarderConsumer.Dtos;
using UForwarderConsumer.Services.MessageProcessingService;
using UForwarderConsumer.Services.RetryService;

namespace UForwarderConsumer.Workers
{
    public class ConsumerWorker : BackgroundService
    {
        private readonly ILogger<ConsumerWorker> _logger;
        private readonly IConfiguration _configuration;
        private readonly IMessageProcessor messageProcessor;
        private readonly int MAX_RETRY_COUNT = 4;
        private readonly IRetryService retryService;

        public ConsumerWorker(ILogger<ConsumerWorker> logger,
            IConfiguration configuration,
            IMessageProcessor messageProcessor,
            IRetryService retryService)
        {
            _logger = logger;
            _configuration = configuration;
            this.messageProcessor = messageProcessor;
            this.retryService = retryService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _configuration["Kafka:BootstrapServers"],
                GroupId = _configuration["Kafka:GroupId"],
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            var consumer = new ConsumerBuilder<string, string>(config).Build();

            consumer.Subscribe(_configuration["Kafka:Topic"]);

            _logger.LogInformation("Kafka consumer started.");

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var result = consumer.Consume(stoppingToken);

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    ForwarderMessage? deserializedMessage = JsonSerializer.Deserialize<ForwarderMessage>(result.Message.Value, options);

                    if (deserializedMessage != null)
                    {
                        try
                        {
                            await messageProcessor.ProcessMessage(deserializedMessage);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error processing message: {Message}", result.Message.Value);
                            if (deserializedMessage.RetryCount < MAX_RETRY_COUNT)
                            {
                                await this.retryService.AddForRetry(deserializedMessage);
                                _logger.LogInformation("Message will be retried. Current retry count: {RetryCount}", deserializedMessage.RetryCount);
                            }
                            else
                            {
                                _logger.LogWarning("Max retry count reached for message: {Message}. It will not be retried.", result.Message.Value);
                            }
                        }

                        _logger.LogInformation("Consumed message: {Message}", result.Message.Value);

                        await Task.Delay(1000, stoppingToken);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consuming messages.");
            }
            finally
            {
                consumer.Close();
                _logger.LogInformation("Kafka consumer stopped.");
            }
        }
    }
}
