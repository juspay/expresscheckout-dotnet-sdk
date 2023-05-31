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
        //[Fact]
        public void JuspaySessionAPITest()
        {
            CreateSessionInput sessionInputFromJson = JuspayEntity.FromJson<CreateSessionInput>("{\n\"amount\":\"10.00\",\n\"order_id\":\"tes_1680679317\",\n\"customer_id\":\"cst_9uiehncjizlfcnps\",\n\"payment_page_client_id\":\"azharamin\",\n\"action\":\"paymentPage\",\n\"return_url\": \"https://google.com\"\n}");
            JuspayEntity sessionInput = new JuspayEntity { Data = JsonSerializer.Deserialize<Dictionary<string, object>>("{\n\"amount\":\"10.00\",\n\"order_id\":\"tes_1680679317\",\n\"customer_id\":\"cst_9uiehncjizlfcnps\",\n\"payment_page_client_id\":\"azharamin\",\n\"action\":\"paymentPage\",\n\"return_url\": \"https://google.com\"\n}") };
            JuspayResponse sessionRes = new SessionService().CreateSession(sessionInput, new RequestOptions("azhar_test", null, null, null));
            Assert.NotNull(sessionRes);
            Assert.IsType<SessionResponse>(sessionRes);
        }
        //[Fact]
        public string CreateOrderTest() {
            string orderId = $"order_{new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds()}";
            JuspayEntity createOrderInput = new JuspayEntity { Data = new Dictionary<string, object> { {"order_id", $"{orderId}"},  {"amount", 10 } } };
            JuspayResponse order = new OrderService().CreateOrder(createOrderInput, new RequestOptions("azhar_test", null, null, null));
            Assert.NotNull(order);
            Assert.IsType<OrderResponse>(order);
            return orderId;
        }
        
        //[Fact]
        public void GetOrderTest() {
            string orderId = CreateOrderTest();
            Console.WriteLine($"order created {orderId}");
            JuspayResponse orderStatus = new OrderService().GetOrder(orderId, new RequestOptions("azhar_test", null, null, null));
            Assert.NotNull(orderStatus);
        }
        // [Fact]
        public void RefundOrderTest() {
            string orderId = CreateOrderTest();
            string reqId = $"req_{new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds()}";
            JuspayEntity refundOrder = new JuspayEntity { Data = new Dictionary<string, object> { {"order_id", $"{orderId}"}, {"unique_request_id", $"{reqId}"}, { "amount", 10 } } };
            JuspayResponse refundStatus = new OrderService().RefundOrder(orderId, refundOrder, new RequestOptions("azhar_test", null, null, null));
        }

        // [Fact]
        public string CreateCustomerWithClientAuthToken() {
            string customerId = $"customer_{new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds()}";
            JuspayEntity createCustomerInput = new JuspayEntity { Data = new Dictionary<string, object>{ {"object_reference_id", $"{customerId}"}, {"mobile_number", "1234567890"}, {"email_address", "customer@juspay.com"}, {"mobile_country_code", "91"} , {"options", new Dictionary<string, object> {{"get_client_auth_token", true }} }}};
            CustomerResponse newCustomer = new CustomerService().CreateCustomer(createCustomerInput, new RequestOptions("azhar_test", null, null, null)) as CustomerResponse;
            Assert.NotNull(newCustomer);
            Assert.NotNull(newCustomer.Juspay.ClientAuthToken);
            Assert.IsType<CustomerResponse>(newCustomer);
            return customerId;
        }
        // [Fact]
        public string CreateCustomerWithOutClientAuthToken() {
            string customerId = $"customer_{new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds()}";  
            JuspayEntity createCustomerInput = new JuspayEntity(new Dictionary<string, object>{ {"object_reference_id", $"{customerId}"}, {"mobile_number", "1234567890"}, {"email_address", "customer@juspay.com"}, {"mobile_country_code", "91"} });
            CustomerResponse newCustomer = new CustomerService().CreateCustomer(createCustomerInput, new RequestOptions("azhar_test", null, null, null)) as CustomerResponse;
            Assert.NotNull(newCustomer);
            Assert.Null(newCustomer.Juspay);
            Assert.IsType<CustomerResponse>(newCustomer);
            return customerId;
        }
        [Fact]
        public void GetCustomer() {
            string customerId = CreateCustomerWithOutClientAuthToken();
            CustomerResponse customer = new CustomerService().GetCustomer(customerId, null, new RequestOptions("azhar_test", null, null, null)) as CustomerResponse;
            Assert.NotNull(customer);
            Assert.IsType<CustomerResponse>(customer);
            Assert.True(customerId == customer.ObjectReferenceId);
        }
    }
}