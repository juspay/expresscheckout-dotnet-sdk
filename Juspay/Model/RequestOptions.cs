namespace Juspay {
     using Newtonsoft.Json;
     using System.Net;
     using System.Net.Http;
     using System;
     public class RequestOptions {

      public RequestOptions() {
         this.JuspayJWT = JuspayEnvironment.Instance.JuspayJWT;
         this.MerchantId = JuspayEnvironment.Instance.MerchantId;
      }
      public RequestOptions(string merchantId, string apiKey, SecurityProtocolType? ssl, long? readTimeoutInMilliSeconds, IJuspayJWT juspayJWT = null) {
         if (merchantId != null) this.MerchantId = merchantId;
         else this.MerchantId = JuspayEnvironment.Instance.MerchantId;
         if (apiKey != null) this.ApiKey = apiKey;
         if (ssl.HasValue) this.SSL = ssl.Value;
         if (readTimeoutInMilliSeconds.HasValue) this.ReadTimeoutInMilliSeconds = readTimeoutInMilliSeconds.Value;
         if (juspayJWT != null) this.juspayJWT = juspayJWT;
         else this.juspayJWT = JuspayEnvironment.Instance.JuspayJWT;
      }
         private string merchantId;

         [JsonProperty("x-merchantid")]
        public string MerchantId { get { return this.merchantId; } set {
         if (value == null) this.merchantId = JuspayEnvironment.Instance.MerchantId;
         else this.merchantId = value;
        } }
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

         private IJuspayJWT juspayJWT;
         public IJuspayJWT JuspayJWT { get { return this.juspayJWT; } set {
            if(value == null) this.juspayJWT = JuspayEnvironment.Instance.JuspayJWT;
            else this.juspayJWT = value;
         } }
         [JsonProperty("x-customerid")]
         public string CustomerId { get; set; }
        public override string ToString()
        {
           return JsonConvert.SerializeObject(
                this,
                Formatting.Indented);
        }
     }
}