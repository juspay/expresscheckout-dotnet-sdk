namespace Juspay
{
    using System.Net;
    using System.Net.Http.Headers;

    /// <summary>
    /// Represents a buffered textual response from Juspay's API.
    /// </summary>
    public class JuspayResponse : JuspayResponseBase
    {
        /// <param name="statusCode">The HTTP status code.</param>
        /// <param name="headers">The HTTP headers of the response.</param>
        /// <param name="content">The body of the response.</param>
        public JuspayResponse(HttpStatusCode statusCode, HttpResponseHeaders headers, string content)
            : base(statusCode, headers)
        {
            this.Content = content;
        }

        /// <summary>Gets the body of the response.</summary>
        /// <value>The body of the response.</value>
        public string Content { get; }
    }
}
