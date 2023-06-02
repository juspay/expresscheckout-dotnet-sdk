namespace Juspay {
    using Newtonsoft.Json;
    public class OrderResponse : JuspayResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("order_id")]
        public string OrderId { get; set; }
        [JsonProperty("merchant_id")]
        public string MerchantId { get; set; }
        [JsonProperty("txn_id")]
        public string TxnId { get; set; }
        [JsonProperty("amount")]
        public double Amount { get; set; }
        [JsonProperty("effective_amount")]
        public double EffectiveAmount { get; set; }
        [JsonProperty("resp_code")]
        public string RespCode { get; set; }
        [JsonProperty("resp_message")]
        public string RespMessage { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
        [JsonProperty("customer_id")]
        public string CustomerId { get; set; }
        [JsonProperty("customer_email")]
        public string CustomerEmail { get; set; }
        [JsonProperty("customer_phone")]
        public string CustomerPhone { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("product_id")]
        public string ProductId { get; set; }
        [JsonProperty("gateway_id")]
        public long GatewayId { get; set; }
        [JsonProperty("return_url")]
        public string ReturnUrl { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("status_id")]
        public long StatusId { get; set; }
        [JsonProperty("gateway_reference_id")]
        public string GatewayReferenceId { get; set; }

    }
}