namespace Juspay
{
    using System.Text.Json.Serialization;

    public class JuspayError : JuspayEntity
    {
        
        [JsonPropertyName("error_code")]
        public string ErrorCode { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("error_message")]
        public string ErrorMessage { get; set; }

        [JsonPropertyName("user_message")]
        public string UserMessage { get; set; }
    }
}
