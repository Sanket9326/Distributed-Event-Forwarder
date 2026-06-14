using System;
using System.Collections.Generic;
using System.Text;
using UForwarderConsumer.Dtos;

namespace UForwarderConsumer.Services.Implementations
{
    public class MessageProcessor : IMessageProcessor
    {
        private readonly IRoutineEngine routineEngine;
        private readonly ILogger<MessageProcessor> logger;

        public MessageProcessor(IRoutineEngine routineEngine, ILogger<MessageProcessor> logger)
        {
            this.routineEngine = routineEngine;
            this.logger = logger;
        }

        public async Task ProcessMessage(ForwarderMessage message)
        {
            try
            {
                List<string> destinations = await routineEngine.GetDestinations(message.EventType);

                // Process the message for each destination
                foreach (var destination in destinations)
                {
                    Console.WriteLine($"Processing message : {message.Payload} for destination: {destination}");
                }

                logger.LogInformation("Message processed successfully for {Destinations}", string.Join(", ", destinations));
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                logger.LogError(ex, "Error processing message");
            }
        }
    }
}
