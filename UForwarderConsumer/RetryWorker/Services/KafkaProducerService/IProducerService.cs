using RetryWorker.Dtos;

namespace RetryWorker.Services.KafkaProducerService
{
    public interface IProducerService
    {
        /// <summary>
        /// Publishes a message to Kafka asynchronously.
        /// </summary>
        /// <param name="message">The message to be published.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the message was successfully published.</returns>
        Task<bool> PublishAsync(ForwarderMessage message);
    }
}
