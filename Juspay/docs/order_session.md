## Create Order Session
[POST /session](#)
```cs
string customerId = "customer_id";
string orderId = "order_id";
CreateOrderSessionInput sessionInput = JuspayEntity.FromJson<CreateOrderSessionInput>($"{{\n\"amount\":\"10.00\",\n\"order_id\":\"{orderId}\",\n\"customer_id\":\"{customerId}\",\n\"payment_page_client_id\":\"{JuspayEnvironment.MerchantId}\",\n\"action\":\"paymentPage\",\n\"return_url\": \"https://google.com\"\n}}");
JuspayResponse sessionRes = new SessionService().CreateOrderSession(sessionInput, null);
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
JuspayResponse sessionRes = new SessionService().EncryptedCreateOrderSession(sessionInput, new RequestOptions(null, null, null, null, new JuspayJWTRSA(keys)));
```