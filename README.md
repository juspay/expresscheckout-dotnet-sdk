# Express Checkout C# SDK

## Configure the SDK
Configure the SDK by invoking the `Juspay.ExpressCheckout.Base.Config.Configure` method:

```cs
using Juspay.ExpressCheckout.Base;

namespace MyApplication
{
    class MyClass
    {
        static MyClass()
        {
            Config.Configure(Config.Environment.SANDBOX, 
                "my-merchant-id", "my-api-key");
        }
    }
}
```

## API Usage

### General notes / Before starting

Ensure that the ExpressCheckout client is configured correctly by invoking the configuration logic. None of the
APIs described below will work if the configuration is not done / configuration is incorrect.

### Note on `ECApiResponse` and `async`
All APIs of the ExpressCheckout SDK return instances of `ECApiResponse`. An instance of `ECApiResponse` contains the following data:

- HTTP status code `StatusCode` of type `HttpStatusCode`
- Response headers `Headers` of type `HttpResponseHeaders`
- Response body `RawResponse` of type `string`
- Parsed response body `Response` of type `JObject`

Additionally note that all the APIs exposed are `async`

### Order APIs

##### Create Order
#
```cs
using Juspay.ExpressCheckout;
using Newtonsoft.Json.Linq;

namespace MyApplication 
{
    public class MyClass
    {
        public static async void CreateOrder()
        {
            // Prepare a dictionary of parameters.
            // the minimum 2 parameters required to create the order are the orderId and the order amount
            var OrderDetails = new Dictionary<string, string>();
            
            // Generate an order_id
            var OrderId = RandomOrderId();
            
            // Set the details
            OrderDetails.Add("order_id", OrderId);
            OrderDetails.Add("amount", "10.00");
            

            // Wait for the task to finish and
            ECApiResponse OrderResponse = await Orders.CreateOrder(OrderDetails);
        }
    }
}
```


##### Order Status API
#
```cs
using Juspay.ExpressCheckout;

namespace MyApplication
{
    class MyClass
    {
        public static async void FetchOrderStatus()
        {
            var OrderId = "Csharp-SDK-b2b78e5c-c2bf-481d-aae5-2003ec9738df";
            
            // Wait for the Task to finish and get the response
            ECApiResponse OrderStatusResponse = await Orders.GetStatus(OrderId);
        }
    }
}
```
##### Order List
#
```cs
namespace MyApplication
{
    class MyClass
    {
        public static async void GetOrderList()
        {
            int count = 20;
            
            // Wait for the Task to finish and get the response
            EcApiResponse OrderListResponse = await Orders.List(count);
        }
    }
}
```
