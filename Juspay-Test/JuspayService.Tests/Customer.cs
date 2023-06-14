using Xunit;
using System;
using System.Collections.Generic;
using Juspay;

namespace JuspayTest {
    public class CustomerTest {

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
            JuspayResponse newCustomer = new CustomerService().CreateCustomer((CreateCustomerInput)createCustomerInput, null);
            Assert.NotNull(newCustomer);
            Assert.NotNull(newCustomer.Response.juspay.client_auth_token);
            Assert.NotNull(newCustomer.Response);
            Assert.NotNull(newCustomer.ResponseBase);
            Assert.True(newCustomer.ResponseBase.StatusCode == 200);
            Assert.NotNull(newCustomer.ResponseBase.XResponseId);
            Assert.True(newCustomer.ResponseBase.XMerchantId == JuspayEnvironment.MerchantId);
            Assert.NotNull(newCustomer.RawContent);
            Assert.True((string)newCustomer.Response.object_reference_id == customerId);
            Assert.IsType<JuspayResponse>(newCustomer);
            return customerId;
        }


        public static string CreateCustomerWithOutClientAuthToken() 
        {
            string customerId = $"customer_{JuspayServiceTest.Rnd.Next()}"; 
            CreateCustomerInput createCustomerInput = new CreateCustomerInput(new Dictionary<string, object>{ {"object_reference_id", $"{customerId}"}, {"mobile_number", "1234567890"}, {"email_address", "customer@juspay.com"}, {"mobile_country_code", "91"} });
            JuspayResponse newCustomer = new CustomerService().CreateCustomer(createCustomerInput, new RequestOptions("azhar_test", null, null, null));
            Assert.NotNull(newCustomer);
            Assert.Null(newCustomer.Response.juspay);
            Assert.IsType<JuspayResponse>(newCustomer);
            return customerId;
        }

        public static string CreateCustomerAsync() 
        {
            string customerId = $"customer_{JuspayServiceTest.Rnd.Next()}"; 
            CreateCustomerInput createCustomerInput = new CreateCustomerInput(new Dictionary<string, object>{ {"object_reference_id", $"{customerId}"}, {"mobile_number", "1234567890"}, {"email_address", "customer@juspay.com"}, {"mobile_country_code", "91"} });
            JuspayResponse newCustomer = new CustomerService().CreateCustomerAsync(createCustomerInput, new RequestOptions("azhar_test", null, null, null)).ConfigureAwait(false).GetAwaiter().GetResult();
            Assert.NotNull(newCustomer);
            Assert.IsType<JuspayResponse>(newCustomer);
            return customerId;
        }

        public static void GetCustomer() 
        {
            string customerId = CreateCustomerWithOutClientAuthToken();
            JuspayResponse customer = new CustomerService().GetCustomer(customerId, null, new RequestOptions("azhar_test", null, null, null));
            Assert.NotNull(customer);
            Assert.IsType<JuspayResponse>(customer);
            Assert.NotNull(customer.ResponseBase);
            Assert.NotNull(customer.Response);
            Assert.NotNull(customer.RawContent);
            Assert.True(customerId == (string)customer.Response.object_reference_id);
            customer = new CustomerService().GetCustomerAsync(customerId, null, new RequestOptions("azhar_test", null, null, null)).ConfigureAwait(false).GetAwaiter().GetResult();
            Assert.NotNull(customer);
        }

        public static void TestCustomerService() {
            CreateCustomerWithClientAuthToken();
            CreateCustomerAsync();
            TestGetterCustomerInput();
            GetCustomer();
            TestSetterCustomerInput();
        }
    }
}