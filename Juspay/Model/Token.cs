namespace Juspay
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    public class Token : JuspayResponse
    {
        [JsonProperty("card_reference")]
        public string CardReference { get; set; }
        [JsonProperty("last_four_digits")]
        public string LastFourDigits { get; set; }
        [JsonProperty("card_fingerprint")]
        public string CardFingerprint { get; set; }
        [JsonProperty("card_isin")]
        public string CardIsin { get; set; }
        [JsonProperty("expiry_year")]
        public string ExpiryYear { get; set; }
        [JsonProperty("expiry_month")]
        public string ExpiryMonth { get; set; }
        [JsonProperty("tokenization_status")]
        public string TokenizationStatus { get; set; }
        [JsonProperty("par")]
        public string Par { get; set; }
        [JsonProperty("cvv_less_support")]
        public bool CvvLessSupport { get; set; }
        [JsonProperty("cvv_less_supported_gateways")]
        public List<string> CvvLessSupportedGateways { get; set; }
    }
}