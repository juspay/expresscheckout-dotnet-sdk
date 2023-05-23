using Xunit;
using Juspay;

namespace JuspayTest.Services
{
    public class JuspayServiceTest
    {
        [Fact]
        public void IsPrime_InputIs1_ReturnFalse()
        {
             JuspayEnvironment.ApiKey = "2CEBE8B62BD40908C373FACAD4F605";
            CreateSessionInput sessionInput = JuspayEntity.FromJson<CreateSessionInput>("{\n\"amount\":\"10.00\",\n\"order_id\":\"tes_1680679317\",\n\"customer_id\":\"cst_9uiehncjizlfcnps\",\n\"payment_page_client_id\":\"azharamin\",\n\"action\":\"paymentPage\",\n\"return_url\": \"https://google.com\"\n}");
            SessionResponse sessionRes = new SessionService().CreateSession(sessionInput, new RequestOptions { MerchantId = "azhar_test"});
            Console.WriteLine($"Status: {sessionRes.Status}");
            Assert.True(sessionRes.Status == "NEW");
        }
    }
}