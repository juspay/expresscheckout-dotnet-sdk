namespace Juspay {
    using Newtonsoft.Json;
    using System.Threading.Tasks;
    public class OrderCreate : JuspayEntity
    {
        public OrderCreate () : base() {}
        public OrderCreate(Dictionary<string, object> data) : base(data) {
            this.PopulateObject(data);
        }
        [JsonProperty("order_id")]
        public string? OrderId { get; set; }
        [JsonProperty("amount")]
        public double? Amount { get; set; }
        [JsonProperty("currency")]
        public string? Currency { get; set; }
        [JsonProperty("metadata")]
        public Dictionary<string, object>? Metadata { get; set; }
        [JsonProperty("options")]
        public ClientAuthToken? Options { get; set; }
    }

    public class RefundOrder : JuspayEntity
    {
        public RefundOrder() : base() {}
        public RefundOrder(Dictionary<string, object> data) : base(data) {
            this.PopulateObject(data);
        }
        [JsonProperty("order_id")]
        public string? OrderId { get; set; }
        [JsonProperty("amount")]
        public double? Amount { get; set; }
        [JsonProperty("unique_request_id")]
        public string RequestId { get; set; }
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
            return await this.GetAsync(orderId, null, null, requestOptions);
        }
        public OrderResponse GetOrder(string orderId, RequestOptions requestOptions) {
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