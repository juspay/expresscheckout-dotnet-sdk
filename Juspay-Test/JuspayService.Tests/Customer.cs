using Xunit;
using System;
using System.Collections.Generic;
using Juspay;

namespace JuspayTest {
    public class CustomerTest {
        public static void TestCustomerResponseWithAuthTokenEntity() {
            string customer = "{\"first_name\":null,\"mobile_country_code\":\"91\",\"last_updated\":\"2023-06-02T06:41:27Z\",\"object_reference_id\":\"customer_72328694\",\"mobile_number\":\"1234567890\",\"juspay\":{\"client_auth_token_expiry\":\"2023-06-02T06:56:27Z\",\"client_auth_token\":\"tkn_f14dd51eacac4175b36e22be7ea01e5c\"},\"last_name\":null,\"object\":\"customer\",\"email_address\":\"customer@juspay.com\",\"id\":\"cth_9eWcot9adszM1LNp\",\"date_created\":\"2023-06-02T06:41:27Z\"}";
            CustomerResponse Customer = JuspayResponse.FromJson<CustomerResponse>(customer);
            Assert.True(Customer.ObjectReferenceId == "customer_72328694");
            Assert.True(Customer.Juspay.ClientAuthToken == "tkn_f14dd51eacac4175b36e22be7ea01e5c");
        }
        public static void TestSetterCustomerResponse() {
            string customer = "{\"first_name\":null,\"mobile_country_code\":\"91\",\"last_updated\":\"2023-06-02T06:41:27Z\",\"object_reference_id\":\"customer_72328694\",\"mobile_number\":\"1234567890\",\"juspay\":{\"client_auth_token_expiry\":\"2023-06-02T06:56:27Z\",\"client_auth_token\":\"tkn_f14dd51eacac4175b36e22be7ea01e5c\"},\"last_name\":null,\"object\":\"customer\",\"email_address\":\"customer@juspay.com\",\"id\":\"cth_9eWcot9adszM1LNp\",\"date_created\":\"2023-06-02T06:41:27Z\"}";
            CustomerResponse Customer = JuspayResponse.FromJson<CustomerResponse>(customer);
            Customer.Juspay.ClientAuthToken = "123";
            Assert.True(Customer.Juspay.ClientAuthToken == "123");
            Assert.True((string)(Customer.Response["juspay"] as Dictionary<string, object>)["client_auth_token"] == Customer.Juspay.ClientAuthToken); 
        }
        public static void TestGetterCustomerInput() {
            string customerId = $"customer_{JuspayServiceTest.Rnd.Next()}"; 
            CreateCustomerInput CustomerInput = new CreateCustomerInput(new Dictionary<string, object>{ {"object_reference_id", $"{customerId}"}, {"mobile_number", "1234567890"}, {"email_address", "customer@juspay.com"}, {"mobile_country_code", "91"} , {"options", new Dictionary<string, object> {{"get_client_auth_token", true }} }});
            bool token = (bool)(CustomerInput.Data["options"] as Dictionary<string, object>)["get_client_auth_token"];
            Assert.NotNull(CustomerInput.ObjectReferenceId);
            Assert.NotNull(CustomerInput.Options);
            Assert.True(CustomerInput.Options.GetClientAuthToken);
        }

        public static void TestSetterCustomerInput() {
            string customerId = $"customer_{JuspayServiceTest.Rnd.Next()}"; 
            CreateCustomerInput CustomerInput = new CreateCustomerInput(new Dictionary<string, object>{ {"object_reference_id", $"{customerId}"}, {"mobile_number", "1234567890"}, {"email_address", "customer@juspay.com"}, {"mobile_country_code", "91"} , {"options", new Dictionary<string, object> {{"get_client_auth_token", true }} }});
            CustomerInput.Options.GetClientAuthToken = false;
            Assert.False(CustomerInput.Options.GetClientAuthToken);
            Assert.False((bool)(CustomerInput.Data["options"] as Dictionary<string, object>)["get_client_auth_token"]);
            CustomerInput.Options.GetClientAuthToken = true;
            Assert.True(CustomerInput.Options.GetClientAuthToken);
            Assert.True((bool)(CustomerInput.Data["options"] as Dictionary<string, object>)["get_client_auth_token"]);
            (CustomerInput.Data["options"] as Dictionary<string, object>)["get_client_auth_token"] = false;
            Assert.False(CustomerInput.Options.GetClientAuthToken);
            Assert.False((bool)(CustomerInput.Data["options"] as Dictionary<string, object>)["get_client_auth_token"]);
        }
        public static string CreateCustomerWithClientAuthToken() 
        {
            string customerId = $"customer_{JuspayServiceTest.Rnd.Next()}"; 
            JuspayEntity createCustomerInput = new CreateCustomerInput(new Dictionary<string, object>{ {"object_reference_id", $"{customerId}"}, {"mobile_number", "1234567890"}, {"email_address", "customer@juspay.com"}, {"mobile_country_code", "91"} , {"options", new Dictionary<string, object> {{"get_client_auth_token", true }} }});
            CustomerResponse newCustomer = new CustomerService().CreateCustomer((CreateCustomerInput)createCustomerInput, null);
            Assert.NotNull(newCustomer);
            Assert.NotNull(newCustomer.Juspay.ClientAuthToken);
            Assert.NotNull(newCustomer.Response);
            Assert.NotNull(newCustomer.ResponseBase);
            Assert.True(newCustomer.ResponseBase.StatusCode == 200);
            Assert.NotNull(newCustomer.ResponseBase.XResponseId);
            Assert.True(newCustomer.ResponseBase.XMerchantId == JuspayEnvironment.MerchantId);
            Assert.NotNull(newCustomer.RawContent);
            Assert.True(newCustomer.ObjectReferenceId == customerId);
            Assert.IsType<CustomerResponse>(newCustomer);
            return customerId;
        }


        public static string CreateCustomerWithOutClientAuthToken() 
        {
            string customerId = $"customer_{JuspayServiceTest.Rnd.Next()}"; 
            CreateCustomerInput createCustomerInput = new CreateCustomerInput(new Dictionary<string, object>{ {"object_reference_id", $"{customerId}"}, {"mobile_number", "1234567890"}, {"email_address", "customer@juspay.com"}, {"mobile_country_code", "91"} });
            CustomerResponse newCustomer = new CustomerService().CreateCustomer(createCustomerInput, new RequestOptions("azhar_test", null, null, null));
            Assert.NotNull(newCustomer);
            Assert.Null(newCustomer.Juspay);
            Assert.IsType<CustomerResponse>(newCustomer);
            return customerId;
        }

        public static string CreateCustomerAsync() 
        {
            string customerId = $"customer_{JuspayServiceTest.Rnd.Next()}"; 
            CreateCustomerInput createCustomerInput = new CreateCustomerInput(new Dictionary<string, object>{ {"object_reference_id", $"{customerId}"}, {"mobile_number", "1234567890"}, {"email_address", "customer@juspay.com"}, {"mobile_country_code", "91"} });
            CustomerResponse newCustomer = new CustomerService().CreateCustomerAsync(createCustomerInput, new RequestOptions("azhar_test", null, null, null)).ConfigureAwait(false).GetAwaiter().GetResult();
            Assert.NotNull(newCustomer);
            Assert.IsType<CustomerResponse>(newCustomer);
            return customerId;
        }

        public static void GetCustomer() 
        {
            string customerId = CreateCustomerWithOutClientAuthToken();
            JuspayResponse customer = new CustomerService().GetCustomer(customerId, null, new RequestOptions("azhar_test", null, null, null));
            Assert.NotNull(customer);
            Assert.IsType<CustomerResponse>(customer);
            Assert.NotNull(customer.ResponseBase);
            Assert.NotNull(customer.Response);
            Assert.NotNull(customer.RawContent);
            Assert.True(customerId == ((CustomerResponse)customer).ObjectReferenceId);
            customer = new CustomerService().GetCustomerAsync(customerId, null, new RequestOptions("azhar_test", null, null, null)).ConfigureAwait(false).GetAwaiter().GetResult();
            Assert.NotNull(customer);
        }

        public static void TestCustomerService() {
            TestCustomerResponseWithAuthTokenEntity();
            CreateCustomerWithClientAuthToken();
            CreateCustomerAsync();
            TestGetterCustomerInput();
            GetCustomer();
            TestSetterCustomerResponse();
            TestSetterCustomerInput();
        }
    }
}