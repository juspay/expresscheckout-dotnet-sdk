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
        
        public static void GetOrderTest() 
        {
            string orderId = CreateOrderTest();
            JuspayResponse orderStatus = new OrderService().GetOrder(orderId, new RequestOptions(JuspayEnvironment.MerchantId, null, null, null));
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

        public static void TestOrderService() {
            GetOrderTest();
            CreateOrderWithMetadataEntityTest();
            CreateOrderTest();
            CreateOrderTestAsync();
            InstantRefundTest();
            InstantRefundAsyncTest();
        }
    }
}