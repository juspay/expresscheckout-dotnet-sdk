# Juspay.net
Official [Juspay](https://developer.juspay.in/) .NET SDK, supporting .NET Framework 4.5.2+ and .NET 5.0+

## Usage

#### Installation
Using dotnet
```sh
dotnet add package expresscheckout --version {version_number}
```
using Nuget Package Manager
```sh
Install-Package expresscheckout -Version {version_number}
```
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
JuspayEnvironment.BaseUrl = "custom url"; // (predefined base url JuspayEnvironment.SANDBOX_BASE_URL, JuspayEnvironment.PRODUCTION_BASE_URL)
JuspayEnvironment.ConnectTimeoutInMilliSeconds = 5000; // Supported only .net6.0 and higher
JuspayEnvironment.ReadTimeoutInMilliSeconds = 5000;
JuspayEnvironment.SSL = SecurityProtocolType.SystemDefault;
JuspayEnvironment.JuspayJWT =  new JuspayJWTRSA("keyId", publicKey, privateKey);
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
JuspayResponse newCustomer = new Customer().Create(createCustomerInput, new RequestOptions("merchant_id", null, null, null));
```
```cs
// Async version
string customerId = "customer id";
CreateCustomerInput createCustomerInput = new CreateCustomerInput(new Dictionary<string, object>{ {"object_reference_id", $"{customerId}"}, {"mobile_number", "1234567890"}, {"email_address", "customer@juspay.com"}, {"mobile_country_code", "91"} });
JuspayResponse newCustomer = new Customer().CreateAsync(createCustomerInput, new RequestOptions("merchant_id", null, null, null)).ConfigureAwait(false).GetAwaiter().GetResult();
```

#### Input Object
Input object as Dictionary<string, object> as input and provides getters and setters for fields accepted by the endpoint.

#### JuspayResponse Object
Response object contains Juspay endpoint response along with Headers, Status Code, getters and setters. Use ```.RawContent``` to get the raw response as string. Use ```.Response``` to get the response as ```dynamic```. To access the Headers and Status Code use ```.ResponseBase.Headers``` and ```.ResponseBase.StatusCode``` respectively. Response object also provides getter and setter for important fields. Getters are provided for retriving x-request-id (```.ResponseBase.XRequestId```), x-response-id (```.ResponseBase.XResponseId```) and x-jp-merchant-id (```.ResponseBase.XMerchantId```) from headers.
```cs
string orderId = "order_id";
OrderCreate createOrderInput = new OrderCreate(new Dictionary<string, object> { {"order_id", $"{orderId}"},  {"amount", 10 } } );
JuspayResponse order = new Order().Create(createOrderInput, new RequestOptions("azhar_test", null, null, null));
Console.WriteLine(order.Response);
Assert.WriteLine(order.ResponseBase);
Console.WriteLine(order.RawContent);
Console.WriteLine(((string)order.Response.order_id));
```

#### Request Options
RequestOptions provide option to set/override merchant id, API key (to override the global api key set by ```JuspayEnvironment.ApiKey```), CustomerId, Security protocol type and read timeout.
```cs
RequestOptions.MerchantId = "merchant id";
RequestOptions.ApiKey = "new api key";
RequestOptions.SSL = SecurityProtocolType.Tls13;
RequestOptions.ReadTimeoutInMilliSeconds = 7000;
RequsetOptions.CustomerId = "x-customer-id";
// using constructor
RequestOptions reqOptions = new RequestOptions(string merchantId, string apiKey, SecurityProtocolType? ssl, long? readTimeoutInMilliSeconds);
```
### JWT
Pass JuspayJWTRSA in request option or set JuspayEnvironment.JuspayJWT. JuspayJWTRSA implements IJuspayJWT interface. IJuspayJWT has three methods ConsumePayload, PreparePayload and Initialize (a factory method to initialize ISign and IEnc objects) along with three attributes Dictionary of keys, Sign of type ISign and Enc of type IEnc. JuspayJWTRSA currently uses JWTSign which is a implementation of ISign interface and JWEEnc which is a implementation of IEnc interface. Currently JuspayJWTRSA class comes with the SDK. Implement IJuspayJWT to create custom JWT classes. JuspayJWTRSA constructor accepts keys with kid as arguments.

#### Using RequestOptions
```cs
string orderId = "order id";
string privateKey = "private key pem contents as string";
string publicKey = "public key pem contents as string";
JuspayResponse orderStatus = new Order().Get(orderId, new RequestOptions(null, null, null, null, new JuspayJWTRSA("keyId", publicKey, privateKey)));
```
#### Using JuspayEnvironment
```cs
string orderId = "order id";
string privateKey = "private key pem contents as string";
string publicKey = "public key pem contents as string";
JuspayEnvironment.JuspayJWT =  new JuspayJWTRSA("keyId", publicKey, privateKey);
JuspayResponse orderStatus = new Order().Get(orderId);
JuspayEnvironment.SetLogLevel(JuspayEnvironment.JuspayLogLevel.Debug);
JuspayEnvironment.SetLogFile("log file name"); // by default logs/juspay_sdk
```
### Errors
Juspay Services throw JuspayException. JuspayException has message, JuspayError, JuspayResponse and StatusCode as attributes.
```cs
string orderId = $"order_{JuspayServiceTest.Rnd.Next()}";
OrderCreate createOrderInput = new OrderCreate(new Dictionary<string, object> { {"order_id", $"{orderId}"},  {"amount", 10 } } );
try 
{
    JuspayResponse order = new Order().Create(createOrderInput, null);
}
catch (AuthorizationException Ex)
{
    Console.WriteLine(Ex.Message) // to get error message
    if (Ex.JuspayError != null) Console.WriteLine(Ex.JuspayError.ErrorMessage); // to get the error message
    if (Ex.JuspayResponse != null) Console.WriteLine(Ex.JuspayResponse.RawContent); // to get the raw response from the api
    Console.WriteLine(Ex.JuspayError.Status); // to get the juspay error status of the response
    Console.WriteLine(Ex.JuspayError.ErrorCode) // to get the juspay error code from the response
    Console.WriteLine(Ex.HttpStatusCode) // to get the status code of the response
//Handler for authorization exception 
}
catch (AuthenticationException Ex)
{
// Handler for authentication exception
}
catch (InvalidRequestException Ex)
{
// Handler for invalid request exception
}
catch (JWTException Ex)
{
// Thrown when there is issue with private or public key
// Handler for validation exception
}
catch (JuspayException Ex)
{
    // All the above Exception inherits JuspayException. Use this as default handler.
}
```

# Docs

## Create Order
[POST /orders](https://developer.juspay.in/reference/create-order-1)
```cs
string orderId = "order_id";
OrderCreate createOrderInput = new OrderCreate(new Dictionary<string, object> { {"order_id", $"{orderId}"},  {"amount", 10 } } );
JuspayResponse order = new Order().Create(createOrderInput, null);
```
## Get Order
[GET /orders/:order_id](https://developer.juspay.in/reference/get-order-status)
```cs
string orderId = "order_id";
JuspayResponse orderStatus = new Order().Get(orderId, null, null);
```

## Update Order
[Post /orders/:order_id](https://developer.juspay.in/reference/update-order)
```cs
string orderId = "order_id";
JuspayResponse order = new Order().Update(orderId, 99.99, null);
```
## Refund Order
[POST /orders/:order_id/refunds](https://developer.juspay.in/reference/refund-order)
```cs
string orderId = "order_id";
string uniqueRequestId = "request_id";
RefundOrder refundInput = new RefundOrder(new Dictionary<string, object> { { "order_id", orderId }, {"amount", 10 }, {"unique_request_id", uniqueRequestId } });
JuspayResponse refundResponse = new Order().Refund(orderId, RefundInput, null);
```
## Transaction Refund
[POST /refunds](https://developer.juspay.in/reference/instant-refund)
```cs
string orderId = "order_id";
string uniqueRequestId = "request_id";
TransactionIdAndInstantRefund RefundInput = new TransactionIdAndInstantRefund(new Dictionary<string, object> { { "order_id", orderId }, {"amount", 10 }, {"unique_request_id", uniqueRequestId }, { "order_type", "Juspay" }, {"refund_type", "STANDARD"} });
JuspayResponse refundResponse = new Refund().Create(RefundInput, null);
```

## Encrypted Order Status
[POST /v4/order-status](https://developer.juspay.in/reference/get-order-status)
```cs
string orderId = CreateOrderTest();
string privateKey = File.ReadAllText("privateKey.pem");
string publicKey = File.ReadAllText("publicKey.pem");
JuspayResponse orderStatus = new Order().Get(orderId, new RequestOptions(null, null, null, null, new JuspayJWTRSA("keyId", publicKey, privateKey)));
```

## Encrypted Order Refund
[POST /v4/order/:order_id/refunds](#)
```cs
string orderId = "order_id";
string uniqueRequestId = "request_id";
string privateKey = File.ReadAllText("privateKey.pem");
string publicKey = File.ReadAllText("publicKey.pem");
Dictionary<string, object> keys = new Dictionary<string, object> { { "privateKey", new Dictionary<string, object> { {"key", privateKey }, { "kid", "testJwe" } }}, { "publicKey", new Dictionary<string, object> { {"key", publicKey }, { "kid", "testJwe" } }}};
RefundOrder RefundInput = new RefundOrder(new Dictionary<string, object> { { "order_id", orderId }, {"amount", 10 }, {"unique_request_id", uniqueRequestId } });
```
## Create Order Session
[PO#ST /session](#)
```cs
string customerId = "customer_id";
string orderId = "order_id";
CreateOrderSessionInput sessionInput = JuspayEntity.FromJson<CreateOrderSessionInput>($"{{\n\"amount\":\"10.00\",\n\"order_id\":\"{orderId}\",\n\"customer_id\":\"{customerId}\",\n\"payment_page_client_id\":\"{JuspayEnvironment.MerchantId}\",\n\"action\":\"paymentPage\",\n\"return_url\": \"https://google.com\"\n}}");
JuspayResponse sessionRes = new OrderSession().Create(sessionInput, null);
Console.WrtieLine(sessionRes.Response);
```

### Encrypted Create Session
[Post /v4/session](#)
```cs
string customerId = "customer_id";
string orderId = "order_id";
CreateOrderSessionInput sessionInput = JuspayEntity.FromJson<CreateOrderSessionInput>($"{{\n\"amount\":\"10.00\",\n\"order_id\":\"{orderId}\",\n\"customer_id\":\"{customerId}\",\n\"payment_page_client_id\":\"{JuspayEnvironment.MerchantId}\",\n\"action\":\"paymentPage\",\n\"return_url\": \"https://google.com\"\n}}");
string privateKey = File.ReadAllText("privateKey.pem");
string publicKey = File.ReadAllText("publicKey.pem");
JuspayResponse sessionRes = new OrderSession().Create(sessionInput, new RequestOptions(null, null, null, null, new JuspayJWTRS("keyId", publicKey, privateKey)));
```
## Create Customer
[POST /customers](https://developer.juspay.in/reference/customer)
```cs
string customerId = "customer_id"; 
JuspayEntity createCustomerInput = new CreateCustomerInput(new Dictionary<string, object>{ {"object_reference_id", $"{customerId}"}, {"mobile_number", "1234567890"}, {"email_address", "customer@juspay.com"}, {"mobile_country_code", "91"} , {"options", new Dictionary<string, object> {{"get_client_auth_token", true }} }});
JuspayResponse newCustomer = new Customer().Create(createCustomerInput, null);
```

## Get Customer
[GET /customers/:customer_id](https://developer.juspay.in/reference/get-customer)
```cs
string customerId = "customer_id";
JuspayResponse customer = new Customer().Get(customerId, null, null);
```

### Sample Integration
```cs
using Juspay; 

namespace custom {
    public class Program()
    {
        static void Main()
        {
            try
            {
                //ENV Initialization

                JuspayEnvironment.ApiKey = "Api key";
                JuspayEnvironment.MerchantId = "merchant id";
                JuspayEnvironment.BaseUrl = "custom url";

                //Create Order
               
                string orderId = "order_id";
                string customerId = "customer_id";
                OrderCreate createOrderInput = new OrderCreate(new Dictionary<string, object> { {"order_id", $"{orderId}"},  {"amount", 1 } } );
                JuspayResponse order = new Order().Create(createOrderInput, null);
                string createdOrderId = order.Response.order_id; // same as  input order id
                Console.WriteLine(order.Response.payment_links.web); // load this link in browser to do a transaction

                // Update Order amount
                JuspayResponse updatedOrder = new Order().Update(orderId, 10, null); // use this to update the order amount

                // Create Session
                CreateOrderSessionInput createOrderSessionInput = new CreateOrderSessionInput(new Dictionary<string, object>{{ "amount", "10.00" }, { "order_id", orderId }, { "customer_id", customerId }, { "payment_page_client_id", JuspayEnvironment.MerchantId }, { "action", "paymentPage" }, { "return_url", "https://google.com" }});
                JuspayResponse sessionRes = new OrderSession().Create(createOrderSessionInput, null);
                Console.WriteLine(sessionRes.Response.payment_links.web); // load this link in browser to do a transaction

                // Get order status
                JuspayResponse orderStatus = new Order().Get(orderId, null, null);
                Console.WriteLine(orderStatus.Response.status); // verify status of the order ("NEW", "CHARGED"..)

                // Refund Order
                string uniqueRequestId = "unique_request_id";
                RefundOrder refundInput = new RefundOrder(new Dictionary<string, object> { { "order_id", orderId }, {"amount", 10 }, {"unique_request_id", uniqueRequestId } });
                JuspayResponse refundResponse = new Order().Refund(orderId, refundInput, null);
                Console.WriteLine(refundResponse.Response.amount_refunded); // check the refunded amount value
            }
            catch (JuspayException Ex)
            {
                // All the above Exception inherits JuspayException. Use this as default handler.
                Console.WriteLine(Ex.Message) // to get error message
                if (Ex.JuspayError != null) Console.WriteLine(Ex.JuspayError.ErrorMessage); // to get juspay API error message
                if (Ex.JuspayResponse != null) Console.WriteLine(Ex.JuspayResponse.RawContent); // to get the raw response from the api
                Console.WriteLine(Ex.JuspayError.Status); // to get the juspay error status of the response
                Console.WriteLine(Ex.JuspayError.ErrorCode); // to get the juspay error code from the response
                Console.WriteLine(Ex.HttpStatusCode); // to get the status code of the response
            }
           
        }
    }
}
```

### Test
All unit test are under Juspay-Test directory. To run the test set    ```API_KEY``` and ```MERCHANT_ID``` env variable, go to Juspay-Test directory and run ```dotnet test```, this will run test for all the .net versions supported by Juspay.net sdk. To run test for specific .net version use ```dotnet test -f net6.0```. 
