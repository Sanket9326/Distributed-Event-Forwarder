using System;
using System.Collections.Generic;
using System.Text;

namespace UForwarderConsumer.Services.MessageProcessingService.Implementations
{
    public class RoutineEngine:IRoutineEngine
    {
        public async Task<List<string>> GetDestinations(string eventType)
        {
            if (string.IsNullOrEmpty(eventType))
            {
                throw new ArgumentNullException("eventType", "Event type cannot be null or empty.");
            }

            switch (eventType) {

                case "order.created":
                    return new List<string> { "Intventory", "Email" };
                case "order.placed":
                    return new List<string> { "Intventory", "Email" };
                default:
                    return new List<string>();
            }

        }
    }
}
