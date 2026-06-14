using Confluent.Kafka;
using RetryWorker.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace RetryWorker.Services.KafkaProducerService.Implementations
{
    public class ProducerService : IProducerService
    {
        private readonly IProducer<string, string> producer;
        private readonly IConfiguration configuration;
        private readonly ILogger<ProducerService> logger;

        public ProducerService(IProducer<string, string> producer,
        IConfiguration configuration,
        ILogger<ProducerService> logger)
        {
            this.producer = producer;
            this.configuration = configuration;
            this.logger = logger;
        }

        public async Task<bool> PublishAsync(ForwarderMessage message)
        {
            try
            {
                string topic = configuration["Kafka:Topic"]!;

                string json = JsonSerializer.Serialize(message);

                await producer.ProduceAsync(
                    topic,
                    new Message<string, string>
                    {
                        Key = message.MessageId ?? Guid.NewGuid().ToString(),
                        Value = json
                    });
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error publishing message to Kafka");
                return false;
            }
        }
    }
}
