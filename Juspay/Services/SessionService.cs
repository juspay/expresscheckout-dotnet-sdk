namespace Juspay {
     using System.Text.Json.Serialization;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    public class CreateOrderSessionInput : JuspayEntity {
        public CreateOrderSessionInput() : base() {}
        public CreateOrderSessionInput(Dictionary<string, dynamic> data) : base(data) {}
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