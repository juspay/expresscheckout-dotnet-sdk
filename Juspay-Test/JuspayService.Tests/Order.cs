using Xunit;
using System;
using System.Collections.Generic;
using Juspay;

namespace JuspayTest {
    public class OrderTest {
        public static void OrderResponseEntityTest() {
            string order = "{\"status\":\"CREATED\",\"status_id\":1,\"id\":\"ordeh_e4413483448d464295bcfc9a945a58b8\",\"order_id\":\"order_1478031549\",\"payment_links\":{\"iframe\":\"https://sandbox.juspay.in/r/xLhHqP\",\"web\":\"https://sandbox.juspay.in/r/xicWsh\",\"mobile\":\"https://sandbox.juspay.in/r/jXP6CR\"}}";
            OrderResponse Order = JuspayResponse.FromJson<OrderResponse>(order);
            Assert.True(Order.OrderId == "order_1478031549");
            Assert.NotNull(Order.PaymentLinks);
            Assert.NotNull(Order.PaymentLinks.Iframe);
        }

        public static void CreateOrderWithMetadataEntityTest() {
           string orderId = $"order_{JuspayServiceTest.Rnd.Next()}";
            OrderCreate CreateOrderInput = new OrderCreate(new Dictionary<string, object> { {"order_id", $"{orderId}"},  {"amount", 10 }, {"metadata", new Dictionary<string, object>(){ { "value1", 100 } }} } ); 
            Assert.True((int)CreateOrderInput.Metadata["value1"] == 100);
        }

        public static string CreateOrderTest() 
        {
            string orderId = $"order_{JuspayServiceTest.Rnd.Next()}";
            OrderCreate createOrderInput = new OrderCreate(new Dictionary<string, object> { {"order_id", $"{orderId}"},  {"amount", 10 } } );
            OrderResponse order = new OrderService().CreateOrder(createOrderInput, new RequestOptions("azhar_test", null, null, null));
            Assert.NotNull(order);
            Assert.NotNull(order.Response);
            Assert.NotNull(order.ResponseBase);
            Assert.NotNull(order.RawContent);
            Assert.NotNull(order.OrderId);
            Assert.IsType<OrderResponse>(order);
            return orderId;
        }
        
        public static void GetOrderTest() 
        {
            string orderId = CreateOrderTest();
            OrderResponse orderStatus = new OrderService().GetOrder(orderId, new RequestOptions("azhar_test", null, null, null));
            Assert.NotNull(orderStatus);
            Assert.NotNull(orderStatus.Response);
            Assert.NotNull(orderStatus.ResponseBase);
            Assert.NotNull(orderStatus.RawContent);
            Assert.IsType<OrderResponse>(orderStatus);
        }

        public static void TestInstantRefundTest()
        {
            string orderId = CreateOrderTest();
            string uniqueRequestId = $"request_{JuspayServiceTest.Rnd.Next()}";
            TransactionIdAndInstantRefund RefundInput = new TransactionIdAndInstantRefund(new Dictionary<string, object> { { "order_id", orderId }, {"amount", 10 }, {"unique_request_id", uniqueRequestId }, { "order_type", "Juspay" }, {"refund_type", "STANDARD"} });
            try
            {
                RefundResponse refundResponse = new InstantRefundService().GetTransactionIdAndInstantRefund(RefundInput, null);
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
            OrderResponseEntityTest();
            CreateOrderTest();
            TestInstantRefundTest();
        }
    }
}