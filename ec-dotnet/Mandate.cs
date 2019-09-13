using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Juspay.ExpressCheckout;
using Juspay.ExpressCheckout.Base;

namespace Juspay.ExpressCheckout
{
    public class Mandate
    {
        public enum MandateMode
        {
            REQUIRED,
            OPTIONAL
        }

        private static readonly string[] MANDATE_REQ_PARAMETERS = { "order_id", "amount", "customer_id", "mandate_id"};

        private static readonly string[] MANDATE_ALLOWED_PARAMETERS = {
            "order_id", "amount", "currency", "customer_id", "mandate_id", "customer_email", "customer_phone", "product_id", "return_url", "description",

            "billing_address_first_name", "billing_address_last_name", "billing_address_line1", "billing_address_line2", "billing_address_line3",
            "billing_address_city", "billing_address_state", "billing_address_country","billing_address_postal_code","billing_address_phone",
            "billing_address_country_code_iso",

            "shipping_address_first_name","shipping_address_last_name","shipping_address_line1","shipping_address_line2","shipping_address_line3",
            "shipping_address_city","shipping_address_state","shipping_address_postal_code", "shipping_address_phone","shipping_address_country_code_iso",
            "shipping_address_country",

            "udf1", "udf2", "udf3", "udf4", "udf5", "udf6", "udf7", "udf8", "udf9", "udf10",};
        

        public static async Task<ECApiResponse> CreateMandateOrder(IDictionary<string, string> orderDetails, MandateMode mode, string maxAmount, ECApiCredentials creds = null)
        {
            orderDetails.Add("options.create_mandate", mode.ToString());
            orderDetails.Add("mandate_max_amount", maxAmount);

            return await Orders.CreateOrder(orderDetails, creds);
        }

        public static async Task<ECApiResponse> Revoke(string mandateId, ECApiCredentials creds = null)
        {
            IDictionary<string, string> Payload = new Dictionary<string, string>();
            Payload.Add("command", "revoke");

            return await HTTPUtils.ParseAndWrapResponseJObject(
                await HTTPUtils.DoPost("/mandates/" + mandateId, Payload, creds));
        }

        public static async Task<ECApiResponse> List(string customerId, ECApiCredentials creds = null)
        {
            return await HTTPUtils.ParseAndWrapResponseJObject(
                await HTTPUtils.DoGet("/customers/" + customerId + "/mandates", null, creds));
        }

        /// <summary>
        /// Execute a S2S call to perform a mandate transaction
        /// </summary>
        /// <param name="mandateOrderDetails">
        ///     Dictionary holding the information of order to be created.
        ///     Note that the "order." prefix is not required as the method
        ///     adds it automatically.
        ///     For a full list of parameters, refer:
        ///     https://developer.juspay.in/reference#customer-initiates-the-transaction
        /// </param>
        /// <param name="mandateToken">
        ///     Mandate token previously obtained.
        /// </param>
        /// <param name="creds">
        ///     Optional API credentials to be passed which is chosen
        ///     over the credentials configured using the 
        ///     <see cref="Juspay.ExpressCheckout.Base.Config.Configure(Config.Environment, string, string, string)">
        ///         Configure
        ///     </see> API call.
        /// </param>
        /// <returns>ECApiResponse object having the JSON response data</returns>
        /// <exception cref="ArgumentException">Thrown when parameters are not specified correctly.</exception>
        public static async Task<ECApiResponse> ExecuteMandateTxn(
            IDictionary<string, string> mandateOrderDetails,
            string mandateToken,
            ECApiCredentials creds = null)
        {
            foreach(string param in MANDATE_REQ_PARAMETERS)
            {
                if(!mandateOrderDetails.ContainsKey(param))
                {
                    throw new ArgumentException(param + " must be specified");
                }
            }

            IDictionary<string, string> CleanDict = new Dictionary<string, string>();
            foreach(string param in MANDATE_ALLOWED_PARAMETERS)
            {
                if (mandateOrderDetails.ContainsKey(param))
                {
                    CleanDict.Add("order." + param, mandateOrderDetails[param]);
                }
            }

            return await HTTPUtils.ParseAndWrapResponseJObject(
                await HTTPUtils.DoPost("/txns", CleanDict, creds));
        }
    }
}
