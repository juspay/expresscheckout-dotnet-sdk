using Xunit;
using Juspay;
using System.Text.Json;

namespace JuspayTest.Services
{
    [Collection("Sequential")]
    public class JuspayServiceTest
    {
        public JuspayServiceTest () {
            JuspayEnvironment.ApiKey = Environment.GetEnvironmentVariable("API_KEY");
        }
        [Fact]
        public void JuspaySessionAPITest()
        {
            CreateSessionInput sessionInputFromJson = JuspayEntity.FromJson<CreateSessionInput>("{\n\"amount\":\"10.00\",\n\"order_id\":\"tes_1680679317\",\n\"customer_id\":\"cst_9uiehncjizlfcnps\",\n\"payment_page_client_id\":\"azharamin\",\n\"action\":\"paymentPage\",\n\"return_url\": \"https://google.com\"\n}");
            JuspayEntity sessionInput = new JuspayEntity { DictionaryObject = JsonSerializer.Deserialize<Dictionary<string, object>>("{\n\"amount\":\"10.00\",\n\"order_id\":\"tes_1680679317\",\n\"customer_id\":\"cst_9uiehncjizlfcnps\",\n\"payment_page_client_id\":\"azharamin\",\n\"action\":\"paymentPage\",\n\"return_url\": \"https://google.com\"\n}") };
            JuspayEntity sessionRes = new SessionService().CreateSession(sessionInput, new RequestOptions("azhar_test", null, null, null));
            Assert.NotNull(sessionRes);
            Assert.IsType<SessionResponse>(sessionRes);
            Console.WriteLine(sessionRes.ToJson());
        }
        [Fact]
        public string CreateOrderTest() {
            string orderId = $"order_{new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds()}";
            JuspayEntity createOrderInput = new JuspayEntity { DictionaryObject = new Dictionary<string, object> { {"order_id", $"{orderId}"},  {"amount", 10 } } };
            Console.WriteLine(createOrderInput.ToJson());
            JuspayEntity order = new OrderService().CreateOrder(createOrderInput, new RequestOptions("azhar_test", null, null, null));
            Console.WriteLine(order.ToJson());
            Assert.NotNull(order);
            Assert.IsType<OrderResponse>(order);
            return orderId;
        }
        
        [Fact]
        public void GetOrderTest() {
            string orderId = CreateOrderTest();
            Console.WriteLine($"order created {orderId}");
            JuspayEntity orderStatus = new OrderService().GetOrder(orderId, new RequestOptions("azhar_test", null, null, null));
            Console.WriteLine($"order status => {orderStatus.ToJson()}");
            Assert.NotNull(orderStatus);
        }

        // [Fact]
        public void RefundOrderTest() {
            string orderId = CreateOrderTest();
            string reqId = $"req_{new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds()}";
            Console.WriteLine($"order created  refund it {orderId}");
            JuspayEntity refundOrder = new JuspayEntity { DictionaryObject = new Dictionary<string, object> { {"order_id", $"{orderId}"}, {"unique_request_id", $"{reqId}"}, { "amount", 10 } } };
            JuspayEntity refundStatus = new OrderService().RefundOrder(orderId, refundOrder, new RequestOptions("azhar_test", null, null, null));
            Console.WriteLine($"refund status => {refundStatus.ToJson()}");
        }
    }
}