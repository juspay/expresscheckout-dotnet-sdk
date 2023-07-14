# Juspay.net
Official [Juspay](https://developer.juspay.in/) .NET SDK, supporting .NET Framework 4.5.2+ and .NET 5.0+

## Usage

### Import
All Juspay.net SDK's classes resides under namespace `Juspay`
```cs 
using Juspay;
```
### Authentication
Juspay authenticates API request using API key. API key are passed in Authorization headers.

Use `JuspayEnvironment.ApiKey` property to set the API key

### Environment Settings
```cs
JuspayEnvironment.ApiKey = "Api key";
JuspayEnvironment.MerchantId = "merchant id";
JuspayEnvironment.BaseUrl = "custom url"; // (predefined base url JuspayEnvironment.SANDBOX_BASE_URL, JuspayEnvironment.PRODUCTION_BASE_URL
)
JuspayEnvironment.ConnectTimeoutInMilliSeconds = 5000; // Supported only .net6.0 and higher
JuspayEnvironment.ReadTimeoutInMilliSeconds = 5000;
JuspayEnvironment.SSL = SecurityProtocolType.SystemDefault;
```
```cs
using Juspay;
JuspayEnvironment.ApiKey = "api_key";
JuspayEnvironment.BaseUrl = "https://sandbox.juspay.in";
```
### Services
Use Juspay Service classes to create, get or update Juspay resources. Each Service class accepts a Dictionary<string, object> and RequestOptions as Input and produces a JuspayResponse. All service has both Synchronous and Asynchronous version.

```cs
string customerId = "customer id";
CreateCustomerInput createCustomerInput = new CreateCustomerInput(new Dictionary<string, object>{ {"object_reference_id", $"{customerId}"}, {"mobile_number", "1234567890"}, {"email_address", "customer@juspay.com"}, {"mobile_country_code", "91"} });
JuspayResponse newCustomer = new CustomerService().CreateCustomer(createCustomerInput, new RequestOptions("merchant_id", null, null, null));
```
```cs
// Async version
string customerId = "customer id";
CreateCustomerInput createCustomerInput = new CreateCustomerInput(new Dictionary<string, object>{ {"object_reference_id", $"{customerId}"}, {"mobile_number", "1234567890"}, {"email_address", "customer@juspay.com"}, {"mobile_country_code", "91"} });
JuspayResponse newCustomer = new CustomerService().CreateCustomerAsync(createCustomerInput, new RequestOptions("merchant_id", null, null, null)).ConfigureAwait(false).GetAwaiter().GetResult();
```

#### Input Object
Input object as Dictionary<string, object> as input and provides getters and setters for fields accepted by the endpoint.

#### JuspayResponse Object
Response object contains Juspay endpoint response along with Headers, Status Code, getters and setters. Use ```.RawContent``` to get the raw response as string. Use ```.Response``` to get the response as ```dynamic```. To access the Headers and Status Code use ```.ResponseBase.Headers``` and ```.ResponseBase.StatusCode``` respectively. Response object also provides getter and setter for important fields. Getters are provided for retriving x-request-id (```.ResponseBase.XRequestId```), x-response-id (```.ResponseBase.XResponseId```) and x-jp-merchant-id (```.ResponseBase.XMerchantId```) from headers.
```cs
string orderId = $"order_id}";
OrderCreate createOrderInput = new OrderCreate(new Dictionary<string, object> { {"order_id", $"{orderId}"},  {"amount", 10 } } );
JuspayResponse order = new OrderService().CreateOrder(createOrderInput, new RequestOptions("azhar_test", null, null, null));
Console.WriteLine(order.Response);
Assert.WriteLine(order.ResponseBase);
Console.WriteLine(order.RawContent);
Console.WriteLine(((string)order.Response.order_id));
```

#### Request Options
RequestOptions provide option to set/override merchant id, API key (to override the global api key set by ```JuspayEnvironment.ApiKey```), Security protocol type and read timeout.
```cs
RequestOptions.MerchantId = "merchant id";
RequestOptions.ApiKey = "new api key";
RequestOptions.SSL = SecurityProtocolType.Tls13;
RequestOptions.ReadTimeoutInMilliSeconds = 7000;
// using constructor
RequestOptions reqOptions = new RequestOptions(string merchantId, string apiKey, SecurityProtocolType? ssl, long? readTimeoutInMilliSeconds);
```
### JWT
Pass JuspayJWTRSA in request option. JuspayJWTRSA implements IJuspayJWT interface. IJuspayJWT has three methods ConsumePayload, PreparePayload and Initialize (a factory method to initialize ISign and IEnc objects) along with three attributes Dictionary of keys, Sign of type ISign and Enc of type IEnc. JuspayJWTRSA currently uses JWTSign which is a implementation of ISign interface and JWEEnc which is a implementation of IEnc interface. Currently JuspayJWTRSA class comes with the SDK. Implement IJuspayJWT to create custom JWT classes. JuspayJWTRSA constructor accepts keys with kid as arguments.

```cs
string orderId = "order id";
string privateKey = "private key pem contents as string";
string publicKey = "public key pem contents as string";
Dictionary<string, object> keys = new Dictionary<string, object> { { "privateKey", new Dictionary<string, object> { {"key", privateKey }, { "kid", "key id" } }}, { "publicKey", new Dictionary<string, object> { {"key", publicKey }, { "kid", "key id" } }}};
JuspayResponse orderStatus = new OrderService().EncryptedOrderStatus(orderId, new RequestOptions(null, null, null, null, new JuspayJWTRSA(keys)));
```
### Errors
Juspay Services throw JuspayException. JuspayException has message, JuspayError, JuspayResponse and StatusCode as attributes.
```cs
using Juspay;
string orderId = "order_id";
string uniqueRequestId = $"request_id";
TransactionIdAndInstantRefund RefundInput = new TransactionIdAndInstantRefund(new Dictionary<string, object> { { "order_id", orderId }, {"amount", 10 }, {"unique_request_id", uniqueRequestId }, { "order_type", "Juspay" }, {"refund_type", "STANDARD"} });
try
{
    JuspayResponse refundResponse = new InstantRefundService().GetTransactionIdAndInstantRefund(RefundInput, null);
}
catch (JuspayException Ex)
{
    Console.WriteLine(Ex.JuspayError.ErrorMessage);
    Console.WriteLine(Ex.JuspayResponse.RawContent);
}
```
### Docs
- [Order](Juspay/docs/order.md)
- [Session](Juspay/docs/order_session.md)
- [Customer](Juspay/docs/customer.md)
### Test
All unit test are under Juspay-Test directory. To run the test set    ```API_KEY``` and ```MERCHANT_ID``` env variable, go to Juspay-Test directory and run ```dotnet test```, this will run test for all the .net versions supported by Juspay.net sdk. To run test for specific .net version use ```dotnet test -f net6.0```. 
