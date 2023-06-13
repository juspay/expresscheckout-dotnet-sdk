namespace Juspay {
    using Newtonsoft.Json;
    using System.Collections.Generic;
    public class OrderResponse : JuspayResponse
    {
        [JsonProperty("id")]
        public string Id
        {
            get { return GetValue<string>("id"); }
            set { SetValue("id", value); }
        }

        [JsonProperty("payment_links")]
        public PaymentLinks PaymentLinks
        {
            get { return GetObject<PaymentLinks>("payment_links"); }
            set { SetValue("payment_links", value); }
        }
        
        [JsonProperty("order_id")]
        public string OrderId
        {
            get { return GetValue<string>("order_id"); }
            set { SetValue("order_id", value); }
        }

        [JsonProperty("merchant_id")]
        public string MerchantId
        {
            get { return GetValue<string>("merchant_id"); }
            set { SetValue("merchant_id", value); }
        }

        [JsonProperty("txn_id")]
        public string TxnId
        {
            get { return GetValue<string>("txn_id"); }
            set { SetValue("txn_id", value); }
        }

        [JsonProperty("amount")]
        public double Amount
        {
            get { return GetValue<double>("amount"); }
            set { SetValue("amount", value); }
        }

        [JsonProperty("effective_amount")]
        public double EffectiveAmount
        {
            get { return GetValue<double>("effective_amount"); }
            set { SetValue("effective_amount", value); }
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

        [JsonProperty("currency")]
        public string Currency
        {
            get { return GetValue<string>("currency"); }
            set { SetValue("currency", value); }
        }

        [JsonProperty("customer_id")]
        public string CustomerId
        {
            get { return GetValue<string>("customer_id"); }
            set { SetValue("customer_id", value); }
        }

        [JsonProperty("customer_email")]
        public string CustomerEmail
        {
            get { return GetValue<string>("customer_email"); }
            set { SetValue("customer_email", value); }
        }

        [JsonProperty("customer_phone")]
        public string CustomerPhone
        {
            get { return GetValue<string>("customer_phone"); }
            set { SetValue("customer_phone", value); }
        }

        [JsonProperty("description")]
        public string Description
        {
            get { return GetValue<string>("description"); }
            set { SetValue("description", value); }
        }

        [JsonProperty("product_id")]
        public string ProductId
        {
            get { return GetValue<string>("product_id"); }
            set { SetValue("product_id", value); }
        }

        [JsonProperty("gateway_id")]
        public long GatewayId
        {
            get { return GetValue<long>("gateway_id"); }
            set { SetValue("gateway_id", value); }
        }

        [JsonProperty("return_url")]
        public string ReturnUrl
        {
            get { return GetValue<string>("return_url"); }
            set { SetValue("return_url", value); }
        }

        [JsonProperty("status")]
        public string Status
        {
            get { return GetValue<string>("status"); }
            set { SetValue("status", value); }
        }

        [JsonProperty("status_id")]
        public long StatusId
        {
            get { return GetValue<long>("status_id"); }
            set { SetValue("status_id", value); }
        }

        [JsonProperty("gateway_reference_id")]
        public string GatewayReferenceId
        {
            get { return GetValue<string>("gateway_reference_id"); }
            set { SetValue("gateway_reference_id", value); }
        }
    }

    public class RefundResponse : JuspayResponse
    {
        [JsonProperty("unique_request_id")]
        public string UniqueRequestId
        {
            get { return GetValue<string>("unique_request_id"); }
            set { SetValue("unique_request_id", value); }
        }

        [JsonProperty("txn_id")]
        public string TxnId
        {
            get { return GetValue<string>("txn_id"); }
            set { SetValue("txn_id", value); }
        }

        [JsonProperty("status")]
        public string Status
        {
            get { return GetValue<string>("status"); }
            set { SetValue("status", value); }
        }

        [JsonProperty("sent_to_gateway")]
        public bool SentToGateway
        {
            get { return GetValue<bool>("sent_to_gateway"); }
            set { SetValue("sent_to_gateway", value); }
        }

        [JsonProperty("response_code")]
        public string ResponseCode
        {
            get { return GetValue<string>("response_code"); }
            set { SetValue("response_code", value); }
        }

        [JsonProperty("refund_type")]
        public string RefundType
        {
            get { return GetValue<string>("refund_type"); }
            set { SetValue("refund_type", value); }
        }

        [JsonProperty("refund_source")]
        public string RefundSource
        {
            get { return GetValue<string>("refund_source"); }
            set { SetValue("refund_source", value); }
        }

        [JsonProperty("refund_arn")]
        public string RefundArn
        {
            get { return GetValue<string>("refund_arn"); }
            set { SetValue("refund_arn", value); }
        }

        [JsonProperty("reference_id")]
        public string ReferenceId
        {
            get { return GetValue<string>("reference_id"); }
            set { SetValue("reference_id", value); }
        }

        [JsonProperty("order_id")]
        public string OrderId
        {
            get { return GetValue<string>("order_id"); }
            set { SetValue("order_id", value); }
        }

        [JsonProperty("initiated_by")]
        public string InitiatedBy
        {
            get { return GetValue<string>("initiated_by"); }
            set { SetValue("initiated_by", value); }
        }

        [JsonProperty("gateway")]
        public string Gateway
        {
            get { return GetValue<string>("gateway"); }
            set { SetValue("gateway", value); }
        }

        [JsonProperty("error_message")]
        public string ErrorMessage
        {
            get { return GetValue<string>("error_message"); }
            set { SetValue("error_message", value); }
        }

        [JsonProperty("epg_txn_id")]
        public string EpgTxnId
        {
            get { return GetValue<string>("epg_txn_id"); }
            set { SetValue("epg_txn_id", value); }
        }

        [JsonProperty("created")]
        public string Created
        {
            get { return GetValue<string>("created"); }
            set { SetValue("created", value); }
        }

        [JsonProperty("authorization_id")]
        public string AuthorizationId
        {
            get { return GetValue<string>("authorization_id"); }
            set { SetValue("authorization_id", value); }
        }

        [JsonProperty("amount")]
        public double Amount
        {
            get { return GetValue<double>("amount"); }
            set { SetValue("amount", value); }
        }
    }

}