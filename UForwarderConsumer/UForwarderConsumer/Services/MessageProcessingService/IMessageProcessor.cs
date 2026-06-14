using System;
using System.Collections.Generic;
using System.Text;
using UForwarderConsumer.Dtos;

namespace UForwarderConsumer.Services.MessageProcessingService
{
    public interface IMessageProcessor
    {
        /// <summary>
        /// Processes the given message asynchronously.
        /// </summary>
        /// <param name="message">The message to be processed.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task ProcessMessage(ForwarderMessage message);
    }
}
