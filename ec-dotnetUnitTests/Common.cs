using System;
using System.Collections.Generic;
using System.Text;
using Juspay.ExpressCheckout;
using Juspay.ExpressCheckout.Base;
using System.Threading.Tasks;

namespace ec_dotnetUnitTests
{
    class Common
    {
        public static string RandomId()
        {
            return String.Format("Csharp-SDK-CustID-{0}", Guid.NewGuid().ToString());
        }

        public static async Task<ECApiResponse> DoOrderCreate(string OrderId)
        {
            var OrderDetails = new Dictionary<string, string>();
           

            OrderDetails.Add("order_id", OrderId);
            OrderDetails.Add("amount", "10.00");

            return await Orders.CreateOrder(OrderDetails);
        }
    }
}
