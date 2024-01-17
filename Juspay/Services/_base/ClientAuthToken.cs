namespace Juspay {
    using System.Text.Json.Serialization;
     using System.Collections.Generic;
    public class ClientAuthToken : JuspayEntity
    {
        public ClientAuthToken() : base() {}
        public ClientAuthToken(Dictionary<string, object> data) : base (data) {
        }
        [JsonPropertyName("get_client_auth_token")]
        public bool GetClientAuthToken {
            get { return GetValue<bool>("get_client_auth_token"); }
            set { SetValue("get_client_auth_token", value); }
        }
    }
}