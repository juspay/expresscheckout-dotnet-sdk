using Xunit;
using System;
using System.Collections.Generic;
using Juspay;
using System.IO;

namespace JuspayTest {
    public class OrderTest {

        public static void CreateOrderWithMetadataEntityTest() {
           string orderId = $"order_{JuspayServiceTest.Rnd.Next()}";
            OrderCreate CreateOrderInput = new OrderCreate(new Dictionary<string, dynamic> { {"order_id", $"{orderId}"},  {"amount", 10 }, {"metadata", new Dictionary<string, dynamic>(){ { "value1", 100 } }} } ); 
            Assert.True(CreateOrderInput.Data["metadata"]["value1"] == 100);
        }

        public static string CreateOrderTest() 
        {
            string orderId = $"order_{JuspayServiceTest.Rnd.Next()}";
            OrderCreate createOrderInput = new OrderCreate(new Dictionary<string, dynamic> { {"order_id", $"{orderId}"},  {"amount", 10 } } );
            JuspayResponse order = new Order().Create(createOrderInput, null);
            Assert.NotNull(order);
            Assert.NotNull(order.Response);
            Assert.NotNull(order.ResponseBase);
            Assert.NotNull(order.RawContent);
            Assert.NotNull(order.Response["order_id"]);
            Assert.IsType<JuspayResponse>(order);
            return orderId;
        }

        public static string CreateOrderWithCustomerIdTest() 
        {
            string customerId = $"customer_{JuspayServiceTest.Rnd.Next()}"; 
            JuspayEntity createCustomerInput = new CreateCustomerInput(new Dictionary<string, dynamic>{ {"object_reference_id", $"{customerId}"}, {"mobile_number", "1234567890"}, {"email_address", "customer@juspay.com"}, {"mobile_country_code", "91"} , {"options", new Dictionary<string, dynamic> {{"get_client_auth_token", true }} }});
            JuspayResponse newCustomer = new Customer().Create((CreateCustomerInput)createCustomerInput, null);
            string orderId = $"order_{JuspayServiceTest.Rnd.Next()}";
            OrderCreate createOrderInput = new OrderCreate(new Dictionary<string, dynamic> { {"order_id", $"{orderId}"},  {"amount", 10 } } );
            RequestOptions requestOptions = new RequestOptions{ CustomerId = customerId};
            JuspayResponse order = new Order().Create(createOrderInput, requestOptions);
            Assert.NotNull(order);
            Assert.NotNull(order.Response);
            Assert.NotNull(order.ResponseBase);
            Assert.NotNull(order.RawContent);
            Assert.NotNull(order.Response["order_id"]);
            Assert.IsType<JuspayResponse>(order);
            return orderId;

        }

        public static string CreateOrderTestAsync()
        {
            string orderId = $"order_{JuspayServiceTest.Rnd.Next()}";
            OrderCreate createOrderInput = new OrderCreate(new Dictionary<string, dynamic> { {"order_id", $"{orderId}"},  {"amount", 10 } } );
            JuspayResponse order = new Order().CreateAsync(createOrderInput, new RequestOptions(JuspayEnvironment.MerchantId, null, null, null)).ConfigureAwait(false).GetAwaiter().GetResult();
            Assert.NotNull(order);
            Assert.NotNull(order.Response);
            Assert.NotNull(order.ResponseBase);
            Assert.NotNull(order.RawContent);
            Assert.NotNull(order.Response["order_id"]);
            Assert.IsType<JuspayResponse>(order);
            return orderId;
        }
        
        public static void UpdateOrderTest()
        {
            string orderId = CreateOrderTest();
            JuspayResponse order = new Order().Update(orderId, 99.99, null);
            Assert.True((string)order.Response["order_id"] == orderId);
            Assert.True((double)order.Response["amount"] == 99.99);
        }

        public static void UpdateOrderAsyncTest()
        {
            string orderId = CreateOrderTest();
            JuspayResponse order = new Order().UpdateAsync(orderId, 99.99, null).ConfigureAwait(false).GetAwaiter().GetResult();;
            Assert.True((string)order.Response["order_id"] == orderId);
            Assert.True((double)order.Response["amount"] == 99.99);
        }

        public static void GetOrderTest() 
        {
            string orderId = CreateOrderTest();
            JuspayResponse orderStatus = new Order().Get(orderId, null, new RequestOptions(JuspayEnvironment.MerchantId, null, null, null));
            Assert.NotNull(orderStatus);
            Assert.NotNull(orderStatus.Response);
            Assert.NotNull(orderStatus.ResponseBase);
            Assert.NotNull(orderStatus.RawContent);
            Assert.IsType<JuspayResponse>(orderStatus);
            // Async Test
            orderStatus = new Order().GetAsync(orderId, null, null).ConfigureAwait(false).GetAwaiter().GetResult();
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
            TransactionIdAndInstantRefund RefundInput = new TransactionIdAndInstantRefund(new Dictionary<string, dynamic> { { "order_id", orderId }, {"amount", 10 }, {"unique_request_id", uniqueRequestId }, { "order_type", "Juspay" }, {"refund_type", "STANDARD"} });
            try
            {
                JuspayResponse refundResponse = new Refund().Create(RefundInput, null);
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
            TransactionIdAndInstantRefund RefundInput = new TransactionIdAndInstantRefund(new Dictionary<string, dynamic> { { "order_id", orderId }, {"amount", 10 }, {"unique_request_id", uniqueRequestId }, { "order_type", "Juspay" }, {"refund_type", "STANDARD"} });
            try
            {
                JuspayResponse refundResponse = new Refund().CreateAsync(RefundInput, null).ConfigureAwait(false).GetAwaiter().GetResult();
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
            JuspayResponse orderStatus = new Order().Get(orderId, null, new RequestOptions(null, null, null, null, new JuspayJWTRSA("key_26b1a82e16cf4c6e850325c3d98368cb", publicKey2, privateKey1)));
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
            JuspayEnvironment.JuspayJWT =  new JuspayJWTRSA("key_26b1a82e16cf4c6e850325c3d98368cb", publicKey2, privateKey1);
            try
            {
                JuspayResponse orderStatus = new Order().Get(orderId, null, null);
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
            RefundOrder RefundInput = new RefundOrder(new Dictionary<string, dynamic> { { "order_id", orderId }, {"amount", 10 }, {"unique_request_id", uniqueRequestId } });
            try
            {
                JuspayResponse refundResponse = new Order().Refund(orderId, RefundInput, null);
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
            RefundOrder RefundInput = new RefundOrder(new Dictionary<string, dynamic> { { "order_id", orderId }, {"amount", 10 }, {"unique_request_id", uniqueRequestId } });
            try
            {
                JuspayResponse refundResponse = new Order().Refund(orderId, RefundInput, new RequestOptions(null, null, null, null, new JuspayJWTRSA("key_26b1a82e16cf4c6e850325c3d98368cb", publicKey2, privateKey1)));
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
            RefundOrder RefundInput = new RefundOrder(new Dictionary<string, dynamic> { { "order_id", orderId }, {"amount", 10 }, {"unique_request_id", uniqueRequestId } });
            JuspayEnvironment.JuspayJWT =  new JuspayJWTRSA( "key_26b1a82e16cf4c6e850325c3d98368cb", publicKey2, privateKey1);
            try
            {
                JuspayResponse refundResponse = new Order().Refund(orderId, RefundInput, null);
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
            JuspayEntity createCustomerInput = new CreateCustomerInput(new Dictionary<string, dynamic>{ {"object_reference_id", $"{customerId}"}, {"mobile_number", "1234567890"}, {"email_address", "customer@juspay.com"}, {"mobile_country_code", "91"} , {"options", new Dictionary<string, dynamic> {{"get_client_auth_token", true }} }});
            JuspayResponse newCustomer = new Customer().Create((CreateCustomerInput)createCustomerInput, null);
            string clientAuthToken = (string)newCustomer.Response["juspay"]["client_auth_token"];
            string orderId = CreateOrderTest();
            JuspayResponse orderStatus = new Order().Get(orderId, new Dictionary<string, dynamic> {{"client_auth_token", clientAuthToken}}, new RequestOptions(null, "", null, null, null));
            Assert.NotNull(orderStatus.Response);

        }


        public static void GetEncryptedOrderClientAuthTokenTest() {
            string customerId = $"customer_{JuspayServiceTest.Rnd.Next()}"; 
            JuspayEntity createCustomerInput = new CreateCustomerInput(new Dictionary<string, dynamic>{ {"object_reference_id", $"{customerId}"}, {"mobile_number", "1234567890"}, {"email_address", "customer@juspay.com"}, {"mobile_country_code", "91"} , {"options", new Dictionary<string, dynamic> {{"get_client_auth_token", true }} }});
            JuspayResponse newCustomer = new Customer().Create((CreateCustomerInput)createCustomerInput, null);
            string clientAuthToken = (string)newCustomer.Response["juspay"]["client_auth_token"];
            string orderId = CreateOrderTest();
            string privateKey1 = File.ReadAllText("../../../privateKey1.pem");
            string publicKey2 = File.ReadAllText("../../../publicKey2.pem");
            JuspayResponse orderStatus = new Order().Get(orderId, new Dictionary<string, dynamic> {{"client_auth_token", clientAuthToken}}, new RequestOptions(null, "", null, null, new JuspayJWTRSA("key_26b1a82e16cf4c6e850325c3d98368cb", publicKey2, privateKey1)));
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