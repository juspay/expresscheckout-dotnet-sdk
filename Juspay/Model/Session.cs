namespace Juspay {
    using Newtonsoft.Json;
    public class SdkPayload : JuspayResponse
    {
        [JsonProperty("requestId")]
        public string RequestId
        {
            get { return GetValue<string>("requestId"); }
            set { SetValue("requestId", value); }
        }

        [JsonProperty("service")]
        public string Service
        {
            get { return GetValue<string>("service"); }
            set { SetValue("service", value); }
        }

        [JsonProperty("payload")]
        public Payload Payload
        {
            get { return GetObject<Payload>("payload"); }
            set { SetValue("payload", value); }
        }

        [JsonProperty("expiry")]
        public string ExpiryDate
        {
            get { return GetValue<string>("expiry"); }
            set { SetValue("expiry", value); }
        }
    }
    public class Payload : JuspayResponse
    {
        [JsonProperty("clientId")]
        public string ClientId
        {
            get { return GetValue<string>("clientId"); }
            set { SetValue("clientId", value); }
        }

        [JsonProperty("amount")]
        public string Amount
        {
            get { return GetValue<string>("amount"); }
            set { SetValue("amount", value); }
        }

        [JsonProperty("merchantId")]
        public string MerchantId
        {
            get { return GetValue<string>("merchantId"); }
            set { SetValue("merchantId", value); }
        }

        [JsonProperty("clientAuthToken")]
        public string ClientAuthToken
        {
            get { return GetValue<string>("clientAuthToken"); }
            set { SetValue("clientAuthToken", value); }
        }

        [JsonProperty("clientAuthTokenExpiry")]
        public string ClientAuthTokenExpiryDate
        {
            get { return GetValue<string>("clientAuthTokenExpiry"); }
            set { SetValue("clientAuthTokenExpiry", value); }
        }

        [JsonProperty("environment")]
        public string Environment
        {
            get { return GetValue<string>("environment"); }
            set { SetValue("environment", value); }
        }

        [JsonProperty("action")]
        public string Action
        {
            get { return GetValue<string>("action"); }
            set { SetValue("action", value); }
        }

        [JsonProperty("customerId")]
        public string CustomerId
        {
            get { return GetValue<string>("customerId"); }
            set { SetValue("customerId", value); }
        }

        [JsonProperty("returnUrl")]
        public string ReturnUrl
        {
            get { return GetValue<string>("returnUrl"); }
            set { SetValue("returnUrl", value); }
        }

        [JsonProperty("currency")]
        public string Currency
        {
            get { return GetValue<string>("currency"); }
            set { SetValue("currency", value); }
        }

        [JsonProperty("customerPhone")]
        public string CustomerPhone
        {
            get { return GetValue<string>("customerPhone"); }
            set { SetValue("customerPhone", value); }
        }

        [JsonProperty("customerEmail")]
        public string CustomerEmail
        {
            get { return GetValue<string>("customerEmail"); }
            set { SetValue("customerEmail", value); }
        }

        [JsonProperty("orderId")]
        public string OrderId
        {
            get { return GetValue<string>("orderId"); }
            set { SetValue("orderId", value); }
        }
    }
    public class SessionResponse : JuspayResponse
    {
        [JsonProperty("status")]
        public string Status
        {
            get { return GetValue<string>("status"); }
            set { SetValue("status", value); }
        }

        [JsonProperty("id")]
        public string Id
        {
            get { return GetValue<string>("id"); }
            set { SetValue("id", value); }
        }

        [JsonProperty("order_id")]
        public string OrderId
        {
            get { return GetValue<string>("order_id"); }
            set { SetValue("order_id", value); }
        }

        [JsonProperty("payment_links")]
        public PaymentLinks PaymentLinks
        {
            get { return GetObject<PaymentLinks>("payment_links"); }
            set { SetValue("payment_links", value); }
        }

        [JsonProperty("sdk_payload")]
        public SdkPayload SdkPayload
        {
            get { return GetObject<SdkPayload>("sdk_payload"); }
            set { SetValue("sdk_payload", value); }
        }

    }
}