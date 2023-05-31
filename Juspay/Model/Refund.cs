namespace Juspay
{
    using Newtonsoft.Json;
    public class Refund : JuspayResponse
    {
        [JsonProperty("id")]
        public string? Id { get; set; }
        [JsonProperty("unique_request_id")]
        public string? UniqueRequestId { get; set; }
        [JsonProperty("ref")]
        public string? Ref { get; set; }
        [JsonProperty("amount")]
        public double? Amount { get; set; }
        [JsonProperty("created")]
        public string? Created { get; set; }
        [JsonProperty("status")]
        public string? Status { get; set; }
        [JsonProperty("error_message")]
        public string? ErrorMessage { get; set; }
        [JsonProperty("initiated_by")]
        public string? InitiatedBy { get; set; }
        [JsonProperty("sent_to_gateway")]
        public bool? SentToGateway { get; set; }
        [JsonProperty("arn")]
        public string? Arn { get; set; }
        [JsonProperty("internal_reference_id")]
        public string? InternalReferenceId { get; set; }
        [JsonProperty("gateway")]
        public string? Gateway { get; set; }
        [JsonProperty("epg_txn_id")]
        public string? EpgTxnId { get; set; }
        [JsonProperty("authorization_id")]
        public string? AuthorizationId { get; set; }
        [JsonProperty("reference_id")]
        public string? ReferenceId { get; set; }
        [JsonProperty("response_code")]
        public string? ResponseCode { get; set; }
        [JsonProperty("refund_arn")]
        public string? RefundArn { get; set; }
        [JsonProperty("refund_type")]
        public string? RefundType { get; set; }
        [JsonProperty("refund_source")]
        public string? RefundSource { get; set; }
        [JsonProperty("txn_id")]
        public string? TxnId { get; set; }
        [JsonProperty("order_id")]
        public string? OrderId { get; set; }
        [JsonProperty("metadata")]
        public Dictionary<string, object>? Metadata { get; set; }
    }
}