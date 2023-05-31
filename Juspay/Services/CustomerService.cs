namespace Juspay {
    using Newtonsoft.Json;
    using System.Threading.Tasks;
    public class CreateCustomerInput : JuspayEntity
    {
        [JsonProperty("object_reference_id")]
        public string? ObjectReferenceId { get; set; }
        [JsonProperty("mobile_number")]
        public string? MobileNumber { get; set; }
        [JsonProperty("email_address")]
        public string? EmailAddress { get; set; }
        [JsonProperty("first_name")]
        public string? FirstName { get; set; }
        [JsonProperty("last_name")]
        public string? LastName { get; set; }
        [JsonProperty("mobile_country_code")]
        public string? MobileCountryCode { get; set; }
        [JsonProperty("options")]
        public ClientAuthToken? Options { get; set; }
    }
    public class GetCustomerOptions : JuspayEntity {
        [JsonProperty("options")]
        public ClientAuthToken? Options { get; set; } 
    }

    public class CustomerService : Service<CustomerResponse> {
        public CustomerService()
            : base()
        {
        }

        public CustomerService(IJuspayClient client)
            : base(client)
        {
        }
    
        public override string BasePath => "/customers";
    
        public async Task<CustomerResponse> CreateCustomerAsync(JuspayEntity input, RequestOptions requestOptions)
        {
            return await this.CreateAsync(input, requestOptions);
        }
        public CustomerResponse CreateCustomer(JuspayEntity input, RequestOptions requestOptions)
        {
            return this.Create(input, requestOptions);
        }
        public async Task<CustomerResponse> GetCustomerAsync(string customerId, JuspayEntity getCustomerOptions, RequestOptions requestOptions) {
            return await this.GetAsync(customerId, null, getCustomerOptions, requestOptions);
        }
        public CustomerResponse GetCustomer(string customerId, JuspayEntity getCustomerOptions, RequestOptions requestOptions) {
            return this.Get(customerId, null, getCustomerOptions, requestOptions);
        }
    }
}