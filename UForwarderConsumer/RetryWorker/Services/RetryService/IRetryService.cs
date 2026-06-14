using RetryWorker.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RetryWorker.Services.RetryService
{
    public interface IRetryService
    {
        /// <summary>
        /// Retrieves all messages from the retry queue that are due for retrying, based on their scheduled retry time, and returns them as a list of RetryMessage objects.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, with a list of RetryMessage objects as the result.</returns>
        Task<List<RetryMessage>> GetAllMessagesToRetry();

        /// <summary>
        /// Removes a message from the retry queue in Redis.
        /// </summary>
        /// <param name="message">The message to be removed from the retry queue.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task RemoveFromRetryQueue(RetryMessage message);
    }
}
