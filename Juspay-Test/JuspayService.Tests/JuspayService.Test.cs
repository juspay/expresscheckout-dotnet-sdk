using Xunit;
using System;
using System.Collections.Generic;
using Juspay;
using System.IO;

namespace JuspayTest
{   
    [Collection("Sequential")]
    public class JuspayServiceTest
    {
        public static Random Rnd { get; set; }
        public JuspayServiceTest () {
            JuspayEnvironment.Instance.ApiKey = Environment.GetEnvironmentVariable("API_KEY");
            JuspayEnvironment.Instance.MerchantId = Environment.GetEnvironmentVariable("MERCHANT_ID");
            JuspayEnvironment.Instance.BaseUrl = "https://sandbox.juspay.in";
            JuspayEnvironment.Instance.SetLogLevel(JuspayEnvironment.JuspayLogLevel.Debug);
            JuspayEnvironment.Instance.SetLogFile("../../../logs/juspay_sdk");
            Rnd = new Random();
        }

        // [Fact]
        // public void TestCustomer () {
        //     CustomerTest.TestCustomerService();
        // }

        [Fact]
        public void TestOrder() {
            OrderTest.TestOrderService();
        }

        [Fact]
        public void TestSession() {
            SessionTest.TestOrderSession();
        }

        [Fact]
        public void TestJWT() {
            JWTTest.readPrivateKeyPkcs1Test();
            JWTTest.readPrivateKeyPkcs8Test();
            JWTTest.readPublicKeyRSATest();
            JWTTest.invalidPrivateKeyTest();
            JWTTest.invalidRSAPublicKeyTest();
            JWTTest.testSigningJWSRSA();
            JWTTest.testVerifySignatureRSAFailure();
            JWTTest.testKeyEncryptionRSA();
            JWTTest.testContentEncryptionAESGCM();
            JWTTest.testJWTRSAAESGCM();
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