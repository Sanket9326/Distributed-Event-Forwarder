using System;
using System.Collections.Generic;
using System.Text;

namespace UForwarderConsumer.Dtos
{
    public class DlqMessage
    {
        public string MessageId { get; set; } = Guid.NewGuid().ToString();

        public string EventType { get; set; } = string.Empty;

        public string Payload { get; set; } = string.Empty;

        public int RetryCount { get; set; } = 0;

        public DateTime LastTimeStamp { get; set; } = DateTime.UtcNow;
    }
}
