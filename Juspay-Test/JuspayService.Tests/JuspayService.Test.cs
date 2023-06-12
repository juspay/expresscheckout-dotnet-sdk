using Xunit;
using System;
using System.Collections.Generic;
using Juspay;

namespace JuspayTest
{   
    [Collection("Sequential")]
    public class JuspayServiceTest
    {
        public static Random Rnd { get; set; }
        public JuspayServiceTest () {
            JuspayEnvironment.ApiKey = Environment.GetEnvironmentVariable("API_KEY");
            Rnd = new Random();
        }

        [Fact]
        public void TestCustomer () {
            CustomerTest.TestCustomerService();
        }

        [Fact]
        public void TestOrder() {
            OrderTest.TestOrderService();
        }

        [Fact]
        public void TestSession() {
            SessionTest.TestSessionService();
        }
        
        [Fact]
        public void TestInputEntity()
        {
            InputEntityTest.TestInputEntity();
        }

        [Fact]
        public void TestResponseEntity()
        {
            ResponseEntityTest.TestResponseEntity();
        }

    }
}