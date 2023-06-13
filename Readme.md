# Juspay.net
official [Juspay](https://developer.juspay.in/) .NET library, supporting .NET Framework 4.5.2+, .NET Core 2.0+, .NET 5.0+

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

### Services
Use Juspay Service classes to create, get or update Juspay resources. Each Service class accepts a Dictionary<string, object> and RequestOptions as Input and produces a JuspayResponse.

```C#
    string customerId = "customer id";
    CreateCustomerInput createCustomerInput = new CreateCustomerInput(new Dictionary<string, object>{ {"object_reference_id", $"{customerId}"}, {"mobile_number", "1234567890"}, {"email_address", "customer@juspay.com"}, {"mobile_country_code", "91"} });
    CustomerResponse newCustomer = new CustomerService().CreateCustomer(createCustomerInput, new RequestOptions("merchant_id", null, null, null));
```

#### Input Object
Input object as Dictionary<string, object> as input and provides getters and setters for fields accepted by the endpoint.

#### Response Object
Response object contains Juspay endpoint response along with Headers, getters and setters. Use ```.RawContent``` to get the raw response as string. Use ```.Response``` to get the response as Dictionary<string, object>. To access the Headers and Status Code use ```.ResponseBase.Headers``` and ```.ResponseBase.StatusCode``` respectively. Response object also provides getter and setter for important fields. Getter are provided for retriving x-request-id, x-response-id, x-jp-merchant-id from headers, Use ```.ResponseBase.XRequestId```, ```.ResponseBase.XResponseId``` and ```.ResponseBase.XMerchantId```.

#### Request Options
RequestOptions provide option to set merchant id, API key (to override the global api key set by ```JuspayEnvironment.ApiKey```), Security protocol type and read timeout.

### Test
All unit test are under Juspay-Test directory. To run the test set    ```API_KEY``` and ```MERCHANT_ID``` env variable, go to Juspay-Test directory and run ```dotnet test```. This will run test for all the .net versions supported by Juspay.net sdk. To run test for specific .net version use ```dotnet test -f net6.0```.






