namespace Juspay
{
    using Newtonsoft.Json;
    public class Upi : JuspayResponse
    {
        [JsonProperty("txn_flow_type")]
        public string TxnFlowType
        {
            get => GetValue<string>("txn_flow_type");
            set => SetValue("txn_flow_type", value);
        }

        [JsonProperty("masked_bank_account_number")]
        public string MaskedBankAccountNumber
        {
            get => GetValue<string>("masked_bank_account_number");
            set => SetValue("masked_bank_account_number", value);
        }

        [JsonProperty("bank_name")]
        public string BankName
        {
            get => GetValue<string>("bank_name");
            set => SetValue("bank_name", value);
        }

        [JsonProperty("bank_code")]
        public string BankCode
        {
            get => GetValue<string>("bank_code");
            set => SetValue("bank_code", value);
        }

        [JsonProperty("upi_bank_account_reference_id")]
        public string UpiBankAccountReferenceId
        {
            get => GetValue<string>("upi_bank_account_reference_id");
            set => SetValue("upi_bank_account_reference_id", value);
        }

        [JsonProperty("payer_vpa")]
        public string PayerVpa
        {
            get => GetValue<string>("payer_vpa");
            set => SetValue("payer_vpa", value);
        }

        [JsonProperty("payer_app")]
        public string PayerApp
        {
            get => GetValue<string>("payer_app");
            set => SetValue("payer_app", value);
        }

        [JsonProperty("juspay_bank_code")]
        public string JuspayBankCode
        {
            get => GetValue<string>("juspay_bank_code");
            set => SetValue("juspay_bank_code", value);
        }
    }

}