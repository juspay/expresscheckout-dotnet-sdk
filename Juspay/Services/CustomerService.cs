namespace Juspay {
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    public class CreateCustomerInput : JuspayEntity
    {   
        public CreateCustomerInput() : base() {}
        public CreateCustomerInput(Dictionary<string, dynamic> data) : base(data) {}

    }
    public class GetCustomerOptions : JuspayEntity {
        public GetCustomerOptions() : base() {}
        public GetCustomerOptions(Dictionary<string, dynamic> data) : base(data) {
            
        }
    }

    public class Customer : Service {
        public Customer()
            : base()
        {
        }

        public Customer(IJuspayClient client)
            : base(client)
        {
        }
    
        public override string BasePath => "/customers";
    
        public async Task<JuspayResponse> CreateAsync(CreateCustomerInput input, RequestOptions requestOptions)
        {
            return await base.CreateAsync(input, requestOptions);
        }
        public JuspayResponse Create(CreateCustomerInput input, RequestOptions requestOptions)
        {
            return base.Create(input, requestOptions);
        }
        public async Task<JuspayResponse> GetAsync(string customerId, GetCustomerOptions getCustomerOptions, RequestOptions requestOptions) {
            return await base.GetAsync(customerId, null, getCustomerOptions, requestOptions);
        }
        public JuspayResponse Get(string customerId, GetCustomerOptions getCustomerOptions, RequestOptions requestOptions) {
            return base.Get(customerId, null, getCustomerOptions, requestOptions);
        }
    }
}