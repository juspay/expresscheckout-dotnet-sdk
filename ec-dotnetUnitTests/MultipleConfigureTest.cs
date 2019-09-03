using System;
using Juspay.ExpressCheckout.Base;
using Xunit;
using Juspay.ExpressCheckout;
using System.Collections.Generic;

namespace ec_dotnetUnitTests
{
    public class MultipleConfigureTest
    {
        [Fact]
        public void ExplicitConfigurationCalls()
        {
            new System.Threading.Thread(() =>
            {
                string randId = Common.RandomId();
                for (int i=0; i<30; i++)
                {
                    string orderId = "sandbox__" + randId + "__" + i;
                    dynamic orderResult =
                        Common.DoOrderCreate(orderId,
                                             null,
                                             new ECApiCredentials("sriduth_sandbox_test", "4D94B6287BE459EB735EE9AB0DA212", "2018-10-25"));

                    orderResult = orderResult.Result.Response;
                    Assert.Equal(orderResult["order_id"].ToString(), orderId);

                }
         
            }).Start();

            new System.Threading.Thread(() =>
            {
                string randId = Common.RandomId();
                for (int i = 0; i < 30; i++)
                {
                    string orderId = "production__" + randId + "__" + i;
                    dynamic orderResult =
                        Common.DoOrderCreate(orderId,
                                             null,
                                             new ECApiCredentials("sriduth_prod_test", "F4211C67C9B84CCDB2A0ED2E65A4201D", "2018-10-25"));

                    orderResult = orderResult.Result.Response;
                    Assert.Equal(orderResult["order_id"].ToString(), orderId);

                }
            }).Start();
        }

        [Fact]
        public void ParallelCallsWithCommonAuth()
        {
            new System.Threading.Thread(() =>
            {
                Console.WriteLine("prod");
                System.Threading.Thread.CurrentThread.IsBackground = true;
                string randId = Common.RandomId();
                for (int i = 0; i < 30; i++)
                {
                    Console.WriteLine("randid" + randId + i);
                    Config.Configure(Config.Environment.SANDBOX, "sriduth_sandbox_test", "4D94B6287BE459EB735EE9AB0DA212");
                    string orderId = "sandbox__" + randId + "__" + i;
                    dynamic orderResult =
                        Common.DoOrderCreate(orderId);

                    orderResult = orderResult.Result.Response;
                    Assert.Equal(orderResult["order_id"].ToString(), orderId);
                }

            }).Start();

            new System.Threading.Thread(() =>
            {
                try
                {
                    Console.WriteLine("sandbox");
                    System.Threading.Thread.CurrentThread.IsBackground = true;
                    string randId = Common.RandomId();
                    for (int i = 0; i < 30; i++)
                    {
                        Config.Configure(Config.Environment.PRODUCTION, "sriduth_prod_test", "F4211C67C9B84CCDB2A0ED2E65A4201D");
                        string orderId = "production__" + randId + "__" + i;
                        dynamic orderResult =
                            Common.DoOrderCreate(orderId);

                        orderResult = orderResult.Result.Response;
                        Assert.Equal(orderResult["order_id"].ToString(), orderId);
                    }
                } 
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
           
            }).Start();

            System.Threading.Thread.Sleep(1000 * 20);
        }

        [Fact]
        public void TestMultipleConfigureCalls()
        {
            string o1 = Common.RandomId();
            string o2 = Common.RandomId();

            // Create 2 orders in sandbox and production respectively
            Config.Configure(Config.Environment.SANDBOX, "sriduth_sandbox_test", "4D94B6287BE459EB735EE9AB0DA212");
            dynamic order1 = Common.DoOrderCreate(o1);
            order1 = order1.Result.Response;
            Assert.Equal(order1["order_id"].ToString(), o1);

            Config.Configure(Config.Environment.PRODUCTION, "sriduth_prod_test", "F4211C67C9B84CCDB2A0ED2E65A4201D");
            dynamic order2 = Common.DoOrderCreate(o2);
            order2 = order2.Result.Response;
            Assert.Equal(order2["order_id"].ToString(), o2);

            // passing the order created in sandbox to order status with 
            // production configuration should yield a NOT_FOUND response code
            Config.Configure(Config.Environment.SANDBOX, "sriduth_sandbox_test", "4D94B6287BE459EB735EE9AB0DA212");
            order2 = Orders.GetStatus(o2);
            order2 = order2.Result.Response;
            Assert.Equal(order2["status"].ToString(), "NOT_FOUND");

            // Checking the same order status in the production environment must yield a proper status
            Config.Configure(Config.Environment.PRODUCTION, "sriduth_prod_test", "F4211C67C9B84CCDB2A0ED2E65A4201D");
            order2 = Orders.GetStatus(o2);

            // reset to the original values else, the configuration will affect other tests in progress
            Config.Configure(Config.Environment.SANDBOX, "sriduth_sandbox_test", "4D94B6287BE459EB735EE9AB0DA212");
            order2 = order2.Result.Response;
            Assert.Equal(order2["order_id"].ToString(), o2);
        }
    }
}
