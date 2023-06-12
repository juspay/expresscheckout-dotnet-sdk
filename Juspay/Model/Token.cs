namespace Juspay
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    public class Token : JuspayResponse
    {
        [JsonProperty("card_reference")]
        public string CardReference
        {
            get => GetValue<string>("card_reference");
            set => SetValue("card_reference", value);
        }

        [JsonProperty("last_four_digits")]
        public string LastFourDigits
        {
            get => GetValue<string>("last_four_digits");
            set => SetValue("last_four_digits", value);
        }

        [JsonProperty("card_fingerprint")]
        public string CardFingerprint
        {
            get => GetValue<string>("card_fingerprint");
            set => SetValue("card_fingerprint", value);
        }

        [JsonProperty("card_isin")]
        public string CardIsin
        {
            get => GetValue<string>("card_isin");
            set => SetValue("card_isin", value);
        }

        [JsonProperty("expiry_year")]
        public string ExpiryYear
        {
            get => GetValue<string>("expiry_year");
            set => SetValue("expiry_year", value);
        }

        [JsonProperty("expiry_month")]
        public string ExpiryMonth
        {
            get => GetValue<string>("expiry_month");
            set => SetValue("expiry_month", value);
        }

        [JsonProperty("tokenization_status")]
        public string TokenizationStatus
        {
            get => GetValue<string>("tokenization_status");
            set => SetValue("tokenization_status", value);
        }

        [JsonProperty("par")]
        public string Par
        {
            get => GetValue<string>("par");
            set => SetValue("par", value);
        }

        [JsonProperty("cvv_less_support")]
        public bool CvvLessSupport
        {
            get => GetValue<bool>("cvv_less_support");
            set => SetValue("cvv_less_support", value);
        }

        [JsonProperty("cvv_less_supported_gateways")]
        public List<object> CvvLessSupportedGateways
        {
            get => GetValue<List<object>>("cvv_less_supported_gateways");
            set => SetValue("cvv_less_supported_gateways", value);
        }
    }

}