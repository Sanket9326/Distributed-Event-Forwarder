using System;
using System.Collections.Generic;
using System.Text;

namespace UForwarderConsumer.Services
{
    public interface IRoutineEngine
    {
        /// <summary>
        /// Gets the list of destinations for a given event type asynchronously.
        /// </summary>
        /// <param name="eventType">The type of the event for which to get destinations.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of destinations.</returns>
        Task<List<string>> GetDestinations(string eventType);
    }
}
