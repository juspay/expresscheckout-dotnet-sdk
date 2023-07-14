## Create Customer
[POST /customers](https://developer.juspay.in/reference/customer)
```cs
string customerId = "customer_id"; 
JuspayEntity createCustomerInput = new CreateCustomerInput(new Dictionary<string, object>{ {"object_reference_id", $"{customerId}"}, {"mobile_number", "1234567890"}, {"email_address", "customer@juspay.com"}, {"mobile_country_code", "91"} , {"options", new Dictionary<string, object> {{"get_client_auth_token", true }} }});
JuspayResponse newCustomer = new CustomerService().CreateCustomer(createCustomerInput, null);
```

## Get Customer
[GET /customers/:customer_id](https://developer.juspay.in/reference/get-customer)
```cs
string customerId = "customer_id";
JuspayResponse customer = new CustomerService().GetCustomer(customerId, null, null);
```