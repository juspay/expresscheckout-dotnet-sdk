namespace Juspay
{
    using Newtonsoft.Json;
    public class TxnDetail : JuspayResponse
    {
        [JsonProperty("txn_id")]
        public string TxnId
        {
            get => GetValue<string>("txn_id");
            set => SetValue("txn_id", value);
        }

        [JsonProperty("order_id")]
        public string OrderId
        {
            get => GetValue<string>("order_id");
            set => SetValue("order_id", value);
        }

        [JsonProperty("txn_uuid")]
        public string TxnUuid
        {
            get => GetValue<string>("txn_uuid");
            set => SetValue("txn_uuid", value);
        }

        [JsonProperty("gateway_id")]
        public string GatewayId
        {
            get => GetValue<string>("gateway_id");
            set => SetValue("gateway_id", value);
        }

        [JsonProperty("status")]
        public string Status
        {
            get => GetValue<string>("status");
            set => SetValue("status", value);
        }

        [JsonProperty("gateway")]
        public string Gateway
        {
            get => GetValue<string>("gateway");
            set => SetValue("gateway", value);
        }

        [JsonProperty("express_checkout")]
        public bool ExpressCheckout
        {
            get => GetValue<bool>("express_checkout");
            set => SetValue("express_checkout", value);
        }

        [JsonProperty("redirect")]
        public bool Redirect
        {
            get => GetValue<bool>("redirect");
            set => SetValue("redirect", value);
        }

        [JsonProperty("net_amount")]
        public double NetAmount
        {
            get => GetValue<double>("net_amount");
            set => SetValue("net_amount", value);
        }

        [JsonProperty("surcharge_amount")]
        public double SurchargeAmount
        {
            get => GetValue<double>("surcharge_amount");
            set => SetValue("surcharge_amount", value);
        }

        [JsonProperty("tax_amount")]
        public double TaxAmount
        {
            get => GetValue<double>("tax_amount");
            set => SetValue("tax_amount", value);
        }

        [JsonProperty("txn_amount")]
        public double TxnAmount
        {
            get => GetValue<double>("txn_amount");
            set => SetValue("txn_amount", value);
        }

        [JsonProperty("currency")]
        public string Currency
        {
            get => GetValue<string>("currency");
            set => SetValue("currency", value);
        }

        [JsonProperty("error_message")]
        public string ErrorMessage
        {
            get => GetValue<string>("error_message");
            set => SetValue("error_message", value);
        }

        [JsonProperty("error_code")]
        public string ErrorCode
        {
            get => GetValue<string>("error_code");
            set => SetValue("error_code", value);
        }

        [JsonProperty("created")]
        public string Created
        {
            get => GetValue<string>("created");
            set => SetValue("created", value);
        }

        [JsonProperty("txn_object_type")]
        public string TxnObjectType
        {
            get => GetValue<string>("txn_object_type");
            set => SetValue("txn_object_type", value);
        }

        [JsonProperty("source_object")]
        public string SourceObject
        {
            get => GetValue<string>("source_object");
            set => SetValue("source_object", value);
        }

        [JsonProperty("source_object_id")]
        public string SourceObjectId
        {
            get => GetValue<string>("source_object_id");
            set => SetValue("source_object_id", value);
        }

        [JsonProperty("is_conflicted")]
        public bool IsConflicted
        {
            get => GetValue<bool>("is_conflicted");
            set => SetValue("is_conflicted", value);
        }

        [JsonProperty("is_emi")]
        public bool IsEmi
        {
            get => GetValue<bool>("is_emi");
            set => SetValue("is_emi", value);
        }

        [JsonProperty("emi_tenure")]
        public int EmiTenure
        {
            get => GetValue<int>("emi_tenure");
            set => SetValue("emi_tenure", value);
        }

        [JsonProperty("emi_bank")]
        public string EmiBank
        {
            get => GetValue<string>("emi_bank");
            set => SetValue("emi_bank", value);
        }

        [JsonProperty("refunded_amount")]
        public double RefundedAmount
        {
            get => GetValue<double>("refunded_amount");
            set => SetValue("refunded_amount", value);
        }

        [JsonProperty("refunded_entirely")]
        public bool RefundedEntirely
        {
            get => GetValue<bool>("refunded_entirely");
            set => SetValue("refunded_entirely", value);
        }

        [JsonProperty("txn_card_info")]
        public TxnCardInfo TxnCardInfo
        {
            get => GetObject<TxnCardInfo>("txn_card_info");
            set => SetValue("txn_card_info", value);
        }

        [JsonProperty("payment_gateway_response")]
        public PaymentGatewayResponse PaymentGatewayResponse
        {
            get => GetObject<PaymentGatewayResponse>("payment_gateway_response");
            set => SetValue("payment_gateway_response", value);
        }
    }

}