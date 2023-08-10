using Xunit;
using System;
using Juspay;
using System.Collections.Generic;
using System.IO;

namespace JuspayTest {
    public class SessionTest {


        public static void JuspaySessionAPITest()
        {    
            string customerId = CustomerTest.CreateCustomerWithOutClientAuthToken();
            string orderId = OrderTest.CreateOrderTest();
            CreateOrderSessionInput sessionInput = JuspayEntity.FromJson<CreateOrderSessionInput>($"{{\n\"amount\":\"10.00\",\n\"order_id\":\"{orderId}\",\n\"customer_id\":\"{customerId}\",\n\"payment_page_client_id\":\"{JuspayEnvironment.MerchantId}\",\n\"action\":\"paymentPage\",\n\"return_url\": \"https://google.com\"\n}}");
            JuspayResponse sessionRes = new SessionService().CreateOrderSession(sessionInput, null);
            Assert.NotNull(sessionRes);
            Assert.NotNull(sessionRes.Response);
            Assert.NotNull(sessionRes.ResponseBase);
            Assert.NotNull(sessionRes.RawContent);
            Assert.NotNull(sessionRes.Response.id);
            Assert.IsType<JuspayResponse>(sessionRes);
        }

        public static void JuspaySessionAPIJWTTest()
        {    
            string customerId = CustomerTest.CreateCustomerWithOutClientAuthToken();
            string orderId = OrderTest.CreateOrderTest();
            CreateOrderSessionInput sessionInput = JuspayEntity.FromJson<CreateOrderSessionInput>($"{{\n\"amount\":\"10.00\",\n\"order_id\":\"{orderId}\",\n\"customer_id\":\"{customerId}\",\n\"payment_page_client_id\":\"{JuspayEnvironment.MerchantId}\",\n\"action\":\"paymentPage\",\n\"return_url\": \"https://google.com\"\n}}");
            string privateKey1 = File.ReadAllText("../../../privateKey1.pem");
            string publicKey2 = File.ReadAllText("../../../publicKey2.pem");
            Dictionary<string, object> keys = new Dictionary<string, object> { { "privateKey", new Dictionary<string, object> { {"key", privateKey1 }, { "kid", "testJwe" } }}, { "publicKey", new Dictionary<string, object> { {"key", publicKey2 }, { "kid", "testJwe" } }}};
            JuspayResponse sessionRes = new SessionService().CreateOrderSession(sessionInput, new RequestOptions(null, null, null, null, new JuspayJWTRSA(keys)));
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
            CreateOrderSessionInput sessionInput = JuspayEntity.FromJson<CreateOrderSessionInput>($"{{\n\"amount\":\"10.00\",\n\"order_id\":\"{orderId}\",\n\"customer_id\":\"{customerId}\",\n\"payment_page_client_id\":\"{JuspayEnvironment.MerchantId}\",\n\"action\":\"paymentPage\",\n\"return_url\": \"https://google.com\"\n}}");
            JuspayResponse sessionRes = new SessionService().CreateOrderSessionAsync(sessionInput, null).ConfigureAwait(false).GetAwaiter().GetResult();
            Assert.NotNull(sessionRes);
            Assert.NotNull(sessionRes.Response);
            Assert.NotNull(sessionRes.ResponseBase);
            Assert.NotNull(sessionRes.RawContent);
            Assert.NotNull(sessionRes.Response.id);
            Assert.IsType<JuspayResponse>(sessionRes);
        }

        public static void TestSessionService() {
            JuspaySessionAPITest();
            JuspaySessionAPIAsyncTest();
            JuspaySessionAPIJWTTest();
        }
    }
}