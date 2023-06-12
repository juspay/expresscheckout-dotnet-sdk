namespace Juspay
{
    using Newtonsoft.Json;
    public class PaymentLinks : JuspayResponse
    {
        [JsonProperty("web")]
        public string Web
        {
            get { return GetValue<string>("web"); }
            set { SetValue("web", value); }
        }

        [JsonProperty("mobile")]
        public string Mobile
        {
            get { return GetValue<string>("mobile"); }
            set { SetValue("mobile", value); }
        }

        [JsonProperty("iframe")]
        public string Iframe
        {
            get { return GetValue<string>("iframe"); }
            set { SetValue("iframe", value); }
        }
    }

}