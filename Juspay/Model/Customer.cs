namespace Juspay {
    using Newtonsoft.Json;
    public class CustomerResponse : JuspayEntity
    {
        [JsonProperty("id")]
        public string? Id { get; set; }
        [JsonProperty("object")]
        public string? Object { get; set; }
        [JsonProperty("object_reference_id")]
        public string? ObjectReferenceId { get; set; }
        [JsonProperty("mobile_country_code")]
        public string? MobileCountryCode { get; set; }
        [JsonProperty("mobile_number")]
        public string? MobileNumber { get; set; }
        [JsonProperty("email_address")]
        public string? EmailAddress { get; set; }
        [JsonProperty("first_name")]
        public string? FirstName { get; set; }
        [JsonProperty("last_name")]
        public string? LastName { get; set; }
        [JsonProperty("date_created")]
        public string? DateCreated { get; set; }
        [JsonProperty("last_updated")]
        public string? LastUpdated { get; set; }
        [JsonProperty("juspay")]
        public JuspayOptions? Juspay { get; set; }
    }
    public class JuspayOptions : JuspayEntity
    {
        [JsonProperty("client_auth_token")]
        public string? ClientAuthToken { get; set; }
        [JsonProperty("client_auth_token_expiry")]
        public string? ClientAuthTokenExpiry { get; set; }
    }
    
}