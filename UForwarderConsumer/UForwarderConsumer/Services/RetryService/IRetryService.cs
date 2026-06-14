using System;
using System.Collections.Generic;
using System.Text;
using UForwarderConsumer.Dtos;

namespace UForwarderConsumer.Services.RetryService
{
    public interface IRetryService
    {
        /// <summary>
        /// Adds a message to the retry queue, calculating the next retry time based on the retry count and storing it in Redis.
        /// </summary>
        /// <param name="message">The message to be added to the retry queue.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean result indicating success or failure.</returns>
        Task<bool> AddForRetry(ForwarderMessage message);
    }
}
