namespace Juspay
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http.Headers;

    public class JuspayResponseBase
    {
        public JuspayResponseBase(int statusCode, HttpResponseHeaders headers, bool isSuccessStatusCode)
        {
            this.StatusCode = statusCode;
            this.Headers = headers;
            this.IsSuccessStatusCode = isSuccessStatusCode;
        }

        public int StatusCode { get; set; }

        public bool IsSuccessStatusCode { get; set; }

        public HttpResponseHeaders Headers { get; set; }

        public DateTimeOffset? Date => this.Headers.Date;

        public string XRequestId => MaybeGetHeader(this.Headers, "x-request-id");

        public string XResponseId => MaybeGetHeader(this.Headers, "x-response-id");

        public string XMerchantId => MaybeGetHeader(this.Headers, "x-jp-merchant-id");

    
        public override string ToString()
        {
            return string.Format(
                "<{0} status={1} Request-Id={2} Response-Id={3} Merchant-Id={4} Headers={5}>",
                this.GetType().FullName,
                (int)this.StatusCode,
                this.XRequestId,
                this.XResponseId,
                this.XMerchantId,
                this.Headers.ToString());
        }

        private static string MaybeGetHeader(HttpHeaders headers, string name)
        {
            if ((headers == null) || (!headers.Contains(name)))
            {
                return null;
            }

            return headers.GetValues(name).First();
        }
    }
}
