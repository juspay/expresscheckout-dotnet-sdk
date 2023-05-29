namespace Juspay {
    using Newtonsoft.Json;
    using System.Threading.Tasks;
    public class CreateSessionInput : JuspayEntity {
        [JsonProperty("amount")]
        public string? Amount { get; set; }
        [JsonProperty("order_id")]
        public string? OrderId { get; set; }
        [JsonProperty("payment_page_client_id")]
        public string? PaymentPageClientId { get; set; }
        [JsonProperty("action")]
        public string? Action { get; set; }
        [JsonProperty("return_url")]
        public string? ReturnUrl { get; set; }
    }

    public class SessionService : Service<SessionResponse> {
        public SessionService()
            : base()
        {
        }

        public SessionService(IJuspayClient client)
            : base(client)
        {
        }
    
        public override string BasePath => "/session";
    
        public async Task<JuspayEntity> CreateSessionAsync(JuspayEntity input, RequestOptions requestOptions)
        {
            return await this.CreateAsync(input, requestOptions, "application/json");
        }
        public JuspayEntity CreateSession(JuspayEntity input, RequestOptions requestOptions)
        {
            return this.Create(input, requestOptions, "application/json");
        }
    }
}