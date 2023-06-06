namespace Juspay
{
    using Newtonsoft.Json;
    public class TxnCardInfo : JuspayResponse
    {
        [JsonProperty("object")]
        public string Object
        {
            get { return GetValue<string>("object"); }
            set { SetValue("object", value); }
        }

        [JsonProperty("card_type")]
        public string CardType
        {
            get { return GetValue<string>("card_type"); }
            set { SetValue("card_type", value); }
        }

        [JsonProperty("card_isin")]
        public string CardIsin
        {
            get { return GetValue<string>("card_isin"); }
            set { SetValue("card_isin", value); }
        }

        [JsonProperty("card_issuer")]
        public string CardIssuer
        {
            get { return GetValue<string>("card_issuer"); }
            set { SetValue("card_issuer", value); }
        }

        [JsonProperty("card_exp_year")]
        public string CardExpYear
        {
            get { return GetValue<string>("card_exp_year"); }
            set { SetValue("card_exp_year", value); }
        }

        [JsonProperty("card_exp_month")]
        public string CardExpMonth
        {
            get { return GetValue<string>("card_exp_month"); }
            set { SetValue("card_exp_month", value); }
        }

        [JsonProperty("card_brand")]
        public string CardBrand
        {
            get { return GetValue<string>("card_brand"); }
            set { SetValue("card_brand", value); }
        }

        [JsonProperty("card_last_four_digits")]
        public string CardLastFourDigits
        {
            get { return GetValue<string>("card_last_four_digits"); }
            set { SetValue("card_last_four_digits", value); }
        }

        [JsonProperty("name_on_card")]
        public string NameOnCard
        {
            get { return GetValue<string>("name_on_card"); }
            set { SetValue("name_on_card", value); }
        }

        [JsonProperty("card_reference")]
        public string CardReference
        {
            get { return GetValue<string>("card_reference"); }
            set { SetValue("card_reference", value); }
        }

        [JsonProperty("card_fingerprint")]
        public string CardFingerprint
        {
            get { return GetValue<string>("card_fingerprint"); }
            set { SetValue("card_fingerprint", value); }
        }
    }

}