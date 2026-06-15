using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using UForwarderConsumer.Dtos;

namespace UForwarderConsumer.Services.DlqService.Implementations
{
    public class DlqService : IDlqService
    {
        private readonly IProducer<string, string> producer;
        private readonly IConfiguration configuration;
        private readonly ILogger<DlqService> logger;

        public DlqService(IProducer<string, string> producer, IConfiguration configuration, ILogger<DlqService> logger)
        {
            this.producer = producer;
            this.configuration = configuration;
            this.logger = logger;
        }

        public async Task<bool> AddToDLq(ForwarderMessage message)
        {
            try
            {
                logger.LogInformation("Adding message with ID {MessageId} to DLQ", message.MessageId);

                var topic = configuration["Kafka:DlqTopic"]!;

                var dlqMessage = new DlqMessage
                {
                    MessageId = message.MessageId ?? Guid.NewGuid().ToString(),
                    EventType = message.EventType,
                    Payload = JsonSerializer.Serialize(message),
                    RetryCount = message.RetryCount,
                    LastTimeStamp = DateTime.UtcNow
                };

                var messageValue = JsonSerializer.Serialize(dlqMessage);

                await producer.ProduceAsync(topic, new Message<string, string>
                {
                    Key = dlqMessage.MessageId,
                    Value = messageValue
                });

                logger.LogInformation("Message with ID {MessageId} added to DLQ", dlqMessage.MessageId);

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error adding message to DLQ");
                return false;
            }
        }
    }
}
