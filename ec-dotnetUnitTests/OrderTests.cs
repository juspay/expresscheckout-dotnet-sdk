using System;
using Xunit;
using Juspay.ExpressCheckout;
using System.Collections.Generic;
using Juspay.ExpressCheckout.Base;
using System;
using System.Threading;

namespace ec_dotnetUnitTests
{
    public class OrdersTests
    {
        static OrdersTests()
        {
            Config.Configure(Config.Environment.SANDBOX, "sriduth_sandbox_test", "4D94B6287BE459EB735EE9AB0DA212");
        }

        private static string RandomOrderId()
        {
            return Common.RandomId();
        }

        [Fact]
        public void CreateOrderTest()
        {
            var OrderId = RandomOrderId();

            var udfs = new string[] { "1", "2", "3", "4", "5", "6", "7", "8" };
            var OrderDetails = new Dictionary<string, string>();

            OrderDetails.Add("order_id", OrderId);
            OrderDetails.Add("amount", "10.00");

            for (int i = 0; i < udfs.Length; i++)
            {
                OrderDetails.Add("udf" + (i + 1), udfs[i]);
            }

            OrderDetails.Add("options.create_mandate", "OPTIONAL");
            OrderDetails.Add("mandate_max_amount", "1000.0");
            OrderDetails.Add("customer_id", "random_customer_id");

            dynamic OrderResponse = Orders.CreateOrder(OrderDetails);

            try
            {
                OrderResponse = OrderResponse.Result.Response;
                Assert.Equal(OrderResponse["order_id"].ToString(), OrderId);

                // sleep for 2 seconds so that status can propogate to the system
                Thread.Sleep(2000);

                dynamic OrderStatus = Orders.GetStatus(OrderId);
                OrderStatus = OrderStatus.Result.Response;

                Assert.Equal(OrderStatus["customer_id"].ToString(), "random_customer_id");
            }
            catch (AggregateException e)
            {
                Assert.True(false, "message");
            }
        }

        [Fact]
        public void OrderStatusTest()
        {
            var OrderId = "Csharp-SDK-455347f6-1d29-4846-b65d-ec01c56f00e6";

            try
            {
                dynamic OrderStatusResponse = Orders.GetStatus(OrderId);
                OrderStatusResponse = OrderStatusResponse.Result.Response;
                Assert.Equal(OrderStatusResponse["order_id"].ToString(), OrderId);
            }
            catch (AggregateException e)
            {
                Assert.True(false, "message");
            }
        }

        [Fact]
        public void OrderListTest()
        {
            var count = 20;
            dynamic OrderListResponse = Orders.List(count);
            try
            {
                OrderListResponse = OrderListResponse.Result.Response;
                Assert.Equal(OrderListResponse.count.ToString(), count.ToString());
            }
            catch (AggregateException e)
            {
                Assert.True(false, "message");
            }
        }

        [Fact]
        public void OrderCreateVerifyUDFS()
        {
            var OrderId = RandomOrderId();

            string[] udfs = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10"};

            dynamic OrderResponse = Common.DoOrderCreate(OrderId, udfs).Result.Response;
            dynamic OrderStatus =  Orders.GetStatus(OrderId).Result.Response;

            Assert.True(OrderResponse["status"] == "CREATED");
            for(int i=0;i<udfs.Length; i++)
            {
                string receivedVal = OrderStatus["udf" + (i + 1)];
                string expectedVal = (i + 1).ToString();

                Assert.Equal(receivedVal, expectedVal);
            }
        }
    }
}
