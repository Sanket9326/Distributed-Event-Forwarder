namespace UForwarderConsumer.Services.RateLimitingService
{
    public interface IRateLimitingService
    {
        /// <summary>
        /// Checks if the request can be processed based on the rate limits for the given services.
        /// </summary>
        /// <param name="services">The list of services to check the rate limits for.</param>
        /// <returns>True if the request can be processed, false otherwise.</returns>
        Task<bool> CanProcessRequest(List<String> services);
    }
}
