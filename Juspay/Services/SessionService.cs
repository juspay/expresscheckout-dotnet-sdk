namespace Juspay {
    using Newtonsoft.Json;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    public class CreateOrderSessionInput : JuspayEntity {
        public CreateOrderSessionInput() : base() {}
        public CreateOrderSessionInput(Dictionary<string, object> data) : base(data) {}
        [JsonProperty("amount")]
        public string Amount
        {
            get { return GetValue<string>("amount"); }
            set { SetValue("amount", value); }
        }

        [JsonProperty("order_id")]
        public string OrderId
        {
            get { return GetValue<string>("order_id"); }
            set { SetValue("order_id", value); }
        }

        [JsonProperty("payment_page_client_id")]
        public string PaymentPageClientId
        {
            get { return GetValue<string>("payment_page_client_id"); }
            set { SetValue("payment_page_client_id", value); }
        }

        [JsonProperty("action")]
        public string Action
        {
            get { return GetValue<string>("action"); }
            set { SetValue("action", value); }
        }

        [JsonProperty("return_url")]
        public string ReturnUrl
        {
            get { return GetValue<string>("return_url"); }
            set { SetValue("return_url", value); }
        }
    }

    public class OrderSession : Service {
        public OrderSession()
            : base()
        {
        }

        public OrderSession(IJuspayClient client)
            : base(client)
        {
        }
    
        public override string BasePath { get; set; } = "/session";
    
        public async Task<JuspayResponse> CreateAsync(CreateOrderSessionInput input, RequestOptions requestOptions)
        {
            if (shouldUseJwt(requestOptions))
            {
                return await this.EncryptedCreateOrderSessionAsync(input, requestOptions);
            }
            this.BasePath = "/session";
            return await this.CreateAsync(input, requestOptions, ContentType.Json);
        }
        public JuspayResponse Create(CreateOrderSessionInput input, RequestOptions requestOptions)
        {
            if (shouldUseJwt(requestOptions)) {
                return this.EncryptedCreateOrderSession(input, requestOptions);
            }
            this.BasePath = "/session";
            return this.Create(input, requestOptions, ContentType.Json);
        }

        private JuspayResponse EncryptedCreateOrderSession(CreateOrderSessionInput input, RequestOptions requestOptions)
        {
            this.BasePath = "/v4/session";
            return this.Create(input, requestOptions, ContentType.Json, true);
        }
        private async Task<JuspayResponse> EncryptedCreateOrderSessionAsync(CreateOrderSessionInput input, RequestOptions requestOptions)
        {
            this.BasePath = "/v4/session";
            return await this.CreateAsync(input, requestOptions, ContentType.Json, true);
        }
    }
}