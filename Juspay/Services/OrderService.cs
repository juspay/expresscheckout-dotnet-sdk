namespace Juspay {
     using System.Text.Json.Serialization;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    public class OrderCreate : JuspayEntity
    {
        public OrderCreate () : base() {}
        public OrderCreate(Dictionary<string, object> data) : base(data) {}

        [JsonPropertyName("order_id")]
        public string OrderId
        {
            get { return GetValue<string>("order_id"); }
            set { SetValue("order_id", value); }
        }

        [JsonPropertyName("amount")]
        public double Amount
        {
            get { return GetValue<double>("amount"); }
            set { SetValue("amount", value); }
        }

        [JsonPropertyName("currency")]
        public string Currency
        {
            get { return GetValue<string>("currency"); }
            set { SetValue("currency", value); }
        }

        [JsonPropertyName("metadata")]
        public Dictionary<string, object> Metadata
        {
            get { return GetValue<Dictionary<string, object>>("metadata"); }
            set { SetValue("metadata", value); }
        }

        [JsonPropertyName("options")]
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

        [JsonPropertyName("order_id")]
        public string OrderId
        {
            get { return GetValue<string>("order_id"); }
            set { SetValue("order_id", value); }
        }

        [JsonPropertyName("amount")]
        public double Amount
        {
            get { return GetValue<double>("amount"); }
            set { SetValue("amount", value); }
        }

        [JsonPropertyName("unique_request_id")]
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

        [JsonPropertyName("order_id")]
        public string OrderId
        {
            get { return GetValue<string>("order_id"); }
            set { SetValue("order_id", value); }
        }

        [JsonPropertyName("amount")]
        public double Amount
        {
            get { return GetValue<double>("amount"); }
            set { SetValue("amount", value); }
        }

        [JsonPropertyName("unique_request_id")]
        public string RequestId
        {
            get { return GetValue<string>("unique_request_id"); }
            set { SetValue("unique_request_id", value); }
        }

        [JsonPropertyName("txn_id")]
        public string TxnId
        {
            get { return GetValue<string>("txn_id"); }
            set { SetValue("txn_id", value); }
        }

        [JsonPropertyName("refund_type")]
        public string RefundType
        {
            get { return GetValue<string>("refund_type"); }
            set { SetValue("refund_type", value); }
        }

        [JsonPropertyName("order_type")]
        public string OrderType
        {
            get { return GetValue<string>("order_type"); }
            set { SetValue("order_type", value); }
        }

        [JsonPropertyName("beneficiary_details")]
        public List<TransactionIdAndInstantRefundBeneficiary> BeneficiaryDetails
        {
            get { return GetObjectList<TransactionIdAndInstantRefundBeneficiary>("beneficiary_details"); }
            set { SetValue("beneficiary_details", value); }
        }
    }

    public class TransactionIdAndInstantRefundBeneficiary : JuspayEntity
    {
        public TransactionIdAndInstantRefundBeneficiary() : base() {}
        public TransactionIdAndInstantRefundBeneficiary(Dictionary<string, object> data) : base(data) {}

        [JsonPropertyName("name1")]
        public string Name1
        {
            get { return GetValue<string>("name1"); }
            set { SetValue("name1", value); }
        }

        [JsonPropertyName("name2")]
        public string Name2
        {
            get { return GetValue<string>("name2"); }
            set { SetValue("name2", value); }
        }

    }
    public class Order : Service {
        public Order()
            : base()
        {
        }
        public Order(IJuspayClient client) : base(client)
        {
        }

        public override string BasePath { get; set; } = "/orders";

        public async Task<JuspayResponse> CreateAsync(OrderCreate input, RequestOptions requestOptions)
        {
            this.BasePath = "/orders";
            return await base.CreateAsync(input, requestOptions);
        }

        public JuspayResponse Create(OrderCreate input, RequestOptions requestOptions) {
            this.BasePath = "/orders";
            return base.Create(input, requestOptions);
        }

        public async Task<JuspayResponse> GetAsync(string orderId, Dictionary<string, object> queryParams, RequestOptions requestOptions) {
            if (shouldUseJwt(requestOptions)) {
                return await this.EncryptedOrderStatusAsync(orderId, queryParams, requestOptions);
            }
            this.BasePath = "/orders";
            return await base.GetAsync(orderId, null, queryParams, requestOptions);
        }
        public JuspayResponse Get(string orderId, Dictionary<string, object> queryParams, RequestOptions requestOptions) {
            if (shouldUseJwt(requestOptions))
            {
                return this.EncryptedOrderStatus(orderId, queryParams, requestOptions);
            }
            this.BasePath = "/orders";
            return base.Get(orderId, null, queryParams, requestOptions);
        }

        public JuspayResponse Update(string orderId, double amount, RequestOptions requestOptions)
        {
            this.BasePath = "/orders";
            this.BasePath = this.InstanceUrl(orderId);
            return base.Create(new JuspayEntity(new Dictionary<string, object>(){ {"amount", amount } }), requestOptions);
        }

        public Task<JuspayResponse> UpdateAsync(string orderId, double amount, RequestOptions requestOptions)
        {
            this.BasePath = "/orders";
            this.BasePath = this.InstanceUrl(orderId);
            return base.CreateAsync(new JuspayEntity(new Dictionary<string, object>(){ {"amount", amount } }), requestOptions);
        }

        public async Task<JuspayResponse> RefundAsync(string orderId, RefundOrder input, RequestOptions requestOptions) {
            if (shouldUseJwt(requestOptions))
            {
                return await this.EncryptedRefundOrderAsync(orderId, input, requestOptions);
            }
            this.BasePath = "/orders";
            this.BasePath = this.InstanceUrl(orderId);
            return await base.CreateAsync(input, requestOptions,  ContentType.FormUrlEncoded, false, "/refunds");
        }

        public JuspayResponse Refund(string orderId, RefundOrder input, RequestOptions requestOptions) {
            if (shouldUseJwt(requestOptions))
            {
                return this.EncryptedRefundOrder(orderId, input, requestOptions);
            }
            this.BasePath = "/orders";
            this.BasePath = this.InstanceUrl(orderId);
            return base.Create(input, requestOptions, ContentType.FormUrlEncoded, false, "/refunds");
        }

        private JuspayResponse EncryptedOrderStatus(string orderId, Dictionary<string, object> queryParams, RequestOptions requestOptions) {
            this.BasePath = "/v4/order-status";
            return base.Create(new JuspayEntity(new Dictionary<string, object> {{"order_id", orderId}}), requestOptions, ContentType.Json, true);
        }

        private async Task<JuspayResponse> EncryptedOrderStatusAsync(string orderId, Dictionary<string, object> queryParams, RequestOptions requestOptions) {
            this.BasePath = "/v4/order-status";
            return await base.CreateAsync(new JuspayEntity(new Dictionary<string, object> {{"order_id", orderId}}), requestOptions, ContentType.Json, true);
        }

        private async Task<JuspayResponse> EncryptedRefundOrderAsync(string orderId, RefundOrder input, RequestOptions requestOptions)
        {
            this.BasePath = "/v4/orders";
            this.BasePath = this.InstanceUrl(orderId);
            return await base.CreateAsync(input, requestOptions, ContentType.Json, true, "/refunds");
        }

        private JuspayResponse EncryptedRefundOrder(string orderId, RefundOrder input, RequestOptions requestOptions)
        {
            this.BasePath = "/v4/orders";
            this.BasePath = this.InstanceUrl(orderId);
            return base.Create(input, requestOptions, ContentType.Json, true, "/refunds");
        }
    }

    public class Refund : Service
    {
        public Refund()
            : base()
        {
        }
        public Refund(IJuspayClient client) : base(client)
        {
        }

        public override string BasePath => "/refunds";

        public JuspayResponse Create(TransactionIdAndInstantRefund input, RequestOptions requestOptions)
        {
            return base.Create(input, requestOptions);
        }

        public async Task<JuspayResponse> CreateAsync(TransactionIdAndInstantRefund input, RequestOptions requestOptions)
        {
            return await base.CreateAsync(input, requestOptions);
        }
    }

}