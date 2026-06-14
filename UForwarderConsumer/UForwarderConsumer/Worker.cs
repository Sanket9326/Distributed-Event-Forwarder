using Confluent.Kafka;
using System.Text.Json;
using UForwarderConsumer.Dtos;
using UForwarderConsumer.Services;

namespace UForwarderConsumer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private readonly IMessageProcessor messageProcessor;

        public Worker(ILogger<Worker> logger, IConfiguration configuration, IMessageProcessor messageProcessor)
        {
            _logger = logger;
            _configuration = configuration;
            this.messageProcessor = messageProcessor;
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
                        await messageProcessor.ProcessMessage(deserializedMessage);
                    }

                    _logger.LogInformation("Consumed message: {Message}", result.Message.Value);

                    await Task.Delay(1000, stoppingToken);
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
