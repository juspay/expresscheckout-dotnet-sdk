using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Juspay.ExpressCheckout;
using Juspay.ExpressCheckout.Base;

namespace ec_dotnetUnitTests
{
    public class MandateTests
    {
        ITestOutputHelper _console;
        public MandateTests(ITestOutputHelper helper)
        {
            _console = helper;
            Config.Configure(Config.Environment.SANDBOX, "sriduth_sandbox_test", "4D94B6287BE459EB735EE9AB0DA212");
        }

        async Task<ECApiResponse> CreateMandateOrder(string orderId, string customerId)
        {

            var udfs = new string[] { "1", "2", "3", "4", "5", "6", "7", "8" };
            var OrderDetails = new Dictionary<string, string>();

            OrderDetails.Add("order_id", orderId);
            OrderDetails.Add("amount", "10.00");

            for (int i = 0; i < udfs.Length; i++)
            {
                OrderDetails.Add("udf" + (i + 1), udfs[i]);
            }

            OrderDetails.Add("customer_id", customerId);
            
            return await Mandate.CreateMandateOrder(OrderDetails, Mandate.MandateMode.REQUIRED, "100");
        }

        [Fact]
        public void TestMandateOrderCreate()
        {
            var OrderId = "ord" + Common.RandomId();

            // Create customer, and for that customer, create a mandate order
            var CustomerId = "cst_" + Common.RandomId();
            string FirstName = "Juspay";
            string LastName = "Technologies";
            string MobileNumber = "9876545432";
            string EmailAddress = "support@juspay.in";

            dynamic CustomerResponse = Customer.CreateCustomer(CustomerId, MobileNumber, EmailAddress, FirstName, LastName);
            CustomerResponse = CustomerResponse.Result.Response;

            dynamic OrderResponse = CreateMandateOrder(OrderId, CustomerId);
            
            try
            {
                OrderResponse = OrderResponse.Result.Response;
                Assert.Equal(OrderResponse["order_id"].ToString(), OrderId);
            }
            catch (AggregateException e)
            {
                Assert.True(false, "message");
            }
        }

        [Fact]
        void TestMandateList()
        {
            var OrderId = "order_" + Common.RandomId();
            var CustomerId = "customer_mandate_test_customer";
            string FirstName = "Juspay";
            string LastName = "Technologies";
            string MobileNumber = "9876545432";
            string EmailAddress = "support@juspay.in";

            dynamic CustomerResponse = Customer.CreateCustomer(CustomerId, MobileNumber, EmailAddress, FirstName, LastName);
            CustomerResponse = CustomerResponse.Result.Response;

            dynamic ListResponse = Mandate.List(CustomerId);
            ListResponse = ListResponse.Result;

            // the current customer would not have executed any transactions, hence
            // the count must be 0
            Assert.True(Int32.Parse(ListResponse.Response["total"].ToString()) == 0);
        }
    }
}
