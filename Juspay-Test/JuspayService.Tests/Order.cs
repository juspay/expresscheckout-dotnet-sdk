using Xunit;
using System;
using System.Collections.Generic;
using Juspay;

namespace JuspayTest {
    public class OrderTest {
        public static void OrderResponseEntityTest() {
            string order = "{\"status\":\"CREATED\",\"status_id\":1,\"id\":\"ordeh_e4413483448d464295bcfc9a945a58b8\",\"order_id\":\"order_1478031549\",\"payment_links\":{\"iframe\":\"https://sandbox.juspay.in/r/xLhHqP\",\"web\":\"https://sandbox.juspay.in/r/xicWsh\",\"mobile\":\"https://sandbox.juspay.in/r/jXP6CR\"}}";
            OrderResponse Order = new OrderResponse { RawContent = order };
            Order.PopulateObject();
            Assert.True(Order.OrderId == "order_1478031549");
        }
        public static string CreateOrderTest() 
        {
            string orderId = $"order_{JuspayServiceTest.Rnd.Next()}";
            OrderCreate createOrderInput = new OrderCreate(new Dictionary<string, object> { {"order_id", $"{orderId}"},  {"amount", 10 } } );
            OrderResponse order = new OrderService().CreateOrder(createOrderInput, new RequestOptions("azhar_test", null, null, null));
            Assert.NotNull(order);
            Assert.NotNull(order.Response);
            Assert.NotNull(order.OrderId);
            Assert.IsType<OrderResponse>(order);
            return orderId;
        }
        
        public static void GetOrderTest() 
        {
            string orderId = CreateOrderTest();
            OrderResponse orderStatus = new OrderService().GetOrder(orderId, new RequestOptions("azhar_test", null, null, null));
            Assert.NotNull(orderStatus);
        }
        public static void TestOrderService() {
            GetOrderTest();
            OrderResponseEntityTest();
        }
    }
}