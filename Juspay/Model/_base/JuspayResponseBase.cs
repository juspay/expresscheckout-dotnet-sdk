namespace Juspay
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http.Headers;

    /// <summary>
    /// Represents a response from Juspay's API.
    /// </summary>
    public abstract class JuspayResponseBase
    {
        /// <param name="statusCode">The HTTP status code.</param>
        /// <param name="headers">The HTTP headers of the response.</param>
        public JuspayResponseBase(HttpStatusCode statusCode, HttpResponseHeaders headers)
        {
            this.StatusCode = statusCode;
            this.Headers = headers;
        }

        /// <summary>Gets the HTTP status code of the response.</summary>
        /// <value>The HTTP status code of the response.</value>
        public HttpStatusCode StatusCode { get; }

        /// <summary>Gets the HTTP headers of the response.</summary>
        /// <value>The HTTP headers of the response.</value>
        public HttpResponseHeaders Headers { get; }

        /// <summary>Gets the date of the request, as returned by Juspay.</summary>
        /// <value>The date of the request, as returned by Juspay.</value>
        public DateTimeOffset? Date => this.Headers?.Date;

        /// <summary>Gets the idempotency key of the request, as returned by Juspay.</summary>
        /// <value>The idempotency key of the request, as returned by Juspay.</value>
        public string? RequestId => MaybeGetHeader(this.Headers, "x-request-id");

        /// <summary>Gets the ID of the request, as returned by Juspay.</summary>
        /// <value>The ID of the request, as returned by Juspay.</value>
        public string? ResponseId => MaybeGetHeader(this.Headers, "x-response-id");

        public string? MerchantId => MaybeGetHeader(this.Headers, "x-jp-merchant-id");

        internal int NumRetries { get; set; }

        public override string ToString()
        {
            return string.Format(
                "<{0} status={1} Request-Id={2} Response-Id={3} Merchant-Id={4} Date={5}>",
                this.GetType().FullName,
                (int)this.StatusCode,
                this.RequestId,
                this.ResponseId,
                this.MerchantId,
                this.Date?.ToString("s"));
        }

        private static string? MaybeGetHeader(HttpHeaders headers, string name)
        {
            if ((headers == null) || (!headers.Contains(name)))
            {
                return null;
            }

            return headers.GetValues(name).First();
        }
    }
}
