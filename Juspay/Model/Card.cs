namespace Juspay {
    using Newtonsoft.Json;
    public class Card : JuspayResponse
    {
        [JsonProperty("card_number")]
        public string CardNumber { get; set; }
        [JsonProperty("name_on_card")]
        public string NameOnCard { get; set; }
        [JsonProperty("card_exp_year")]
        public string CardExpYear { get; set; }
        [JsonProperty("card_exp_month")]
        public string CardExpMonth { get; set; }
        [JsonProperty("expiry_month")]
        public string ExpiryMonth { get; set; }
        [JsonProperty("expiry_year")]
        public string ExpiryYear { get; set; }
        [JsonProperty("card_security_code")]
        public string CardSecurityCode { get; set; }
        [JsonProperty("nickname")]
        public string Nickname { get; set; }
        [JsonProperty("card_token")]
        public string CardToken { get; set; }
        [JsonProperty("card_reference")]
        public string CardReference { get; set; }
        [JsonProperty("card_fingerprint")]
        public string CardFingerprint { get; set; }
        [JsonProperty("card_isin")]
        public string CardIsin { get; set; }
        [JsonProperty("last_four_digits")]
        public string LastFourDigits { get; set; }
        [JsonProperty("card_type")]
        public string CardType { get; set; }
        [JsonProperty("card_issuer")]
        public string CardIssuer { get; set; }
        [JsonProperty("saved_to_locker")]
        public bool SavedToLocker { get; set; }
        [JsonProperty("expired")]
        public bool Expired { get; set; }
        [JsonProperty("card_brand")]
        public string CardBrand { get; set; }
        [JsonProperty("card_balance")]
        public double CardBalance { get; set; }
        [JsonProperty("using_saved_card")]
        public bool UsingSavedCard { get; set; }
        [JsonProperty("card_sub_type")]
        public string CardSubType { get; set; }
        [JsonProperty("card_issuer_country")]
        public string CardIssuerCountry { get; set; }
        [JsonProperty("juspay_bank_code")]
        public string JuspayBankCode { get; set; }
        [JsonProperty("using_token")]
        public bool UsingToken { get; set; }
        [JsonProperty("tokenization_user_consent")]
        public bool TokenizationUserConsent { get; set; }
        [JsonProperty("tokenize_support")]
        public bool TokenizeSupport { get; set; }
        [JsonProperty("provider_category")]
        public string ProviderCategory { get; set; }
        [JsonProperty("provider")]
        public string Provider { get; set; }
        [JsonProperty("token")]
        public Token Token { get; set; }
        [JsonProperty("metadata")]
        public Dictionary<string, object> Metadata { get; set; }
    }
}