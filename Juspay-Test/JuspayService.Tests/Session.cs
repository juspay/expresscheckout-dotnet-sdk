using Xunit;
using System;
using Juspay;
using System.Collections.Generic;

namespace JuspayTest {
    public class SessionTest {
        public static void SessionResponseEntityTest() {
            string session = "{\"status\":\"NEW\",\"id\":\"ordeh_93556a1f1e9d48bfb916d2f525cbebb2\",\"order_id\":\"tes_1680679317\",\"payment_links\":{\"web\":\"https://sandbox.juspay.in/r/BkktuI\",\"expiry\":\"2023-05-23T06:58:17Z\"},\"sdk_payload\":{\"requestId\":\"91ab580fb32c4809ab8bb0154176df5e\",\"service\":\"in.juspay.hyperpay\",\"payload\":{\"clientId\":\"azharamin\",\"amount\":\"10.0\",\"merchantId\":\"azhar_test\",\"clientAuthToken\":\"tkn_9eb6279a0637488e988cb60f5450017f\",\"clientAuthTokenExpiry\":\"2023-06-02T08:10:02Z\",\"environment\":\"sandbox\",\"action\":\"paymentPage\",\"customerId\":\"cst_9uiehncjizlfcnps\",\"returnUrl\":\"https://google.com\",\"currency\":\"INR\",\"customerPhone\":\"8778321765\",\"customerEmail\":\"malav.shah@juspay.in\",\"orderId\":\"tes_1680679317\"},\"expiry\":\"2023-05-23T06:58:17Z\"}}";
            SessionResponse Session = JuspayResponse.FromJson<SessionResponse>(session);
            Assert.True(Session.Status == "NEW");
            Assert.True(Session.SdkPayload.RequestId == "91ab580fb32c4809ab8bb0154176df5e");
            Assert.True(Session.SdkPayload.RequestId == (string)(Session.Response["sdk_payload"] as Dictionary<string, object>)["requestId"]);
        }

        public static void SessionResponseSetterEntityTest() {
            string session = "{\"status\":\"NEW\",\"id\":\"ordeh_93556a1f1e9d48bfb916d2f525cbebb2\",\"order_id\":\"tes_1680679317\",\"payment_links\":{\"web\":\"https://sandbox.juspay.in/r/BkktuI\",\"expiry\":\"2023-05-23T06:58:17Z\"},\"sdk_payload\":{\"requestId\":\"91ab580fb32c4809ab8bb0154176df5e\",\"service\":\"in.juspay.hyperpay\",\"payload\":{\"clientId\":\"azharamin\",\"amount\":\"10.0\",\"merchantId\":\"azhar_test\",\"clientAuthToken\":\"tkn_9eb6279a0637488e988cb60f5450017f\",\"clientAuthTokenExpiry\":\"2023-06-02T08:10:02Z\",\"environment\":\"sandbox\",\"action\":\"paymentPage\",\"customerId\":\"cst_9uiehncjizlfcnps\",\"returnUrl\":\"https://google.com\",\"currency\":\"INR\",\"customerPhone\":\"8778321765\",\"customerEmail\":\"malav.shah@juspay.in\",\"orderId\":\"tes_1680679317\"},\"expiry\":\"2023-05-23T06:58:17Z\"}}";
            SessionResponse Session = JuspayResponse.FromJson<SessionResponse>(session);
            Session.SdkPayload.RequestId = "123";
            Assert.True(Session.SdkPayload.RequestId == "123");
            Assert.True(Session.SdkPayload.RequestId == (string)(Session.Response["sdk_payload"] as Dictionary<string, object>)["requestId"]);
        }

        public static void JuspaySessionAPITest()
        {    
            CreateSessionInput sessionInput = JuspayEntity.FromJson<CreateSessionInput>("{\n\"amount\":\"10.00\",\n\"order_id\":\"tes_1680679317\",\n\"customer_id\":\"cst_9uiehncjizlfcnps\",\n\"payment_page_client_id\":\"azharamin\",\n\"action\":\"paymentPage\",\n\"return_url\": \"https://google.com\"\n}");
            SessionResponse sessionRes = new SessionService().CreateSession(sessionInput, new RequestOptions("azhar_test", null, null, null));
            Assert.NotNull(sessionRes);
            Assert.NotNull(sessionRes.Response);
            Assert.NotNull(sessionRes.ResponseBase);
            Assert.NotNull(sessionRes.RawContent);
            Assert.NotNull(sessionRes.Id);
            Assert.IsType<SessionResponse>(sessionRes);
        }

        public static void JuspaySessionAPIAsyncTest()
        {    
            CreateSessionInput sessionInput = JuspayEntity.FromJson<CreateSessionInput>("{\n\"amount\":\"10.00\",\n\"order_id\":\"tes_1680679317\",\n\"customer_id\":\"cst_9uiehncjizlfcnps\",\n\"payment_page_client_id\":\"azharamin\",\n\"action\":\"paymentPage\",\n\"return_url\": \"https://google.com\"\n}");
            SessionResponse sessionRes = new SessionService().CreateSessionAsync(sessionInput, new RequestOptions("azhar_test", null, null, null)).ConfigureAwait(false).GetAwaiter().GetResult();
            Assert.NotNull(sessionRes);
            Assert.NotNull(sessionRes.Response);
            Assert.NotNull(sessionRes.ResponseBase);
            Assert.NotNull(sessionRes.RawContent);
            Assert.NotNull(sessionRes.Id);
            Assert.IsType<SessionResponse>(sessionRes);
        }

        public static void TestSessionService() {
            JuspaySessionAPITest();
            SessionResponseEntityTest();
            SessionResponseSetterEntityTest();
            JuspaySessionAPIAsyncTest();
        }
    }
}