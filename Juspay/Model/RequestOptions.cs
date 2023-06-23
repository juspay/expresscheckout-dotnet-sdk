namespace Juspay {
     using Newtonsoft.Json;
     using System.Net;
     using System.Net.Http;
     using System;
     public class RequestOptions {
      public RequestOptions(string merchantId, string apiKey, SecurityProtocolType? ssl, long? readTimeoutInMilliSeconds, IJuspayJWT juspayJWT) {
         if (merchantId != null) this.MerchantId = merchantId;
         if (apiKey != null) this.ApiKey = apiKey;
         if (ssl.HasValue) this.SSL = ssl.Value;
         if (readTimeoutInMilliSeconds.HasValue) this.ReadTimeoutInMilliSeconds = readTimeoutInMilliSeconds.Value;
         if (juspayJWT != null) this.JuspayJWT = juspayJWT;
      }
        [JsonProperty("x-merchantid")]
        public string MerchantId { get; set; }
        public string ApiKey { get; set; }
        public SecurityProtocolType SSL { get; set; }
        private TimeSpan readTimeout; 
        public long ReadTimeoutInMilliSeconds {
            get => readTimeout.Milliseconds;
            set => readTimeout = TimeSpan.FromTicks(value);
         }

         public TimeSpan ReadTimeout {
            get => readTimeout;
            set => readTimeout = value;
         }

         public IJuspayJWT JuspayJWT { get; set; }

        public override string ToString()
        {
            return string.Format(
               "<{0} MerchantId={1} TimeSpan={2}",
               this.GetType().FullName,
               this.MerchantId == null ? "not added" : this.MerchantId,
               this.readTimeout.ToString() != TimeSpan.Zero.ToString() ? "not added" : this.readTimeout.ToString()
            );
        }
     }
}