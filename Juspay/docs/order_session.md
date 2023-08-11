## Create Order Session
[POST /session](#)
```cs
string customerId = "customer_id";
string orderId = "order_id";
CreateOrderSessionInput sessionInput = JuspayEntity.FromJson<CreateOrderSessionInput>($"{{\n\"amount\":\"10.00\",\n\"order_id\":\"{orderId}\",\n\"customer_id\":\"{customerId}\",\n\"payment_page_client_id\":\"{JuspayEnvironment.MerchantId}\",\n\"action\":\"paymentPage\",\n\"return_url\": \"https://google.com\"\n}}");
JuspayResponse sessionRes = new SessionService().CreateOrderSession(sessionInput, null);
Console.WrtieLine(sessionRes.Response);
```

## Encrypted Create Session
[Post /v4/session](#)
```cs
string customerId = "customer_id";
string orderId = "order_id";
CreateOrderSessionInput sessionInput = JuspayEntity.FromJson<CreateOrderSessionInput>($"{{\n\"amount\":\"10.00\",\n\"order_id\":\"{orderId}\",\n\"customer_id\":\"{customerId}\",\n\"payment_page_client_id\":\"{JuspayEnvironment.MerchantId}\",\n\"action\":\"paymentPage\",\n\"return_url\": \"https://google.com\"\n}}");
string privateKey = File.ReadAllText("privateKey.pem");
string publicKey = File.ReadAllText("publicKey.pem");
Dictionary<string, object> keys = new Dictionary<string, object> { { "privateKey", new Dictionary<string, object> { {"key", privateKey }, { "kid", "key id" } }}, { "publicKey", new Dictionary<string, object> { {"key", publicKey }, { "kid", "key id" } }}};
JuspayResponse sessionRes = new SessionService().CreateOrderSession(sessionInput, new RequestOptions(null, null, null, null, new JuspayJWTRSA(keys)));
```
### Sample Response
```json
{
  "status": "NEW",
  "id": "ordeh_xxxxxxxxxx",
  "order_id": "order_xxxxx",
  "payment_links": {
    "web": "https://sandbox.juspay.in/orders/ordeh_xxxxx/payment-page",
    "expiry": null
  },
  "sdk_payload": {
    "requestId": "xxxxxxxx",
    "service": "in.juspay.hyperpay",
    "payload": {
      "clientId": "xxxx",
      "amount": "10.0",
      "merchantId": "xxxxx",
      "clientAuthToken": "tkn_xxxxxxxxx",
      "clientAuthTokenExpiry": "2023-08-11T06:56:17Z",
      "environment": "sandbox",
      "action": "paymentPage",
      "customerId": "customer_xxx",
      "returnUrl": "https://google.com",
      "currency": "INR",
      "orderId": "order_xxxxxxx"
    }
  }
}
```