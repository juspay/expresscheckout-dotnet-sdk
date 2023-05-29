namespace Juspay
{
    using System.Net;
    using System.Net.Http.Headers;

    public class JuspayResponse : JuspayResponseBase
    {
        public JuspayResponse(HttpStatusCode statusCode, HttpResponseHeaders headers, bool isSuccessStatusCode, string content)
            : base(statusCode, headers, isSuccessStatusCode)
        {
            this.Content = content;
        }

        public string Content { get; }
    }
}
