using Xunit;
using System;
using Juspay;
using System.Collections.Generic;
using System.IO;

namespace JuspayTest {
    public class SessionTest {


        public static void JuspaySessionAPITest()
        {    
            string customerId = $"customer_{JuspayServiceTest.Rnd.Next()}";
            string orderId = $"order_{JuspayServiceTest.Rnd.Next()}";
            CreateOrderSessionInput createOrderSessionInput = new CreateOrderSessionInput(new Dictionary<string, object>{{ "amount", "10.00" }, { "order_id", orderId }, { "customer_id", customerId }, { "payment_page_client_id", JuspayEnvironment.MerchantId }, { "action", "paymentPage" }, { "return_url", "https://google.com" }});
            JuspayResponse sessionRes = new OrderSession().Create(createOrderSessionInput, null);
            Assert.NotNull(sessionRes);
            Assert.NotNull(sessionRes.Response);
            Assert.NotNull(sessionRes.ResponseBase);
            Assert.NotNull(sessionRes.RawContent);
            Assert.NotNull(sessionRes.Response.id);
            Assert.IsType<JuspayResponse>(sessionRes);
        }

        public static void JuspaySessionAPIJWTTestGlobal()
        {    
            string customerId = $"customer_{JuspayServiceTest.Rnd.Next()}";
            string orderId = $"order_{JuspayServiceTest.Rnd.Next()}";
            string privateKey1 = File.ReadAllText("../../../privateKey1.pem");
            string publicKey2 = File.ReadAllText("../../../publicKey2.pem");
            JuspayEnvironment.JuspayJWT =  new JuspayJWTRSA("key_26b1a82e16cf4c6e850325c3d98368cb", publicKey2, privateKey1);
            try
            {
                CreateOrderSessionInput sessionInput = new CreateOrderSessionInput(new Dictionary<string, object>
                    {
                        { "amount", "10.00" },
                        { "order_id", orderId },
                        { "customer_id", customerId },
                        { "payment_page_client_id", JuspayEnvironment.MerchantId },
                        { "action", "paymentPage" },
                        { "return_url", "https://google.com" }
                    }
                );
                JuspayResponse sessionRes = new OrderSession().Create(sessionInput, null);
                Assert.NotNull(sessionRes);
                Assert.NotNull(sessionRes.Response);
                Assert.NotNull(sessionRes.ResponseBase);
                Assert.NotNull(sessionRes.RawContent);
                Assert.NotNull(sessionRes.Response.id);
                Assert.IsType<JuspayResponse>(sessionRes);
            }
            catch (JuspayException)
            {
                JuspayEnvironment.JuspayJWT = null;
                Assert.True(false);
            }
            JuspayEnvironment.JuspayJWT = null;
        }

        public static void JuspaySessionAPIJWTTest()
        {    
            string customerId = $"customer_{JuspayServiceTest.Rnd.Next()}";
            string orderId = $"order_{JuspayServiceTest.Rnd.Next()}";
            string privateKey1 = File.ReadAllText("../../../privateKey1.pem");
            string publicKey2 = File.ReadAllText("../../../publicKey2.pem");
            CreateOrderSessionInput sessionInput = new CreateOrderSessionInput(new Dictionary<string, object>
                    {
                        { "amount", "10.00" },
                        { "order_id", orderId },
                        { "customer_id", customerId },
                        { "payment_page_client_id", JuspayEnvironment.MerchantId },
                        { "action", "paymentPage" },
                        { "return_url", "https://google.com" }
                    }
                );
            JuspayResponse sessionRes = new OrderSession().Create(sessionInput,  new RequestOptions(null, null, null, null, new JuspayJWTRSA("key_26b1a82e16cf4c6e850325c3d98368cb", publicKey2, privateKey1)));
            Assert.NotNull(sessionRes);
            Assert.NotNull(sessionRes.Response);
            Assert.NotNull(sessionRes.ResponseBase);
            Assert.NotNull(sessionRes.RawContent);
            Assert.NotNull(sessionRes.Response.id);
            Assert.IsType<JuspayResponse>(sessionRes);
        }

        public static void JuspaySessionAPIAsyncTest()
        {    
            string customerId = CustomerTest.CreateCustomerWithOutClientAuthToken();
            string orderId = OrderTest.CreateOrderTest();
            CreateOrderSessionInput sessionInput = new CreateOrderSessionInput(new Dictionary<string, object>
                    {
                        { "amount", "10.00" },
                        { "order_id", orderId },
                        { "customer_id", customerId },
                        { "payment_page_client_id", JuspayEnvironment.MerchantId },
                        { "action", "paymentPage" },
                        { "return_url", "https://google.com" }
                    }
            );
            JuspayResponse sessionRes = new OrderSession().CreateAsync(sessionInput, null).ConfigureAwait(false).GetAwaiter().GetResult();
            Assert.NotNull(sessionRes);
            Assert.NotNull(sessionRes.Response);
            Assert.NotNull(sessionRes.ResponseBase);
            Assert.NotNull(sessionRes.RawContent);
            Assert.NotNull(sessionRes.Response.id);
            Assert.IsType<JuspayResponse>(sessionRes);
        }

        public static void TestOrderSession() {
            JuspaySessionAPITest();
            JuspaySessionAPIAsyncTest();
            JuspaySessionAPIJWTTest();
            JuspaySessionAPIJWTTestGlobal();
        }
    }
}