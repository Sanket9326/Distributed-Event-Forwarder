using System;
using System.Collections.Generic;
using System.Text;

namespace RetryWorker.Dtos
{
    public class RetryMessage
    {
        public string MessageId { get; set; } = string.Empty;

        public string EventType { get; set; } = string.Empty;

        public string Payload { get; set; } = string.Empty;

        public int RetryCount { get; set; } = 0;

        public DateTime NextRetryAt { get; set; }
    }
}
