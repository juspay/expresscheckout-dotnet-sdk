namespace Juspay {
     using Newtonsoft.Json;
     using System.Net;
     using System.Net.Http;
     public class RequestOptions {
        [JsonProperty("x-merchantid")]
        public string? MerchantId { get; set; }
        public string? ApiKey { get; set; }
        public SecurityProtocolType? SSL { get; set; }
        public TimeSpan? ReadTimeoutInMilliSeconds { get; set; }
     }
}