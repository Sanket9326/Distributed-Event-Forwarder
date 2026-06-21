namespace TokenRefiller.Services.TokenRefillService
{
    public interface ITokenRefillService
    {
        /// <summary>
        /// Refills tokens for all services that require token refilling. This method should be called periodically to ensure that services have sufficient tokens for processing requests.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task RefillTokensForServices();
    }
}
