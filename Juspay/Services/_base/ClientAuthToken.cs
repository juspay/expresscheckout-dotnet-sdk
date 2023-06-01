namespace Juspay {
     using Newtonsoft.Json;
     using System.Collections.Generic;
    public class ClientAuthToken : JuspayEntity
    {
        public ClientAuthToken() : base() {}
        public ClientAuthToken(Dictionary<string, object> data) : base (data) {
            this.PopulateObject(data);
        }
        [JsonProperty("get_client_auth_token")]
        public bool GetClientAuthToken { get; set; }
    }
}