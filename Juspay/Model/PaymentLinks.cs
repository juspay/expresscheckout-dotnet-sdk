namespace Juspay
{
    using Newtonsoft.Json;
    public class PaymentLinks : JuspayResponse
    {
        [JsonProperty("web")]
        public string? Web { get; set; }
        [JsonProperty("mobile")]
        public string? Mobile { get; set; }
        [JsonProperty("iframe")]
        public string? Iframe { get; set; }
    }
}