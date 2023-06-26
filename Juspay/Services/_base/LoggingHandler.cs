#if NET5_0_OR_GREATER
namespace Juspay
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    public class LoggingHandler : DelegatingHandler
    {
        private readonly ILogger _logger;

        public LoggingHandler(HttpMessageHandler innerHandler, ILogger logger)
            : base(innerHandler)
        {
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            
            _logger.LogInformation($"Sending HTTP request: {request.Method} {request.RequestUri}");

            if (request.Content != null)
            {
                string requestBody = await request.Content.ReadAsStringAsync();
                _logger.LogInformation($"Request Body: {requestBody}");
            }

            // Call the inner handler to send the request
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            // Log the response details

            if (response.Content != null)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"status_code: {(int)response.StatusCode}, Body: {responseBody}");
            }
            else
            {
                _logger.LogInformation($"Received HTTP response:  {response.ReasonPhrase}");
            }

            return response;
        }
    }
}
#endif
