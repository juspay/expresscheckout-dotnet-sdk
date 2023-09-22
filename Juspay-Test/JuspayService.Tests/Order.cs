using Xunit;
using System;
using System.Collections.Generic;
using Juspay;
using System.IO;

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
            OrderCreate createOrderInput = new OrderCreate(new Dictionary<string, object> { {"order_id", $"{orderId}"},  {"amount", 10 } } );
            JuspayResponse order = new OrderService().CreateOrder(createOrderInput, null);
            Assert.NotNull(order);
            Assert.NotNull(order.Response);
            Assert.NotNull(order.ResponseBase);
            Assert.NotNull(order.RawContent);
            Assert.NotNull(order.Response.order_id);
            Assert.IsType<JuspayResponse>(order);
            return orderId;
        }

        public static string CreateOrderWithCustomerIdTest() 
        {
            string customerId = $"customer_{JuspayServiceTest.Rnd.Next()}"; 
            JuspayEntity createCustomerInput = new CreateCustomerInput(new Dictionary<string, object>{ {"object_reference_id", $"{customerId}"}, {"mobile_number", "1234567890"}, {"email_address", "customer@juspay.com"}, {"mobile_country_code", "91"} , {"options", new Dictionary<string, object> {{"get_client_auth_token", true }} }});
            JuspayResponse newCustomer = new CustomerService().CreateCustomer((CreateCustomerInput)createCustomerInput, null);
            string orderId = $"order_{JuspayServiceTest.Rnd.Next()}";
            OrderCreate createOrderInput = new OrderCreate(new Dictionary<string, object> { {"order_id", $"{orderId}"},  {"amount", 10 } } );
            RequestOptions requestOptions = new RequestOptions{ CustomerId = customerId};
            JuspayResponse order = new OrderService().CreateOrder(createOrderInput, requestOptions);
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
            orderStatus = new OrderService().GetOrderAsync(orderId, null, null).ConfigureAwait(false).GetAwaiter().GetResult();
            Assert.NotNull(orderStatus);
            Assert.NotNull(orderStatus.Response);
            Assert.NotNull(orderStatus.ResponseBase);
            Assert.NotNull(orderStatus.RawContent);
            Assert.IsType<JuspayResponse>(orderStatus);
        }

        public static void InstantRefundTest()
        {
            string orderId = CreateOrderTest();
            string uniqueRequestId = $"request_{JuspayServiceTest.Rnd.Next()}";
            TransactionIdAndInstantRefund RefundInput = new TransactionIdAndInstantRefund(new Dictionary<string, object> { { "order_id", orderId }, {"amount", 10 }, {"unique_request_id", uniqueRequestId }, { "order_type", "Juspay" }, {"refund_type", "STANDARD"} });
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
        public static void GetEncryptedOrderTest()
        {
            string orderId = CreateOrderTest();
            string privateKey1 = File.ReadAllText("../../../privateKey1.pem");
            string publicKey2 = File.ReadAllText("../../../publicKey2.pem");
            Dictionary<string, object> keys = new Dictionary<string, object> { { "privateKey", new Dictionary<string, object> { {"key", privateKey1 }, { "kid", "key_26b1a82e16cf4c6e850325c3d98368cb" } }}, { "publicKey", new Dictionary<string, object> { {"key", publicKey2 }, { "kid", "key_26b1a82e16cf4c6e850325c3d98368cb" } }}};
            JuspayResponse orderStatus = new OrderService().GetOrder(orderId, null, new RequestOptions(null, null, null, null, new JuspayJWTRSA(keys)));
            Assert.NotNull(orderStatus);
            Assert.NotNull(orderStatus.Response);
            Assert.NotNull(orderStatus.ResponseBase);
            Assert.NotNull(orderStatus.RawContent);
            Assert.IsType<JuspayResponse>(orderStatus);
        }

        public static void GetEncryptedOrderTestGlobal()
        {
            string orderId = CreateOrderTest();
            string privateKey1 = File.ReadAllText("../../../privateKey1.pem");
            string publicKey2 = File.ReadAllText("../../../publicKey2.pem");
            Dictionary<string, object> keys = new Dictionary<string, object> { { "privateKey", new Dictionary<string, object> { {"key", privateKey1 }, { "kid", "d1a82e16cf4c6e850325c3d98368cb" } }}, { "publicKey", new Dictionary<string, object> { {"key", publicKey2 }, { "kid", "key_26b1a82e16cf4c6e850325c3d98368cb" } }}};
            JuspayEnvironment.JuspayJWT =  new JuspayJWTRSA(keys);
            try
            {
                JuspayResponse orderStatus = new OrderService().GetOrder(orderId, null, null);
                Assert.NotNull(orderStatus);
                Assert.NotNull(orderStatus.Response);
                Assert.NotNull(orderStatus.ResponseBase);
                Assert.NotNull(orderStatus.RawContent);
                Assert.IsType<JuspayResponse>(orderStatus);
            }
            catch (JuspayException)
            {
                JuspayEnvironment.JuspayJWT = null;
                Assert.True(false);
            }
            JuspayEnvironment.JuspayJWT = null;
        }

        public static string RefundOrderTest()
        {
            string orderId = CreateOrderTest();
            string uniqueRequestId = $"request_{JuspayServiceTest.Rnd.Next()}";
            RefundOrder RefundInput = new RefundOrder(new Dictionary<string, object> { { "order_id", orderId }, {"amount", 10 }, {"unique_request_id", uniqueRequestId } });
            try
            {
                JuspayResponse refundResponse = new OrderService().RefundOrder(orderId, RefundInput, null);
                return orderId;
            }
            catch (JuspayException Ex)
            {
                Assert.NotNull(Ex.JuspayError);
                Assert.NotNull(Ex.JuspayError.ErrorMessage);
                Assert.True(Ex.JuspayError.ErrorMessage == "Cannot process unsuccessful order.");
                Assert.NotNull(Ex.JuspayResponse.RawContent);
                return orderId;
            }
        }
        public static void EncryptedRefundOrderTest()
        {
            string orderId = CreateOrderTest();
            string uniqueRequestId = $"request_{JuspayServiceTest.Rnd.Next()}";
            string privateKey1 = File.ReadAllText("../../../privateKey1.pem");
            string publicKey2 = File.ReadAllText("../../../publicKey2.pem");
            Dictionary<string, object> keys = new Dictionary<string, object> { { "privateKey", new Dictionary<string, object> { {"key", privateKey1 }, { "kid", "key_26b1a82e16cf4c6e850325c3d98368cb" } }}, { "publicKey", new Dictionary<string, object> { {"key", publicKey2 }, { "kid", "key_26b1a82e16cf4c6e850325c3d98368cb" } }}};
            RefundOrder RefundInput = new RefundOrder(new Dictionary<string, object> { { "order_id", orderId }, {"amount", 10 }, {"unique_request_id", uniqueRequestId } });
            try
            {
                JuspayResponse refundResponse = new OrderService().RefundOrder(orderId, RefundInput, new RequestOptions(null, null, null, null, new JuspayJWTRSA(keys)));
            }
            catch (JuspayException Ex)
            {
                Assert.NotNull(Ex.JuspayError);
                Assert.NotNull(Ex.JuspayError.ErrorMessage);
                Assert.True(Ex.JuspayError.ErrorMessage == "Cannot process unsuccessful order.");
                Assert.NotNull(Ex.JuspayResponse.RawContent);
            }
        }
        public static void EncryptedRefundOrderTestGlobal()
        {
            string orderId = CreateOrderTest();
            string uniqueRequestId = $"request_{JuspayServiceTest.Rnd.Next()}";
            string privateKey1 = File.ReadAllText("../../../privateKey1.pem");
            string publicKey2 = File.ReadAllText("../../../publicKey2.pem");
            Dictionary<string, object> keys = new Dictionary<string, object> { { "privateKey", new Dictionary<string, object> { {"key", privateKey1 }, { "kid", "key_26b1a82e16cf4c6e850325c3d98368cb" } }}, { "publicKey", new Dictionary<string, object> { {"key", publicKey2 }, { "kid", "key_26b1a82e16cf4c6e850325c3d98368cb" } }}};
            RefundOrder RefundInput = new RefundOrder(new Dictionary<string, object> { { "order_id", orderId }, {"amount", 10 }, {"unique_request_id", uniqueRequestId } });
            JuspayEnvironment.JuspayJWT =  new JuspayJWTRSA(keys);
            try
            {
                JuspayResponse refundResponse = new OrderService().RefundOrder(orderId, RefundInput, null);
            }
            catch (JuspayException Ex)
            {
                Assert.NotNull(Ex.JuspayError);
                Assert.NotNull(Ex.JuspayError.ErrorMessage);
                Assert.True(Ex.JuspayError.ErrorMessage == "Cannot process unsuccessful order.");
                Assert.NotNull(Ex.JuspayResponse.RawContent);
            }
            JuspayEnvironment.JuspayJWT = null;
        }
        public static void GetOrderClientAuthToken() {
            string customerId = $"customer_{JuspayServiceTest.Rnd.Next()}"; 
            JuspayEntity createCustomerInput = new CreateCustomerInput(new Dictionary<string, object>{ {"object_reference_id", $"{customerId}"}, {"mobile_number", "1234567890"}, {"email_address", "customer@juspay.com"}, {"mobile_country_code", "91"} , {"options", new Dictionary<string, object> {{"get_client_auth_token", true }} }});
            JuspayResponse newCustomer = new CustomerService().CreateCustomer((CreateCustomerInput)createCustomerInput, null);
            string clientAuthToken = newCustomer.Response.juspay.client_auth_token;
            string orderId = CreateOrderTest();
            JuspayResponse orderStatus = new OrderService().GetOrder(orderId, new Dictionary<string, object> {{"client_auth_token", clientAuthToken}}, new RequestOptions(null, "", null, null, null));
            Assert.NotNull(orderStatus.Response);

        }


        public static void GetEncryptedOrderClientAuthTokenTest() {
            string customerId = $"customer_{JuspayServiceTest.Rnd.Next()}"; 
            JuspayEntity createCustomerInput = new CreateCustomerInput(new Dictionary<string, object>{ {"object_reference_id", $"{customerId}"}, {"mobile_number", "1234567890"}, {"email_address", "customer@juspay.com"}, {"mobile_country_code", "91"} , {"options", new Dictionary<string, object> {{"get_client_auth_token", true }} }});
            JuspayResponse newCustomer = new CustomerService().CreateCustomer((CreateCustomerInput)createCustomerInput, null);
            string clientAuthToken = newCustomer.Response.juspay.client_auth_token;
            string orderId = CreateOrderTest();
            string privateKey1 = File.ReadAllText("../../../privateKey1.pem");
            string publicKey2 = File.ReadAllText("../../../publicKey2.pem");
            Dictionary<string, object> keys = new Dictionary<string, object> { { "privateKey", new Dictionary<string, object> { {"key", privateKey1 }, { "kid", "key_26b1a82e16cf4c6e850325c3d98368cb" } }}, { "publicKey", new Dictionary<string, object> { {"key", publicKey2 }, { "kid", "key_26b1a82e16cf4c6e850325c3d98368cb" } }}};
            JuspayResponse orderStatus = new OrderService().GetOrder(orderId, new Dictionary<string, object> {{"client_auth_token", clientAuthToken}}, new RequestOptions(null, "", null, null, new JuspayJWTRSA(keys)));
            Assert.NotNull(orderStatus);
            Assert.NotNull(orderStatus.Response);
            Assert.NotNull(orderStatus.ResponseBase);
            Assert.NotNull(orderStatus.RawContent);
        }
        public static void TestOrderService() {
            GetOrderTest();
            CreateOrderWithMetadataEntityTest();
            CreateOrderTest();
            CreateOrderTestAsync();
            InstantRefundTest();
            InstantRefundAsyncTest();
            UpdateOrderTest();
            UpdateOrderAsyncTest();
            RefundOrderTest();
            EncryptedRefundOrderTest();
            GetOrderClientAuthToken();
            GetEncryptedOrderClientAuthTokenTest();
            GetEncryptedOrderTestGlobal();
            EncryptedRefundOrderTestGlobal();
            CreateOrderWithCustomerIdTest();
        }
    }
}