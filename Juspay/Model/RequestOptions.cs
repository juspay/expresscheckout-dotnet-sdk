namespace Juspay {
     using Newtonsoft.Json;
     using System.Net;
     using System.Net.Http;
     public class RequestOptions {
      public RequestOptions(string merchantId, string apiKey, SecurityProtocolType? ssl, TimeSpan? readTimeoutInMilliSeconds) {
         if (merchantId != null) this.MerchantId = merchantId;
         if (apiKey != null) this.ApiKey = apiKey;
         if (ssl.HasValue) this.SSL = ssl.Value;
         if (readTimeoutInMilliSeconds.HasValue) this.ReadTimeoutInMilliSeconds = readTimeoutInMilliSeconds.Value;
      }
        [JsonProperty("x-merchantid")]
        public string MerchantId { get; set; }
        public string ApiKey { get; set; }
        public SecurityProtocolType SSL { get; set; }
        public TimeSpan? ReadTimeoutInMilliSeconds { get; set; }

        public override string ToString()
        {
            return string.Format(
               "<{0} MerchantId={1} TimeSpan={2}",
               this.GetType().FullName,
               this.MerchantId == null ? "not added" : this.MerchantId,
               this.ReadTimeoutInMilliSeconds.HasValue ? "not added" : this.ReadTimeoutInMilliSeconds.Value
            );
        }
     }
}