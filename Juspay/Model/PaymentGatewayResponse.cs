namespace Juspay
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    public class PaymentGatewayResponse : JuspayResponse
    {
        [JsonProperty("rrn")]
        public string Rrn
        {
            get { return GetValue<string>("rrn"); }
            set { SetValue("rrn", value); }
        }

        [JsonProperty("epg_txn_id")]
        public string EpgTxnId
        {
            get { return GetValue<string>("epg_txn_id"); }
            set { SetValue("epg_txn_id", value); }
        }

        [JsonProperty("auth_id_code")]
        public string AuthIdCode
        {
            get { return GetValue<string>("auth_id_code"); }
            set { SetValue("auth_id_code", value); }
        }

        [JsonProperty("txn_id")]
        public string TxnId
        {
            get { return GetValue<string>("txn_id"); }
            set { SetValue("txn_id", value); }
        }

        [JsonProperty("resp_code")]
        public string RespCode
        {
            get { return GetValue<string>("resp_code"); }
            set { SetValue("resp_code", value); }
        }

        [JsonProperty("resp_message")]
        public string RespMessage
        {
            get { return GetValue<string>("resp_message"); }
            set { SetValue("resp_message", value); }
        }

        [JsonProperty("created")]
        public string Created
        {
            get { return GetValue<string>("created"); }
            set { SetValue("created", value); }
        }

        [JsonProperty("offer")]
        public string Offer
        {
            get { return GetValue<string>("offer"); }
            set { SetValue("offer", value); }
        }

        [JsonProperty("offer_type")]
        public string OfferType
        {
            get { return GetValue<string>("offer_type"); }
            set { SetValue("offer_type", value); }
        }

        [JsonProperty("offer_availed")]
        public string OfferAvailed
        {
            get { return GetValue<string>("offer_availed"); }
            set { SetValue("offer_availed", value); }
        }

        [JsonProperty("discount_amount")]
        public double DiscountAmount
        {
            get { return GetValue<double>("discount_amount"); }
            set { SetValue("discount_amount", value); }
        }

        [JsonProperty("gateway_response")]
        public Dictionary<object, object> GatewayResponse
        {
            get { return GetValue<Dictionary<object, object>>("gateway_response"); }
            set { SetValue("gateway_response", value); }
        }
    }

}