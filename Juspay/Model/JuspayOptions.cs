namespace Juspay {
    using Newtonsoft.Json;
    using System.Collections.Generic;
    public class JuspayOptions : JuspayResponse
    {
        [JsonProperty("client_auth_token")]
        public string ClientAuthToken
        {
            get { return GetValue<string>("client_auth_token"); }
            set { SetValue("client_auth_token", value); }
        }

        [JsonProperty("client_auth_token_expiry")]
        public string ClientAuthTokenExpiry
        {
            get { return GetValue<string>("client_auth_token_expiry"); }
            set { SetValue("client_auth_token_expiry", value); }
        }
    }
}