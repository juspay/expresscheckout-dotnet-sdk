namespace Juspay {
    using Newtonsoft.Json;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    public class CreateCustomerInput : JuspayEntity
    {   
        public CreateCustomerInput() : base() {}
        public CreateCustomerInput(Dictionary<string, object> data) : base(data) {}

        [JsonProperty("object_reference_id")]
        public string ObjectReferenceId
        {
            get { return GetValue<string>("object_reference_id"); }
            set { SetValue("object_reference_id", value); }
        }

        [JsonProperty("mobile_number")]
        public string MobileNumber
        {
            get { return GetValue<string>("mobile_number"); }
            set { SetValue("mobile_number", value); }
        }

        [JsonProperty("email_address")]
        public string EmailAddress
        {
            get { return GetValue<string>("email_address"); }
            set { SetValue("email_address", value); }
        }

        [JsonProperty("first_name")]
        public string FirstName
        {
            get { return GetValue<string>("first_name"); }
            set { SetValue("first_name", value); }
        }

        [JsonProperty("last_name")]
        public string LastName
        {
            get { return GetValue<string>("last_name"); }
            set { SetValue("last_name", value); }
        }

        [JsonProperty("mobile_country_code")]
        public string MobileCountryCode
        {
            get { return GetValue<string>("mobile_country_code"); }
            set { SetValue("mobile_country_code", value); }
        }

        [JsonProperty("options")]
        public ClientAuthToken Options
        {
            get { return GetObject<ClientAuthToken>("options"); }
            set { SetValue("options", value); }
        }
    }
    public class GetCustomerOptions : JuspayEntity {
        public GetCustomerOptions() : base() {}
        public GetCustomerOptions(Dictionary<string, object> data) : base(data) {
            
        }
        [JsonProperty("options")]
        public ClientAuthToken Options
        {
            get { return GetObject<ClientAuthToken>("options"); }
            set { SetValue("options", value); }
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