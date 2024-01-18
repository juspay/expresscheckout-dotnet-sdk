namespace Juspay {
     using System.Text.Json.Serialization;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    public class OrderCreate : JuspayEntity
    {
        public OrderCreate () : base() {}
        public OrderCreate(Dictionary<string, dynamic> data) : base(data) {}

    }

    public class RefundOrder : JuspayEntity
    {
        public RefundOrder() : base() {}
        public RefundOrder(Dictionary<string, dynamic> data) : base(data) {}

    }

    public class  TransactionIdAndInstantRefund : JuspayEntity
    {
        public TransactionIdAndInstantRefund() : base() {}
        public TransactionIdAndInstantRefund(Dictionary<string, dynamic> data) : base(data) {}

    }

    public class TransactionIdAndInstantRefundBeneficiary : JuspayEntity
    {
        public TransactionIdAndInstantRefundBeneficiary() : base() {}
        public TransactionIdAndInstantRefundBeneficiary(Dictionary<string, dynamic> data) : base(data) {}

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
            return base.Create(new JuspayEntity(new Dictionary<string, dynamic>(){ {"amount", amount } }), requestOptions);
        }

        public Task<JuspayResponse> UpdateAsync(string orderId, double amount, RequestOptions requestOptions)
        {
            this.BasePath = "/orders";
            this.BasePath = this.InstanceUrl(orderId);
            return base.CreateAsync(new JuspayEntity(new Dictionary<string, dynamic>(){ {"amount", amount } }), requestOptions);
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
            return base.Create(new JuspayEntity(new Dictionary<string, dynamic> {{"order_id", orderId}}), requestOptions, ContentType.Json, true);
        }

        private async Task<JuspayResponse> EncryptedOrderStatusAsync(string orderId, Dictionary<string, object> queryParams, RequestOptions requestOptions) {
            this.BasePath = "/v4/order-status";
            return await base.CreateAsync(new JuspayEntity(new Dictionary<string, dynamic> {{"order_id", orderId}}), requestOptions, ContentType.Json, true);
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