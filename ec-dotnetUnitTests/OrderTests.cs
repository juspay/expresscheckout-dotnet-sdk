using System;
using Xunit;
using Juspay.ExpressCheckout;
using System.Collections.Generic;
using Juspay.ExpressCheckout.Base;

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

            dynamic OrderResponse = Common.DoOrderCreate(OrderId);
            try
            {
                OrderResponse = OrderResponse.Result.Response;
                Assert.Equal(OrderResponse["order_id"].ToString(), OrderId);
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
    }
}
