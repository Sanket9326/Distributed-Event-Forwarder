using System;
using System.Collections.Generic;
using System.Text;
using UForwarderConsumer.Dtos;
using UForwarderConsumer.Services.RateLimitingService;

namespace UForwarderConsumer.Services.MessageProcessingService.Implementations
{
    public class MessageProcessor : IMessageProcessor
    {
        private readonly IRoutineEngine routineEngine;
        private readonly ILogger<MessageProcessor> logger;
        private readonly IRateLimitingService rateLimitingService;

        public MessageProcessor(IRoutineEngine routineEngine, ILogger<MessageProcessor> logger, IRateLimitingService rateLimitingService)
        {
            this.routineEngine = routineEngine;
            this.logger = logger;
            this.rateLimitingService = rateLimitingService;
        }

        public async Task ProcessMessage(ForwarderMessage message)
        {
            try
            {
                List<string> destinations = await routineEngine.GetDestinations(message.EventType);

                if (destinations == null || destinations.Count == 0)
                {
                    logger.LogWarning("No destinations found for event type: {EventType}", message.EventType);
                    return;
                }

                if (!await rateLimitingService.CanProcessRequest(destinations))
                {
                    logger.LogWarning("Rate limit exceeded for event type: {EventType}. Message will be retried later.", message.EventType);
                    throw new Exception("Rate limit exceeded.");
                }

                // Process the message for each destination
                foreach (var destination in destinations)
                {
                    Console.WriteLine($"Processing message : {message.Payload} for destination: {destination}");
                }

                logger.LogInformation("Message processed successfully for {Destinations}", string.Join(", ", destinations));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing message");
                throw;
            }
        }
    }
}
