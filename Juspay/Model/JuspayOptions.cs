namespace Juspay {
    using Newtonsoft.Json;
    public class JuspayOptions : JuspayEntity
    {
        public JuspayOptions() : base() {}
        public JuspayOptions(Dictionary<string, object> data) : base(data) {
            this.PopulateObject(data);
        }
        [JsonProperty("client_auth_token")]
        public string? ClientAuthToken { get; set; }
        [JsonProperty("client_auth_token_expiry")]
        public string? ClientAuthTokenExpiry { get; set; }
    }
}