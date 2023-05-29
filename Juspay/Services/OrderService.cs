namespace Juspay {
    using Newtonsoft.Json;
    using System.Threading.Tasks;
    public class OrderCreate : JuspayEntity
    {
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

        public async Task<JuspayEntity> CreateOrderAsync(JuspayEntity input, RequestOptions requestOptions)
        {
            return await this.CreateAsync(input, requestOptions);
        }

        public JuspayEntity CreateOrder(JuspayEntity input, RequestOptions requestOptions) {
            return this.Create(input, requestOptions);
        }

        public async Task<JuspayEntity> GetOrderAsync(string orderId, RequestOptions requestOptions) {
            return await this.GetAsync(orderId, null, null, requestOptions);
        }
        public JuspayEntity GetOrder(string orderId, RequestOptions requestOptions) {
            return this.Get(orderId, null, null, requestOptions);
        }

        public async Task<JuspayEntity> RefundOrderAsync(string orderId, JuspayEntity input, RequestOptions requestOptions) {
            this.BasePath = this.InstanceUrl(orderId);
            return await this.CreateAsync(input, requestOptions, "application/x-www-form-urlencoded", "/refunds");
            this.BasePath = "/orders";
        }

        public JuspayEntity RefundOrder(string orderId, JuspayEntity input, RequestOptions requestOptions) {
            BasePath = this.InstanceUrl(orderId);
            return this.Create(input, requestOptions, "application/x-www-form-urlencoded", "/refunds");
            this.BasePath = "/orders";
        }
    }

}