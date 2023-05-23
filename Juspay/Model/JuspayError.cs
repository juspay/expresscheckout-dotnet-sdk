namespace Juspay
{
    using Newtonsoft.Json;

    public class JuspayError : JuspayEntity
    {
        
        [JsonProperty("error_code")]
        public string? ErrorCode { get; set; }

        [JsonProperty("status")]
        public string? Status { get; set; }

        [JsonProperty("error_message")]
        public string? ErrorMessage { get; set; }

        [JsonProperty("user_message")]
        public string? UserMessage { get; set; }
    }
}
