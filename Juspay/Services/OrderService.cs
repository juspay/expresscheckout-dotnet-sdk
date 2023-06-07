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

    public class OrderService : Service<OrderResponse> {
        public OrderService()
            : base()
        {
        }
        public OrderService(IJuspayClient client) : base(client)
        {
        }

        public override string BasePath { get; set; } = "/orders";

        public async Task<OrderResponse> CreateOrderAsync(OrderCreate input, RequestOptions requestOptions)
        {
            this.BasePath = "/orders";
            return await this.CreateAsync(input, requestOptions);
        }

        public OrderResponse CreateOrder(OrderCreate input, RequestOptions requestOptions) {
            this.BasePath = "/orders";
            return this.Create(input, requestOptions);
        }

        public async Task<OrderResponse> GetOrderAsync(string orderId, RequestOptions requestOptions) {
            this.BasePath = "/orders";
            return await this.GetAsync(orderId, null, null, requestOptions);
        }
        public OrderResponse GetOrder(string orderId, RequestOptions requestOptions) {
            this.BasePath = "/orders";
            return this.Get(orderId, null, null, requestOptions);
        }

        public async Task<OrderResponse> RefundOrderAsync(string orderId, RefundOrder input, RequestOptions requestOptions) {
            this.BasePath = this.InstanceUrl(orderId);
            return await this.CreateAsync(input, requestOptions, "application/x-www-form-urlencoded", "/refunds");
        }

        public OrderResponse RefundOrder(string orderId, RefundOrder input, RequestOptions requestOptions) {
            this.BasePath = this.InstanceUrl(orderId);
            return this.Create(input, requestOptions, "application/x-www-form-urlencoded", "/refunds");
        }
    }

}