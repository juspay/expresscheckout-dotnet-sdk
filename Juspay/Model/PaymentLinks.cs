namespace Juspay
{
    using Newtonsoft.Json;
    public class PaymentLinks : JuspayResponse
    {
        public string Web
        {
            get { return GetValue<string>("web"); }
            set { SetValue("web", value); }
        }

        public string Mobile
        {
            get { return GetValue<string>("mobile"); }
            set { SetValue("mobile", value); }
        }

        public string Iframe
        {
            get { return GetValue<string>("iframe"); }
            set { SetValue("iframe", value); }
        }
    }
}