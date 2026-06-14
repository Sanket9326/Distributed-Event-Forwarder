namespace RetryWorker.Dtos
{
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

        /// <summary>
        /// Gets or sets the number of times the message has been retried.
        /// </summary>
        public required int RetryCount { get; set; }
    }
}
