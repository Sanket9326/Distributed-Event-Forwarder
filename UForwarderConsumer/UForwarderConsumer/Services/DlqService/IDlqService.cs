using System;
using System.Collections.Generic;
using System.Text;
using UForwarderConsumer.Dtos;

namespace UForwarderConsumer.Services.DlqService
{
    public interface IDlqService
    {
        /// <summary>
        /// Add the message to the Dead Letter Queue (DLQ) for further analysis and troubleshooting.
        /// </summary>
        /// <param name="message">The message to be added to the DLQ.</param>
        /// <returns>Returns true if the message was successfully added to the DLQ, otherwise false.</returns>
        Task<bool> AddToDLq(ForwarderMessage message);
    }
}
