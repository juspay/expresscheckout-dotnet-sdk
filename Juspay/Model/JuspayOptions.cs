namespace Juspay {
    using Newtonsoft.Json;
    public class JuspayOptions : JuspayEntity
    {
        [JsonProperty("client_auth_token")]
        public string? ClientAuthToken { get; set; }
        [JsonProperty("client_auth_token_expiry")]
        public string? ClientAuthTokenExpiry { get; set; }
    }
}