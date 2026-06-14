using System;
using System.Collections.Generic;
using System.Text;

namespace UForwarderConsumer.Dtos
{
    /// <summary>
    /// Represents a message to be forwarded, containing an optional message ID, event type, and payload.
    /// </summary>
    public class ForwarderMessage
    {
        /// <summary>
        /// Gets or sets the unique identifier of the message.
        /// </summary>
        public string? MessageId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of the event.
        /// </summary>
        public required string EventType { get; set; }

        /// <summary>
        /// Gets or sets the payload of the message.
        /// </summary>
        public required string Payload { get; set; }
    }
}
