using Xunit;
using System;
using Juspay;
using System.Collections.Generic;

namespace JuspayTest {
    public class SessionTest {


        public static void JuspaySessionAPITest()
        {    
            string customerId = CustomerTest.CreateCustomerWithOutClientAuthToken();
            string orderId = OrderTest.OrderWithMetadata();
            CreateSessionInput sessionInput = JuspayEntity.FromJson<CreateSessionInput>($"{{\n\"amount\":\"10.00\",\n\"order_id\":\"{orderId}\",\n\"customer_id\":\"{customerId}\",\n\"payment_page_client_id\":\"{JuspayEnvironment.MerchantId}\",\n\"action\":\"paymentPage\",\n\"return_url\": \"https://google.com\"\n}}");
            JuspayResponse sessionRes = new SessionService().CreateSession(sessionInput, null);
            Console.WriteLine(sessionRes.Response);
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
            CreateSessionInput sessionInput = JuspayEntity.FromJson<CreateSessionInput>($"{{\n\"amount\":\"10.00\",\n\"order_id\":\"{orderId}\",\n\"customer_id\":\"{customerId}\",\n\"payment_page_client_id\":\"{JuspayEnvironment.MerchantId}\",\n\"action\":\"paymentPage\",\n\"return_url\": \"https://google.com\"\n}}");
            JuspayResponse sessionRes = new SessionService().CreateSessionAsync(sessionInput, null).ConfigureAwait(false).GetAwaiter().GetResult();
            Assert.NotNull(sessionRes);
            Assert.NotNull(sessionRes.Response);
            Assert.NotNull(sessionRes.ResponseBase);
            Assert.NotNull(sessionRes.RawContent);
            Assert.NotNull(sessionRes.Response.id);
            Assert.IsType<JuspayResponse>(sessionRes);
        }

        public static void TestSessionService() {
            JuspaySessionAPITest();
            // JuspaySessionAPIAsyncTest();
        }
    }
}