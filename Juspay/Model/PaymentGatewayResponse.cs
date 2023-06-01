namespace Juspay
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    public class PaymentGatewayResponse : JuspayResponse
    {
        [JsonProperty("rrn")]
        public string Rrn { get; set; }
        [JsonProperty("epg_txn_id")]
        public string EpgTxnId { get; set; }
        [JsonProperty("auth_id_code")]
        public string AuthIdCode { get; set; }
        [JsonProperty("txn_id")]
        public string TxnId { get; set; }
        [JsonProperty("resp_code")]
        public string RespCode { get; set; }
        [JsonProperty("resp_message")]
        public string RespMessage { get; set; }
        [JsonProperty("created")]
        public string Created { get; set; }
        [JsonProperty("offer")]
        public string Offer { get; set; }
        [JsonProperty("offer_type")]
        public string OfferType { get; set; }
        [JsonProperty("offer_availed")]
        public string OfferAvailed { get; set; }
        [JsonProperty("discount_amount")]
        public double DiscountAmount { get; set; }
        [JsonProperty("gateway_response")]
        public Dictionary<object, object> GatewayResponse { get; set; }
    }
}