namespace Juspay {
    using Newtonsoft.Json;
    using System.Collections.Generic;
    public class OrderResponse : JuspayResponse
    {
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
        
        public string OrderId
        {
            get { return GetValue<string>("order_id"); }
            set { SetValue("order_id", value); }
        }

        public string MerchantId
        {
            get { return GetValue<string>("merchant_id"); }
            set { SetValue("merchant_id", value); }
        }

        public string TxnId
        {
            get { return GetValue<string>("txn_id"); }
            set { SetValue("txn_id", value); }
        }

        public double Amount
        {
            get { return GetValue<double>("amount"); }
            set { SetValue("amount", value); }
        }

        public double EffectiveAmount
        {
            get { return GetValue<double>("effective_amount"); }
            set { SetValue("effective_amount", value); }
        }

        public string RespCode
        {
            get { return GetValue<string>("resp_code"); }
            set { SetValue("resp_code", value); }
        }

        public string RespMessage
        {
            get { return GetValue<string>("resp_message"); }
            set { SetValue("resp_message", value); }
        }

        public string Currency
        {
            get { return GetValue<string>("currency"); }
            set { SetValue("currency", value); }
        }

        public string CustomerId
        {
            get { return GetValue<string>("customer_id"); }
            set { SetValue("customer_id", value); }
        }

        public string CustomerEmail
        {
            get { return GetValue<string>("customer_email"); }
            set { SetValue("customer_email", value); }
        }

        public string CustomerPhone
        {
            get { return GetValue<string>("customer_phone"); }
            set { SetValue("customer_phone", value); }
        }

        public string Description
        {
            get { return GetValue<string>("description"); }
            set { SetValue("description", value); }
        }

        public string ProductId
        {
            get { return GetValue<string>("product_id"); }
            set { SetValue("product_id", value); }
        }

        public long GatewayId
        {
            get { return GetValue<long>("gateway_id"); }
            set { SetValue("gateway_id", value); }
        }

        public string ReturnUrl
        {
            get { return GetValue<string>("return_url"); }
            set { SetValue("return_url", value); }
        }

        public string Status
        {
            get { return GetValue<string>("status"); }
            set { SetValue("status", value); }
        }

        public long StatusId
        {
            get { return GetValue<long>("status_id"); }
            set { SetValue("status_id", value); }
        }

        public string GatewayReferenceId
        {
            get { return GetValue<string>("gateway_reference_id"); }
            set { SetValue("gateway_reference_id", value); }
        }
    }
}