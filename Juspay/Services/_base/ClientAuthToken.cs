namespace Juspay {
     using Newtonsoft.Json;
    public class ClientAuthToken : JuspayEntity
    {
        public ClientAuthToken() : base() {}
        public ClientAuthToken(Dictionary<string, object> data) : base (data) {}
        [JsonProperty("get_client_auth_token")]
        public bool? GetClientAuthToken { get; set; }
    }
}