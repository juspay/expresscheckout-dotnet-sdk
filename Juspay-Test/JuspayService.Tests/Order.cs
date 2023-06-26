using Xunit;
using System;
using System.Collections.Generic;
using Juspay;

namespace JuspayTest {
    public class OrderTest {

        public static void CreateOrderWithMetadataEntityTest() {
           string orderId = $"order_{JuspayServiceTest.Rnd.Next()}";
            OrderCreate CreateOrderInput = new OrderCreate(new Dictionary<string, object> { {"order_id", $"{orderId}"},  {"amount", 10 }, {"metadata", new Dictionary<string, object>(){ { "value1", 100 } }} } ); 
            Assert.True((int)CreateOrderInput.Metadata["value1"] == 100);
        }

        public static string CreateOrderTest() 
        {
            string orderId = $"order_{JuspayServiceTest.Rnd.Next()}";
            OrderCreate createOrderInput = new OrderCreate(new Dictionary<string, object> { {"order_id", $"{orderId}"},  {"amount", 1 } } );
            JuspayResponse order = new OrderService().CreateOrder(createOrderInput, null);
            Assert.NotNull(order);
            Assert.NotNull(order.Response);
            Assert.NotNull(order.ResponseBase);
            Assert.NotNull(order.RawContent);
            Assert.NotNull(order.Response.order_id);
            Assert.IsType<JuspayResponse>(order);
            return orderId;
        }

        public static string CreateOrderTestAsync()
        {
            string orderId = $"order_{JuspayServiceTest.Rnd.Next()}";
            OrderCreate createOrderInput = new OrderCreate(new Dictionary<string, object> { {"order_id", $"{orderId}"},  {"amount", 10 } } );
            JuspayResponse order = new OrderService().CreateOrderAsync(createOrderInput, new RequestOptions(JuspayEnvironment.MerchantId, null, null, null)).ConfigureAwait(false).GetAwaiter().GetResult();
            Assert.NotNull(order);
            Assert.NotNull(order.Response);
            Assert.NotNull(order.ResponseBase);
            Assert.NotNull(order.RawContent);
            Assert.NotNull(order.Response.order_id);
            Assert.IsType<JuspayResponse>(order);
            return orderId;
        }
        
        public static void UpdateOrderTest()
        {
            string orderId = CreateOrderTest();
            JuspayResponse order = new OrderService().UpdateOrder(orderId, 99.99, null);
            Assert.True((string)order.Response.order_id == orderId);
            Assert.True((double)order.Response.amount == 99.99);
        }

        public static void UpdateOrderAsyncTest()
        {
            string orderId = CreateOrderTest();
            JuspayResponse order = new OrderService().UpdateOrderAsync(orderId, 99.99, null).ConfigureAwait(false).GetAwaiter().GetResult();;
            Assert.True((string)order.Response.order_id == orderId);
            Assert.True((double)order.Response.amount == 99.99);
        }

        public static void GetOrderTest() 
        {
            string orderId = CreateOrderTest();
            JuspayResponse orderStatus = new OrderService().GetOrder(orderId, null, new RequestOptions(JuspayEnvironment.MerchantId, null, null, null));
            Assert.NotNull(orderStatus);
            Assert.NotNull(orderStatus.Response);
            Assert.NotNull(orderStatus.ResponseBase);
            Assert.NotNull(orderStatus.RawContent);
            Assert.IsType<JuspayResponse>(orderStatus);
            // Async Test
            orderStatus = new OrderService().GetOrderAsync(orderId, null).ConfigureAwait(false).GetAwaiter().GetResult();
            Assert.NotNull(orderStatus);
            Assert.NotNull(orderStatus.Response);
            Assert.NotNull(orderStatus.ResponseBase);
            Assert.NotNull(orderStatus.RawContent);
            Assert.IsType<JuspayResponse>(orderStatus);
        }

        public static void InstantRefundTest()
        {
            string orderId = CreateOrderTest();
            int amount = 1;
            if (Environment.GetEnvironmentVariable("REFUND_AMOUNT") != null)
            {
                amount = int.Parse(Environment.GetEnvironmentVariable("REFUND_AMOUNT"));
            }
            string uniqueRequestId = $"request_{JuspayServiceTest.Rnd.Next()}";
            TransactionIdAndInstantRefund RefundInput = new TransactionIdAndInstantRefund(new Dictionary<string, object> { { "order_id", orderId }, {"amount", amount }, {"unique_request_id", uniqueRequestId }, { "order_type", "Juspay" }, {"refund_type", "STANDARD"} });
            try
            {
                JuspayResponse refundResponse = new InstantRefundService().GetTransactionIdAndInstantRefund(RefundInput, null);
            }
            catch (JuspayException Ex)
            {
                Assert.NotNull(Ex.JuspayError);
                Assert.NotNull(Ex.JuspayError.ErrorMessage);
                Assert.NotNull(Ex.JuspayResponse.RawContent);
            }
            
        }



        public static void InstantRefundAsyncTest()
        {
            string orderId = CreateOrderTest();
            string uniqueRequestId = $"request_{JuspayServiceTest.Rnd.Next()}";
            TransactionIdAndInstantRefund RefundInput = new TransactionIdAndInstantRefund(new Dictionary<string, object> { { "order_id", orderId }, {"amount", 10 }, {"unique_request_id", uniqueRequestId }, { "order_type", "Juspay" }, {"refund_type", "STANDARD"} });
            try
            {
                JuspayResponse refundResponse = new InstantRefundService().GetTransactionIdAndInstantRefundAsync(RefundInput, null).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (JuspayException Ex)
            {
                Assert.NotNull(Ex.JuspayError);
                Assert.NotNull(Ex.JuspayError.ErrorMessage);
                Assert.NotNull(Ex.JuspayResponse.RawContent);
            }
            
        }

        public static void CreateOrderSpecialCharTest()
        {
            try
            {
                string orderId = $"order_{JuspayServiceTest.Rnd.Next()}_$#@!&*";
                OrderCreate createOrderInput = new OrderCreate(new Dictionary<string, object> { {"order_id", $"{orderId}"},  {"amount", 10 } } );
                JuspayResponse order = new OrderService().CreateOrderAsync(createOrderInput, new RequestOptions(JuspayEnvironment.MerchantId, null, null, null)).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (JuspayException Ex)
            {
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
                Console.WriteLine(Ex.JuspayError.ErrorMessage);
            }
           
        }

        public static string CreateOrderFloatTest()
        {
            string orderId = $"{JuspayServiceTest.Rnd.NextDouble()}";
            OrderCreate createOrderInput = new OrderCreate(new Dictionary<string, object> { {"order_id", $"{orderId}"},  {"amount", 10 } } );
            JuspayResponse order = new OrderService().CreateOrderAsync(createOrderInput, new RequestOptions(JuspayEnvironment.MerchantId, null, null, null)).ConfigureAwait(false).GetAwaiter().GetResult();
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            Console.WriteLine(order.Response);
            Assert.NotNull(order);
            Assert.NotNull(order.Response);
            Assert.NotNull(order.ResponseBase);
            Assert.NotNull(order.RawContent);
            Assert.NotNull(order.Response.order_id);
            Assert.True(order.Response.order_id == orderId);
            return orderId;
        }

        public static string CreateOrderAmountTest()
        {
            string orderId = $"order_{JuspayServiceTest.Rnd.Next()}";
            OrderCreate createOrderInput = new OrderCreate(new Dictionary<string, object> { {"order_id", $"{orderId}"},  {"amount", "10" } } );
            JuspayResponse order = new OrderService().CreateOrderAsync(createOrderInput, new RequestOptions(JuspayEnvironment.MerchantId, null, null, null)).ConfigureAwait(false).GetAwaiter().GetResult();
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            Console.WriteLine(order.Response);
            Assert.NotNull(order);
            Assert.NotNull(order.Response);
            Assert.NotNull(order.ResponseBase);
            Assert.NotNull(order.RawContent);
            Assert.NotNull(order.Response.order_id);
            return orderId;
        }

        public static string CreateOrderAmountFloatTest()
        {
            string orderId = $"order_{JuspayServiceTest.Rnd.Next()}";
            OrderCreate createOrderInput = new OrderCreate(new Dictionary<string, object> { {"order_id", $"{orderId}"},  {"amount", "1.026456" } } );
            JuspayResponse order = new OrderService().CreateOrderAsync(createOrderInput, new RequestOptions(JuspayEnvironment.MerchantId, null, null, null)).ConfigureAwait(false).GetAwaiter().GetResult();
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            Console.WriteLine(order.Response);
            Assert.NotNull(order);
            Assert.NotNull(order.Response);
            Assert.NotNull(order.ResponseBase);
            Assert.NotNull(order.RawContent);
            Assert.NotNull(order.Response.order_id);
            return orderId;
        }

        public static void OrderAmountFloatStatus()
        {
            string orderId = CreateOrderAmountFloatTest();
            JuspayResponse orderStatus = new OrderService().GetOrder(orderId, null);
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            Console.WriteLine(orderStatus.Response);
            Assert.NotNull(orderStatus);
            Assert.NotNull(orderStatus.Response);
            Assert.True(orderStatus.Response.order_id == orderId);
            Assert.True(orderStatus.Response.amount == 1.02);   
        }

        public static void VersionOrderStatus()
        {
            string orderId = CreateOrderAmountTest();
            string version = JuspayEnvironment.API_VERSION;
            Console.WriteLine(version);
            JuspayResponse orderStatus = new OrderService().GetOrder(orderId, null);
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            Console.WriteLine(orderStatus.Response);
            Assert.NotNull(orderStatus);
            Assert.NotNull(orderStatus.Response);
            Assert.True(orderStatus.Response.order_id == orderId);
            Assert.True(orderStatus.Response.amount == 10);
            Assert.NotNull(orderStatus.Response.metadata);
            JuspayEnvironment.API_VERSION = "2015-01-09";
            orderStatus = new OrderService().GetOrder(orderId, null);
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            Console.WriteLine(orderStatus.Response);
            Assert.NotNull(orderStatus);
            Assert.NotNull(orderStatus.Response);
            Assert.True(orderStatus.Response.order_id == orderId);
            Assert.True(orderStatus.Response.amount == 10);
            Assert.Null(orderStatus.Response.metadata);
            JuspayEnvironment.API_VERSION = version;
        }
        public static void OrderAmountStatus()
        {
            string orderId = CreateOrderAmountTest();
            JuspayResponse orderStatus = new OrderService().GetOrder(orderId, null);
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            Console.WriteLine(orderStatus.Response);
            Assert.NotNull(orderStatus);
            Assert.NotNull(orderStatus.Response);
            Assert.True(orderStatus.Response.order_id == orderId);
            Assert.True(orderStatus.Response.amount == 10);
        }

        public static string OrderWithMetadata()
        {
            string orderId = $"order_{JuspayServiceTest.Rnd.Next()}";
            OrderCreate createOrderInput = new OrderCreate(new Dictionary<string, object> { {"order_id", $"{orderId}"},  {"amount", 1 }, {"gateway_id", "23"}, { "metadata", new Dictionary<string, object> { { "RAZORPAY:gateway_reference_id", "rp_tpv_test" } } } } );
            JuspayResponse order = new OrderService().CreateOrderAsync(createOrderInput, new RequestOptions(JuspayEnvironment.MerchantId, null, null, null)).ConfigureAwait(false).GetAwaiter().GetResult();
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            Console.WriteLine(order.Response);
            Assert.NotNull(order);
            Assert.NotNull(order.Response);
            Assert.NotNull(order.ResponseBase);
            Assert.NotNull(order.RawContent);
            Assert.NotNull(order.Response.order_id);
            return orderId;
        }

        public static void OrderWithMetadataStatus()
        {
            string orderId = OrderWithMetadata();
            JuspayResponse orderStatus = new OrderService().GetOrder(orderId, null);
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            Console.WriteLine(orderStatus.Response);
            Assert.NotNull(orderStatus);
            Assert.NotNull(orderStatus.Response);
            Assert.True(orderStatus.Response.order_id == orderId);
            Assert.NotNull(orderStatus.Response.metadata);
            Assert.True(orderStatus.Response.metadata["RAZORPAY:gateway_reference_id"] == "rp_tpv_test");
        }

        public static string OrderWithBasket()
        {
            string orderId = $"order_{JuspayServiceTest.Rnd.Next()}";
            OrderCreate createOrderInput = new OrderCreate(new Dictionary<string, object> { {"order_id", $"{orderId}"},  {"amount", 1 }, { "basket", "[{\"unitPrice\": 10000.00,\"sku\": \"12345\",\"sellerType\": \"VENDOR\",\"quantity\": 1,\"productUrl\": \"[https://www.abc.def/xyz](https://www.abc.def/xyz)\",\"id\": \"Kotak\",\"description\": \"SampleDescription\",\"customParams\": {\"name1\": \"value1\",\"name2\": \"value2\"},\"category\": \"Electronics\",\"passenger\": {\"type\": \"ADT\", \"status\": \"Standard\", \"phone\": \"9620123734\",\"firstName\": \"Anie\",\"lastName\": \"Malar\",\"email\": \"customer@juspay.in\",\"nationality\": \"IN\",\"id\": \"12321211\" },\"productCode\": \"Service\", \"productRisk\": \"normal\", \"productName\":\"Flight ticket\", \"productDescription\": \"Flight ticket\",\"shippingDestinationType\": \"Commercial\" }]"} } );
            JuspayResponse order = new OrderService().CreateOrderAsync(createOrderInput, new RequestOptions(JuspayEnvironment.MerchantId, null, null, null)).ConfigureAwait(false).GetAwaiter().GetResult();
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            Console.WriteLine(order.Response);
            Assert.NotNull(order);
            Assert.NotNull(order.Response);
            Assert.NotNull(order.ResponseBase);
            Assert.NotNull(order.RawContent);
            Assert.NotNull(order.Response.order_id);
            return orderId;   
        }

        public static void OrderWithBasketStatus()
        {
            string orderId = OrderWithBasket();
            JuspayResponse orderStatus = new OrderService().GetOrder(orderId, null);
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            Console.WriteLine(orderStatus.Response);
            Assert.NotNull(orderStatus);
            Assert.NotNull(orderStatus.Response);
            Assert.True(orderStatus.Response.order_id == orderId);
            Assert.NotNull(orderStatus.Response.basket);
        }

        public static string OrderWithShippingDetails()
        {
            string orderId = $"order_{JuspayServiceTest.Rnd.Next()}";
            Dictionary<string, object> requestData = new Dictionary<string, object>()
            {
                {"order_id", orderId},
                {"amount", "1.00"},
                {"currency", "INR"},
                {"customer_id", "9443232345"},
                {"customer_email", "helloworld@email.com"},
                {"customer_phone", "1234567890"},
                {"product_id", "prod-141833"},
                {"return_url", "www.google.co.in"},
                {"description", "Sample description"},
                {"billing_address_first_name", "Juspay"},
                {"billing_address_last_name", "Technologies"},
                {"billing_address_line1", "Girija Building"},
                {"billing_address_line2", "Ganapathi Temple Road"},
                {"billing_address_line3", "8th Block, Koramangala"},
                {"billing_address_city", "Bengaluru"},
                {"billing_address_state", "Karnataka"},
                {"billing_address_country", "India"},
                {"billing_address_postal_code", "560095"},
                {"billing_address_phone", "1234567890"},
                {"billing_address_country_code_iso", "IND"},
                {"shipping_address_first_name", "Juspay"},
                {"shipping_address_last_name", "Technologies"},
                {"shipping_address_line1", "Girija Building"},
                {"shipping_address_line2", "Ganapathi Temple Road"},
                {"shipping_address_line3", "8th Block, Koramangala"},
                {"shipping_address_city", "Bengaluru"},
                {"shipping_address_state", "Karnataka"},
                {"shipping_address_postal_code", "560095"},
                {"shipping_address_phone", "123456789"},
                {"shipping_address_country_code_iso", "IND"},
                {"shipping_address_country", "India"},
                {"gateway_id", "23"}
            };
            OrderCreate createOrderInput = new OrderCreate(requestData);
            JuspayResponse order = new OrderService().CreateOrderAsync(createOrderInput, new RequestOptions(JuspayEnvironment.MerchantId, null, null, null)).ConfigureAwait(false).GetAwaiter().GetResult();
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            Console.WriteLine(order.Response);
            Assert.NotNull(order);
            Assert.NotNull(order.Response);
            Assert.NotNull(order.ResponseBase);
            Assert.NotNull(order.RawContent);
            Assert.NotNull(order.Response.order_id);
            return orderId;   
        }

        public static void OrderWithShippingDetailsStatus()
        {
            string orderId = OrderWithShippingDetails();
            JuspayResponse orderStatus = new OrderService().GetOrder(orderId, null);
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            Console.WriteLine(orderStatus.Response);
            Assert.NotNull(orderStatus);
            Assert.NotNull(orderStatus.Response);
            Assert.True(orderStatus.Response.order_id == orderId);
        }

        public static string OrderWithClientId()
        {
            string orderId = $"order_{JuspayServiceTest.Rnd.Next()}";
            OrderCreate createOrderInput = new OrderCreate(new Dictionary<string, object> { {"order_id", $"{orderId}"},  {"amount", 1 }, { "payment_page_client_id",  "neoinsurance" }, { "payment_page_environment", "sandbox" } });
            JuspayResponse order = new OrderService().CreateOrderAsync(createOrderInput, new RequestOptions(JuspayEnvironment.MerchantId, null, null, null)).ConfigureAwait(false).GetAwaiter().GetResult();
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            Console.WriteLine(order.Response);
            Assert.NotNull(order);
            Assert.NotNull(order.Response);
            Assert.NotNull(order.ResponseBase);
            Assert.NotNull(order.RawContent);
            Assert.NotNull(order.Response.order_id);
            return orderId;
        }
        public static void OrderWithClientIdStatus()
        {
            string orderId = OrderWithClientId();
            JuspayResponse orderStatus = new OrderService().GetOrder(orderId, null);
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            Console.WriteLine(orderStatus.Response);
            Assert.NotNull(orderStatus);
            Assert.NotNull(orderStatus.Response);
            Assert.True(orderStatus.Response.order_id == orderId);
            Assert.True(orderStatus.Response.metadata["payment_page_client_id"] == "neoinsurance");
        }

        public static void orderStatusWithGateWayResponse()
        {
            string orderId = "order_2134032852"; // CreateOrderTest();
            Console.WriteLine(orderId);
            JuspayResponse orderStatus = new OrderService().GetOrder(orderId, new Dictionary<string, object> { { "options", new Dictionary<string, object> { { "add_full_gateway_response", "true" } }}});
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            Console.WriteLine(orderStatus.Response);
            Assert.NotNull(orderStatus);
            Assert.NotNull(orderStatus.Response);
            Assert.True(orderStatus.Response.order_id == orderId);
        }

        public static void orderRefund()
        {
            string orderId = "order_655018375";
            string uniqueRequestId = $"uniq_{JuspayServiceTest.Rnd.Next()}";
            JuspayResponse refund = new OrderService().RefundOrder(orderId, new RefundOrder(new Dictionary<string, object> { {"order_id", orderId}, { "amount", 1 }, { "unique_request_id", uniqueRequestId } }), null);
            Console.WriteLine(refund.Response);
        }
        public static void TestOrderService() {
            // GetOrderTest();
            // CreateOrderWithMetadataEntityTest();
            // CreateOrderTest();
            // CreateOrderTestAsync();
            // InstantRefundTest();
            // InstantRefundAsyncTest();
            // UpdateOrderTest();
            // UpdateOrderAsyncTest();
            // INTEG TEST
            CreateOrderSpecialCharTest();
            CreateOrderFloatTest();
            OrderAmountStatus();
            VersionOrderStatus();
            OrderWithMetadataStatus();
            OrderWithBasket();
            OrderWithBasketStatus();
            OrderWithMetadataStatus();
            OrderWithShippingDetailsStatus();
            OrderWithClientIdStatus();
            orderStatusWithGateWayResponse();
            OrderAmountFloatStatus();
            // orderRefund();
        }
    }
}