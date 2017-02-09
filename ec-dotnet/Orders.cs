using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Juspay.ExpressCheckout.Base;

namespace Juspay.ExpressCheckout
{
    public sealed class Orders
    {
        private static readonly string[] ORDER_REQ_PARAMETERS = { "order_id", "amount" };
        private static readonly string[] ORDER_ALLOWED_PARAMETERS = {
            "order_id", "amount", "currency", "customer_id", "customer_email", "customer_phone", "product_id", "return_url", "description",

            "billing_address_first_name", "billing_address_last_name", "billing_address_line1", "billing_address_line2", "billing_address_line3",
            "billing_address_city", "billing_address_state", "billing_address_country","billing_address_postal_code","billing_address_phone",
            "billing_address_country_code_iso",

            "shipping_address_first_name","shipping_address_last_name","shipping_address_line1","shipping_address_line2","shipping_address_line3",
            "shipping_address_city","shipping_address_state","shipping_address_postal_code", "shipping_address_phone","shipping_address_country_code_iso",
            "shipping_address_country"
        };

        static Orders()
        {
            if(Config.GetApiKey() == null)
            {
                throw new ArgumentException("App is not configured, call Juspay.ExpressCheckout.Base.Config.Configure("
                    + ") to set up Express Checkout");
            }
        }

        // Create order given an orderId,
        // Returns a JObject for ease of use
        public static async Task<ECApiResponse> CreateOrder(IDictionary<string, string> orderDetails)
        {
            foreach(var item in ORDER_REQ_PARAMETERS)
            {
                if(!orderDetails.ContainsKey(item))
                {
                    throw new ArgumentException("order_id and amount must be specified");
                }
            }

            IDictionary<string, string> CleanDict = new Dictionary<string, string>();
            foreach(var item in ORDER_ALLOWED_PARAMETERS)
            {
                if(orderDetails.ContainsKey(item))
                {
                    CleanDict.Add(item, orderDetails[item]); 
                }
            }

            return await HTTPUtils.ParseAndWrapResponseJObject(
                await HTTPUtils.DoPost("/order/create", CleanDict));
        }

        // Get the status for an order
        public static async Task<ECApiResponse> GetStatus(string orderId)
        {
            var message = await HTTPUtils.DoGet("/order/status", new Dictionary<string, string>() { { "order_id", orderId } });
            return await HTTPUtils.ParseAndWrapResponseJObject(message);
        }

        
        // List orders
        public static async Task<ECApiResponse> List(int count=10, int offset=int.MinValue, long gte=long.MinValue,
            long lte=long.MinValue, long gt=long.MinValue, long lt=long.MinValue)
        {

            if (count > 100 || count <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    String.Format("count must be between 0 and 100, you passed {0}", count));
            }

            IDictionary<string, string> query = new Dictionary<string, string>();
            query.Add("count", count.ToString());

            if(offset != int.MinValue)
            {
                query.Add("offset", offset.ToString());
            }

            if(gte != long.MinValue)
            {
                query.Add("created.gte", gte.ToString());
            }
            else if(gt != long.MinValue)
            {
                query.Add("created.gt", gt.ToString());
            }

            if (lte != long.MinValue)
            {
                query.Add("created.lte", lte.ToString());
            }
            else if (lt != long.MinValue)
            {
                query.Add("created.lt", lt.ToString());
            }

            return await HTTPUtils.ParseAndWrapResponseJObject(await HTTPUtils.DoGet("/orders", query));
        }

        // Update the order
        public static async Task<ECApiResponse> Update(string orderId, float amount)
        {
            if(orderId == null || amount <= 0)
            {
                throw new ArgumentException("Please specify orderId and amount");
            }

            var message = await HTTPUtils.DoPost(String.Format("/orders/{0}", orderId), new Dictionary<string, string>()
            {
                {"amount", amount.ToString()}
            });

            return await HTTPUtils.ParseAndWrapResponseJObject(message);
        }


        // Refund an order given orderId, amount and a unique request Id
        public static async Task<ECApiResponse> Refund(string orderId, float amount, string uniqueReqId)
        {
            if (orderId == null || uniqueReqId == null || amount <= 0)
            {
                throw new ArgumentException("Please specify the order ID (orderId) amount to be refunded (amount) and a unique"
                     + " request ID (uniqueReqId)");
            }

            var message = await HTTPUtils.DoPost(String.Format("/orders/{0}/refunds", orderId), new Dictionary<string, string>() {
                { "unique_request_id", uniqueReqId },
                { "amount", amount.ToString() }
            });

            return await HTTPUtils.ParseAndWrapResponseJObject(message);
        }
    }
}
