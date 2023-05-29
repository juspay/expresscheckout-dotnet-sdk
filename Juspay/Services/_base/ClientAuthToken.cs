namespace Juspay {
     using Newtonsoft.Json;
    public class ClientAuthToken : JuspayEntity
    {
        [JsonProperty("get_client_auth_token")]
        public bool? GetClientAuthToken { get; set; }
    }
}