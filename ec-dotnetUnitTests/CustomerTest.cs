using System;
using Juspay.ExpressCheckout.Base;
using Xunit;
using Juspay.ExpressCheckout;
using System.Collections.Generic;

namespace ec_dotnetUnitTests
{
    public class CustomerTest
    {
        static CustomerTest()
        {
            Config.Configure(Config.Environment.SANDBOX, "sriduth_sandbox_test", "4D94B6287BE459EB735EE9AB0DA212");
        }

        private static string RandomCustomerId()
        {
            return String.Format("Csharp-SDK-CustID-{0}", Guid.NewGuid().ToString());
        }


        [Fact]
        public void CreateCustomerTest()
        {
            string CustomerId = RandomCustomerId();
            string FirstName = "Juspay";
            string LastName = "Technologies";
            string MobileNumber = "9876545432";
            string EmailAddress = "support@juspay.in";

            try 
            {
                dynamic CustomerCreateResponse = Customer.CreateCustomer(CustomerId, MobileNumber, EmailAddress, FirstName, LastName);
                CustomerCreateResponse = CustomerCreateResponse.Result.Response;
                Assert.Equal(CustomerCreateResponse["object_reference_id"].ToString(), CustomerId);
    
            }
            catch(ArgumentException e)
            {
                Assert.True(false, "message");
            }
        }

        [Fact]
        public void GetCustomerTest()
        {
            string CustomerId = "Csharp-SDK-CustID-945b1fca-768e-430b-9115-eb6dbaf5b3f4";

            try
            {
                dynamic GetCustomerResponse = Customer.GetCustomer(CustomerId);
                GetCustomerResponse = GetCustomerResponse.Result.Response;

                Assert.Equal(GetCustomerResponse["object_reference_id"].ToString(), CustomerId);
            }
            catch (ArgumentException e)
            {
                Assert.True(false, "message");
            }
        }
        [Fact]
        public void UpdateCustomerTest()
        {
            string CustomerId = "Csharp-SDK-CustID-945b1fca-768e-430b-9115-eb6dbaf5b3f4";
            string NewNamePostFix = Guid.NewGuid().ToString().Substring(24);
            string FirstName = String.Format("JuspayF-{0}", NewNamePostFix);
            string LastName = String.Format("JuspayL-{0}", NewNamePostFix);
            string MobileNumber = DateTime.UtcNow.Ticks.ToString().Substring(8);
                                            
            try 
            {
                dynamic UpdateCustomerResponse = Customer.UpdateCustomer(CustomerId, new Dictionary<string, string>() {
                    { "first_name", FirstName },
                    { "mobile_number", MobileNumber },
                    { "last_name", LastName }
                });

                UpdateCustomerResponse = UpdateCustomerResponse.Result.Response;

                Assert.Equal(UpdateCustomerResponse["object_reference_id"].ToString(), CustomerId);
                Assert.Equal(UpdateCustomerResponse["first_name"].ToString(), FirstName);
                Assert.Equal(UpdateCustomerResponse["last_name"].ToString(), LastName);
                Assert.Equal(UpdateCustomerResponse["mobile_number"].ToString(), MobileNumber);
            }
            catch (ArgumentException e)
            {
                Assert.True(false, "message");
            }

        }

    }
}
