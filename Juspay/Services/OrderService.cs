namespace Juspay {
    using Newtonsoft.Json;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    public class OrderCreate : JuspayEntity
    {
        public OrderCreate () : base() {}
        public OrderCreate(Dictionary<string, object> data) : base(data) {}

        [JsonProperty("order_id")]
        public string OrderId
        {
            get { return GetValue<string>("order_id"); }
            set { SetValue("order_id", value); }
        }

        [JsonProperty("amount")]
        public double Amount
        {
            get { return GetValue<double>("amount"); }
            set { SetValue("amount", value); }
        }

        [JsonProperty("currency")]
        public string Currency
        {
            get { return GetValue<string>("currency"); }
            set { SetValue("currency", value); }
        }

        [JsonProperty("metadata")]
        public Dictionary<string, object> Metadata
        {
            get { return GetValue<Dictionary<string, object>>("metadata"); }
            set { SetValue("metadata", value); }
        }

        [JsonProperty("options")]
        public ClientAuthToken Options
        {
            get { return GetObject<ClientAuthToken>("options"); }
            set { SetValue("options", value); }
        }
    }

    public class RefundOrder : JuspayEntity
    {
        public RefundOrder() : base() {}
        public RefundOrder(Dictionary<string, object> data) : base(data) {}

        [JsonProperty("order_id")]
        public string OrderId
        {
            get { return GetValue<string>("order_id"); }
            set { SetValue("order_id", value); }
        }

        [JsonProperty("amount")]
        public double Amount
        {
            get { return GetValue<double>("amount"); }
            set { SetValue("amount", value); }
        }

        [JsonProperty("unique_request_id")]
        public string RequestId
        {
            get { return GetValue<string>("unique_request_id"); }
            set { SetValue("unique_request_id", value); }
        }


    }

    public class  TransactionIdAndInstantRefund : JuspayEntity
    {
        public TransactionIdAndInstantRefund() : base() {}
        public TransactionIdAndInstantRefund(Dictionary<string, object> data) : base(data) {}

        [JsonProperty("order_id")]
        public string OrderId
        {
            get { return GetValue<string>("order_id"); }
            set { SetValue("order_id", value); }
        }

        [JsonProperty("amount")]
        public double Amount
        {
            get { return GetValue<double>("amount"); }
            set { SetValue("amount", value); }
        }

        [JsonProperty("unique_request_id")]
        public string RequestId
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

        [JsonProperty("refund_type")]
        public string RefundType
        {
            get { return GetValue<string>("refund_type"); }
            set { SetValue("refund_type", value); }
        }

        [JsonProperty("order_type")]
        public string OrderType
        {
            get { return GetValue<string>("order_type"); }
            set { SetValue("order_type", value); }
        }

        [JsonProperty("beneficiary_details")]
        public List<TransactionIdAndInstantRefundBeneficiary> BeneficiaryDetails
        {
            get { return GetValue<List<TransactionIdAndInstantRefundBeneficiary>>("beneficiary_details"); }
            set { SetValue("beneficiary_details", value); }
        }
    }

    public class TransactionIdAndInstantRefundBeneficiary : JuspayEntity
    {
        public TransactionIdAndInstantRefundBeneficiary() : base() {}
        public TransactionIdAndInstantRefundBeneficiary(Dictionary<string, object> data) : base(data) {}

        [JsonProperty("name1")]
        public string Name1
        {
            get { return GetValue<string>("name1"); }
            set { SetValue("name1", value); }
        }

        [JsonProperty("name2")]
        public string Name2
        {
            get { return GetValue<string>("name2"); }
            set { SetValue("name2", value); }
        }

    }
    public class OrderService : Service {
        public OrderService()
            : base()
        {
        }
        public OrderService(IJuspayClient client) : base(client)
        {
        }

        public override string BasePath { get; set; } = "/orders";

        public async Task<JuspayResponse> CreateOrderAsync(OrderCreate input, RequestOptions requestOptions)
        {
            this.BasePath = "/orders";
            return await this.CreateAsync(input, requestOptions);
        }

        public JuspayResponse CreateOrder(OrderCreate input, RequestOptions requestOptions) {
            this.BasePath = "/orders";
            return this.Create(input, requestOptions);
        }

        public async Task<JuspayResponse> GetOrderAsync(string orderId, RequestOptions requestOptions) {
            this.BasePath = "/orders";
            return await this.GetAsync(orderId, null, null, requestOptions);
        }
        public JuspayResponse GetOrder(string orderId, RequestOptions requestOptions) {
            this.BasePath = "/orders";
            return this.Get(orderId, null, null, requestOptions);
        }

        public async Task<JuspayResponse> RefundOrderAsync(string orderId, RefundOrder input, RequestOptions requestOptions) {
            this.BasePath = "/orders";
            this.BasePath = this.InstanceUrl(orderId);
            return await this.CreateAsync(input, requestOptions, "application/x-www-form-urlencoded", "/refunds");
        }

        public JuspayResponse RefundOrder(string orderId, RefundOrder input, RequestOptions requestOptions) {
            this.BasePath = "/orders";
            this.BasePath = this.InstanceUrl(orderId);
            return this.Create(input, requestOptions, "application/x-www-form-urlencoded", "/refunds");
        }

    }

    public class InstantRefundService : Service
    {
        public InstantRefundService()
            : base()
        {
        }
        public InstantRefundService(IJuspayClient client) : base(client)
        {
        }

        public override string BasePath => "/refunds";

        public JuspayResponse GetTransactionIdAndInstantRefund(TransactionIdAndInstantRefund input, RequestOptions requestOptions)
        {
            return this.Create(input, requestOptions);
        }

        public async Task<JuspayResponse> GetTransactionIdAndInstantRefundAsync(TransactionIdAndInstantRefund input, RequestOptions requestOptions)
        {
            return await this.CreateAsync(input, requestOptions);
        }
    }

}