using Microsoft.VisualStudio.TestTools.UnitTesting;
using Juspay.ExpressCheckout;
using Juspay.ExpressCheckout.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Juspay.ExpressCheckout.Tests
{
    [TestClass()]
    public class OrdersTests
    {
        static OrdersTests()
        {
            Config.Configure(Config.Environment.SANDBOX, "sriduth_sandbox_test", "F740059B99694ABF83A7C1C879B6892B");
        }

        private static string RandomOrderId()
        {
            return String.Format("Csharp-SDK-{0}", Guid.NewGuid().ToString());
        }

        [TestMethod()]
        public void CreateOrderTest()
        {
            var OrderDetails = new Dictionary<string, string>();
            var OrderId = RandomOrderId();

            OrderDetails.Add("order_id", OrderId);
            OrderDetails.Add("amount", "10.00");
            
            dynamic OrderResponse = Orders.CreateOrder(OrderDetails);
            try
            {
                OrderResponse = OrderResponse.Result.Response;
                Assert.AreEqual(OrderResponse["order_id"].ToString(), OrderId);
            }
            catch (AggregateException e)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void OrderStatusTest()
        {
            var OrderId = "Csharp-SDK-b2b78e5c-c2bf-481d-aae5-2003ec9738df";

            dynamic OrderStatusResponse = Orders.GetStatus(OrderId);
            try
            {
                OrderStatusResponse = OrderStatusResponse.Result.Reponse;
                Assert.AreEqual(OrderStatusResponse["order_id"].ToString(), OrderId);
            }
            catch(AggregateException e)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void OrderListTest()
        {
            var count = 20;
            dynamic OrderListResponse = Orders.List(count);
            try
            {
                OrderListResponse = OrderListResponse.Result.Response;
                Assert.AreEqual(OrderListResponse.count.ToString(), count.ToString());
            }
            catch(AggregateException e)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void OrderUpdateTest()
        {

        }

        [TestMethod()]
        public void OrderRefundTest()
        {

        }
    }
}