namespace Juspay
{
    using Newtonsoft.Json;
    public class Upi : JuspayEntity
    {
        [JsonProperty("txn_flow_type")]
        public string? TxnFlowType { get; set; }
        [JsonProperty("masked_bank_account_number")]
        public string? MaskedBankAccountNumber { get; set; }
        [JsonProperty("bank_name")]
        public string? NankName { get; set; }
        [JsonProperty("bank_code")]
        public string? NankCode { get; set; }
        [JsonProperty("upi_bank_account_reference_id")]
        public string? UpiBankAccountReferenceId { get; set; }
        [JsonProperty("payer_vpa")]
        public string? PayerVpa { get; set; }
        [JsonProperty("payer_app")]
        public string? PayerApp { get; set; }
        [JsonProperty("juspay_bank_code")]
        public string? JuspayBankCode { get; set; }
    }
}