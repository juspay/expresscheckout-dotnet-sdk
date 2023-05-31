namespace Juspay
{
    using Newtonsoft.Json;
    public class TxnCardInfo : JuspayResponse
    {
        [JsonProperty("object")]
        public string? Object { get; set; }
        [JsonProperty("card_type")]
        public string? CardType { get; set; }
        [JsonProperty("card_isin")]
        public string? CardIsin { get; set; }
        [JsonProperty("card_issuer")]
        public string? CardIssuer { get; set; }
        [JsonProperty("card_exp_year")]
        public string? CardExpYear { get; set; }
        [JsonProperty("card_exp_month")]
        public string? CardExpMonth { get; set; }
        [JsonProperty("card_brand")]
        public string? CardBrand { get; set; }
        [JsonProperty("card_last_four_digits")]
        public string? CardLastFourDigits { get; set; }
        [JsonProperty("name_on_card")]
        public string? NameOnCard { get; set; }
        [JsonProperty("card_reference")]
        public string? CardReference { get; set; }
        [JsonProperty("card_fingerprint")]
        public string? CardFingerprint { get; set; }
    }
}