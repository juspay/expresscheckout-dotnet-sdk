namespace Juspay {
    using Newtonsoft.Json;
    public class SdkPayload : JuspayResponse
    {
        [JsonProperty("requestId")]
        public string RequestId { get; set; }
        [JsonProperty("service")]
        public string Service { get; set; }
        [JsonProperty("payload")]
        public Payload Payload { get; set; }
        [JsonProperty("expiry")]
        public string ExpiryDate { get; set; }
    }
    public class Payload : JuspayResponse
    {
        [JsonProperty("clientId")]
        public string ClientId { get; set; }
        [JsonProperty("amount")]
        public string Amount { get; set; }
        [JsonProperty("merchantId")]
        public string MerchantId { get; set; }
        [JsonProperty("clientAuthToken")]
        public string ClientAuthToken { get; set; }
        [JsonProperty("clientAuthTokenExpiry")]
        public string ClientAuthTokenExpiryDate { get; set; }
        [JsonProperty("environment")]
        public string Environment { get; set; }
        [JsonProperty("action")]
        public string Action { get; set; }
        [JsonProperty("customerId")]
        public string CustomerId { get; set; }
        [JsonProperty("returnUrl")]
        public string ReturnUrl { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
        [JsonProperty("customerPhone")]
        public string CustomerPhone { get; set; }
        [JsonProperty("customerEmail")]
        public string CustomerEmail { get; set; }
        [JsonProperty("orderId")]
        public string OrderId { get; set; }
    }
    public class SessionResponse : JuspayResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("order_id")]
        public string OrderId { get; set; }
        [JsonProperty("payment_links")]
        public PaymentLinks PaymentLinks { get; set; }
        [JsonProperty("sdk_payload")]
        public SdkPayload SdkPayload { get; set; }
    }
}