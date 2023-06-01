namespace Juspay
{
    using Newtonsoft.Json;
    public class TxnDetail : JuspayResponse
    {
        [JsonProperty("txn_id")]
        public string TxnId { get; set; }
        [JsonProperty("order_id")]
        public string OrderId { get; set; }
        [JsonProperty("txn_uuid")]
        public string TxnUuid { get; set; }
        [JsonProperty("gateway_id")]
        public string GatewayId { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("gateway")]
        public string Gateway { get; set; }
        [JsonProperty("express_checkout")]
        public bool ExpressCheckout { get; set; }
        [JsonProperty("redirect")]
        public bool Redirect { get; set; }
        [JsonProperty("net_amount")]
        public double NetAmount { get; set; }
        [JsonProperty("surcharge_amount")]
        public double SurchargeAmount { get; set; }
        [JsonProperty("tax_amount")]
        public double TaxAmount { get; set; }
        [JsonProperty("txn_amount")]
        public double TxnAmount { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
        [JsonProperty("error_message")]
        public string ErrorMessage { get; set; }
        [JsonProperty("error_code")]
        public string ErrorCode { get; set; }
        [JsonProperty("created")]
        public string Created { get; set; }
        [JsonProperty("txn_object_type")]
        public string TxnObjectType { get; set; }
        [JsonProperty("source_object")]
        public string SourceObject { get; set; }
        [JsonProperty("source_object_id")]
        public string SourceObjectId { get; set; }
        [JsonProperty("is_conflicted")]
        public bool IsConflicted { get; set; }
        [JsonProperty("is_emi")]
        public bool IsEmi { get; set; }
        [JsonProperty("emi_tenure")]
        public int EmiTenure { get; set; }
        [JsonProperty("emi_bank")]
        public string EmiBank { get; set; }
        [JsonProperty("refunded_amount")]
        public double RefundedAmount { get; set; }
        [JsonProperty("refunded_entirely")]
        public bool RefundedEntirely { get; set; }
        [JsonProperty("txn_card_info")]
        public TxnCardInfo TxnCardInfo { get; set; }
        [JsonProperty("payment_gateway_response")]
        public PaymentGatewayResponse PaymentGatewayResponse { get; set; }

    }
}