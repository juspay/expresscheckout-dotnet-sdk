namespace Juspay {
    using Newtonsoft.Json;
    using System.Collections.Generic;
    public class CustomerResponse : JuspayResponse
    {
        [JsonProperty("id")]
        public string Id
        {
            get { return GetValue<string>("id"); }
            set { SetValue("id", value); }
        }

        [JsonProperty("object")]
        public string Object
        {
            get { return GetValue<string>("object"); }
            set { SetValue("object", value); }
        }

        [JsonProperty("object_reference_id")]
        public string ObjectReferenceId
        {
            get { return GetValue<string>("object_reference_id"); }
            set { SetValue("object_reference_id", value); }
        }

        [JsonProperty("mobile_country_code")]
        public string MobileCountryCode
        {
            get { return GetValue<string>("mobile_country_code"); }
            set { SetValue("mobile_country_code", value); }
        }

        [JsonProperty("mobile_number")]
        public string MobileNumber
        {
            get { return GetValue<string>("mobile_number"); }
            set { SetValue("mobile_number", value); }
        }

        [JsonProperty("email_address")]
        public string EmailAddress
        {
            get { return GetValue<string>("email_address"); }
            set { SetValue("email_address", value); }
        }

        [JsonProperty("first_name")]
        public string FirstName
        {
            get { return GetValue<string>("first_name"); }
            set { SetValue("first_name", value); }
        }

        [JsonProperty("last_name")]
        public string LastName
        {
            get { return GetValue<string>("last_name"); }
            set { SetValue("last_name", value); }
        }

        [JsonProperty("date_created")]
        public string DateCreated
        {
            get { return GetValue<string>("date_created"); }
            set { SetValue("date_created", value); }
        }

        [JsonProperty("last_updated")]
        public string LastUpdated
        {
            get { return GetValue<string>("last_updated"); }
            set { SetValue("last_updated", value); }
        }

        [JsonProperty("juspay")]
        public JuspayOptions Juspay
        {
            get { return GetObject<JuspayOptions>("juspay"); }
            set { SetValue("juspay", value); }
        }

    }
    
}