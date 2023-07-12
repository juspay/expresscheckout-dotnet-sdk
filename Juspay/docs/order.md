## Create Order
[POST /orders](https://developer.juspay.in/reference/create-order-1)
```cs
using System;
using System.Collections.Generic;
using Juspay;
string orderId = $"order_{JuspayServiceTest.Rnd.Next()}";
OrderCreate createOrderInput = new OrderCreate(new Dictionary<string, object> { {"order_id", $"{orderId}"},  {"amount", 10 } } );
JuspayResponse order = new OrderService().CreateOrder(createOrderInput, null);
```
## Get Order
[GET /orders/:order_id](https://developer.juspay.in/reference/get-order-status)
```cs
string orderId = "order_id";
JuspayResponse orderStatus = new OrderService().GetOrder(orderId, null);
```

## Update Order
[Post /orders/:order_id](https://developer.juspay.in/reference/update-order)
```cs
string orderId = "order_id";
JuspayResponse order = new OrderService().UpdateOrder(orderId, 99.99, null);
```
## Refund Order
[POST /orders/:order_id/refunds](https://developer.juspay.in/reference/refund-order)
```cs
string orderId = "order_id";
string uniqueRequestId = $"request_id";
RefundInput RefundInput = new RefundInput(new Dictionary<string, object> { { "order_id", orderId }, {"amount", 10 }, {"unique_request_id", uniqueRequestId } });
JuspayResponse refundResponse = new OrderService().RefundOrder(orderId, RefundInput, null);
```
## Transaction Refund
[POST /refunds](https://developer.juspay.in/reference/instant-refund)
```cs
string orderId = "order_id";
string uniqueRequestId = $"request_id";
TransactionIdAndInstantRefund RefundInput = new TransactionIdAndInstantRefund(new Dictionary<string, object> { { "order_id", orderId }, {"amount", 10 }, {"unique_request_id", uniqueRequestId }, { "order_type", "Juspay" }, {"refund_type", "STANDARD"} });
JuspayResponse refundResponse = new InstantRefundService().GetTransactionIdAndInstantRefund(RefundInput, null);
```

## Encrypted Order Status
[POST /v4/order-status](https://developer.juspay.in/reference/get-order-status)
```cs
string orderId = CreateOrderTest();
string privateKey = File.ReadAllText("privateKey.pem");
string publicKey = File.ReadAllText("publicKey.pem");
Dictionary<string, object> keys = new Dictionary<string, object> { { "privateKey", new Dictionary<string, object> { {"key", privateKey }, { "kid", "key id" } }}, { "publicKey", new Dictionary<string, object> { {"key", publicKey }, { "kid", "key id" } }}};
JuspayResponse orderStatus = new OrderService().EncryptedOrderStatus(orderId, new RequestOptions(null, null, null, null, new JuspayJWTRSA(keys)));
```