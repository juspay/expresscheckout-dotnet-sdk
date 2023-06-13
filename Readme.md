# Juspay.net
Official [Juspay](https://developer.juspay.in/) .NET SDK, supporting .NET Framework 4.5.2+, .NET Core 2.0+ and .NET 5.0+

## Usage

### Import
All Juspay.net SDK's classes resides under namespace `Juspay`
```C#
using Juspay;
```
### Authentication
Juspay authenticates API request using API key. API key are passed in Authorization headers.

Use `JuspayEnvironment.ApiKey` property to set the API key

```C#
using Juspay;
JuspayEnvironment.ApiKey = "api_key";
```
### Configure Endpoint
By default Juspay.net SDK uses [Sandbox](https://sandbox.juspay.in) as endpoint. There are predefined endpoints(```JuspayEnvironment.SANDBOX_BASE_URL```, ```JuspayEnvironment.PRODUCTION_BASE_URL```) which can be set to ```JuspayEnvironment.BaseUrl``` attribute. To change the endpoint to custom url use ```JuspayEnvironment.BaseUrl = "custom domain"``` 
### Services
Use Juspay Service classes to create, get or update Juspay resources. Each Service class accepts a Dictionary<string, object> and RequestOptions as Input and produces a JuspayResponse. All service has both Synchronous and Asynchronous version.

```C#
string customerId = "customer id";
CreateCustomerInput createCustomerInput = new CreateCustomerInput(new Dictionary<string, object>{ {"object_reference_id", $"{customerId}"}, {"mobile_number", "1234567890"}, {"email_address", "customer@juspay.com"}, {"mobile_country_code", "91"} });
CustomerResponse newCustomer = new CustomerService().CreateCustomer(createCustomerInput, new RequestOptions("merchant_id", null, null, null));
```
```C#
// Async version
string customerId = "customer id";
CreateCustomerInput createCustomerInput = new CreateCustomerInput(new Dictionary<string, object>{ {"object_reference_id", $"{customerId}"}, {"mobile_number", "1234567890"}, {"email_address", "customer@juspay.com"}, {"mobile_country_code", "91"} });
CustomerResponse newCustomer = new CustomerService().CreateCustomerAsync(createCustomerInput, new RequestOptions("merchant_id", null, null, null)).ConfigureAwait(false).GetAwaiter().GetResult();
```

#### Input Object
Input object as Dictionary<string, object> as input and provides getters and setters for fields accepted by the endpoint.

#### Response Object
Response object contains Juspay endpoint response along with Headers, getters and setters. Use ```.RawContent``` to get the raw response as string. Use ```.Response``` to get the response as Dictionary<string, object>. To access the Headers and Status Code use ```.ResponseBase.Headers``` and ```.ResponseBase.StatusCode``` respectively. Response object also provides getter and setter for important fields. Getters are provided for retriving x-request-id (```.ResponseBase.XRequestId```), x-response-id (```.ResponseBase.XResponseId```) and x-jp-merchant-id (```.ResponseBase.XMerchantId```) from headers.

#### Request Options
RequestOptions provide option to set merchant id, API key (to override the global api key set by ```JuspayEnvironment.ApiKey```), Security protocol type and read timeout.

### Errors
Juspay Services throw JuspayException. JuspayException has message, JuspayError, JuspayResponse and StatusCode as attributes.
```C#
using Juspay;
string orderId = "order_id";
string uniqueRequestId = $"request_id";
TransactionIdAndInstantRefund RefundInput = new TransactionIdAndInstantRefund(new Dictionary<string, object> { { "order_id", orderId }, {"amount", 10 }, {"unique_request_id", uniqueRequestId }, { "order_type", "Juspay" }, {"refund_type", "STANDARD"} });
try
{
    RefundResponse refundResponse = new InstantRefundService().GetTransactionIdAndInstantRefund(RefundInput, null);
}
catch (JuspayException Ex)
{
    Console.WriteLine(Ex.JuspayError.ErrorMessage);
    Console.WriteLine(Ex.JuspayResponse.RawContent);
}
``` 
### Test
All unit test are under Juspay-Test directory. To run the test set    ```API_KEY``` and ```MERCHANT_ID``` env variable, go to Juspay-Test directory and run ```dotnet test```, this will run test for all the .net versions supported by Juspay.net sdk. To run test for specific .net version use ```dotnet test -f net6.0```. 







