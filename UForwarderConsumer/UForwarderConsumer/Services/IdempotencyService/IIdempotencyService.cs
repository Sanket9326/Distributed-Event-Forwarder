namespace UForwarderConsumer.Services.IdempotencyService
{
    public interface IIdempotencyService
    {
        /// <summary>
        /// Marks the message as processing in Redis with a TTL of 5 minutes. This indicates that the message is currently being processed and prevents other instances from processing the same message concurrently.
        /// </summary>
        /// <param name="messageId">Unique identifier of the message.</param>
        /// <returns>True if the message is available to process, otherwise false.</returns>
        Task<bool> IsAvailableAsync(string messageId);

        /// <summary>
        /// Marks the message as processing in Redis with a TTL of 5 minutes. This indicates that the message is currently being processed and prevents other instances from processing the same message concurrently.
        /// </summary>
        /// <param name="messageId">Unique identifier of the message.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<bool> AcquireLockAsync(string messageId);

        /// <summary>
        /// Marks the message as completed in Redis with a TTL of 7 days. This indicates that the message has been successfully processed and prevents it from being processed again in the future.
        /// </summary>
        /// <param name="messageId">Unique identifier of the message.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<bool> MarkCompletedAsync(string messageId);

        /// <summary>
        /// Deletes the message from Redis. This can be used to clean up entries for messages that have been processed or are no longer needed, ensuring that Redis does not retain unnecessary data and helps maintain optimal performance.
        /// </summary>
        /// <param name="messageId">Unique identifier of the message.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<bool> ReleaseLockAsync(string messageId);

        /// <summary>
        /// Checks if the message with the given messageId has already been marked as completed in Redis. This is useful to prevent reprocessing of messages that have already been successfully processed, ensuring idempotency in the message processing workflow.
        /// </summary>
        /// <param name="messageId">Unique identifier of the message.</param>
        /// <returns>True if the message has already been completed, otherwise false.</returns>
        Task<bool> IsAlreadyCompleted(string messageId);
    }
}
