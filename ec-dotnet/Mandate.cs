using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Juspay.ExpressCheckout;
using Juspay.ExpressCheckout.Base;

namespace Juspay.ExpressCheckout
{
    class Mandate
    {
        public enum MandateMode
        {
            REQUIRED,
            OPTIONAL
        }

        public static async Task<ECApiResponse> CreateMandateOrder(IDictionary<string, string> orderDetails, MandateMode mode, int maxAmount, ECApiCredentials creds = null)
        {
            orderDetails.Add("options.create_mandate", mode.ToString());
            orderDetails.Add("mandate_max_amount", maxAmount.ToString());

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
    }
}
