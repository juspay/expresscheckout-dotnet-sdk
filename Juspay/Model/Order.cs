namespace Juspay {
    using Newtonsoft.Json;
    public class OrderResponse : JuspayResponse
    {
        [JsonProperty("id")]
        private string Id { get; set; }
        [JsonProperty("order_id")]
        private string OrderId { get; set; }
        [JsonProperty("merchant_id")]
        private string MerchantId { get; set; }
        [JsonProperty("txn_id")]
        private string TxnId { get; set; }
        [JsonProperty("amount")]
        private double Amount { get; set; }
        [JsonProperty("effective_amount")]
        private double EffectiveAmount { get; set; }
        [JsonProperty("resp_code")]
        private string RespCode { get; set; }
        [JsonProperty("resp_message")]
        private string RespMessage { get; set; }
        [JsonProperty("currency")]
        private string Currency { get; set; }
        [JsonProperty("customer_id")]
        private string CustomerId { get; set; }
        [JsonProperty("customer_email")]
        private string CustomerEmail { get; set; }
        [JsonProperty("customer_phone")]
        private string CustomerPhone { get; set; }
        [JsonProperty("description")]
        private string Description { get; set; }
        [JsonProperty("product_id")]
        private string ProductId { get; set; }
        [JsonProperty("gateway_id")]
        private long GatewayId { get; set; }
        [JsonProperty("return_url")]
        private string ReturnUrl { get; set; }
        [JsonProperty("status")]
        private string Status { get; set; }
        [JsonProperty("status_id")]
        private long StatusId { get; set; }
        [JsonProperty("gateway_reference_id")]
        private string GatewayReferenceId { get; set; }

    }
}