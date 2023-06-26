
namespace Juspay {
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Reflection;
    using Newtonsoft.Json;
    using System.Net;
    using System.Net.Http;
    public abstract class JuspayEnvironment {
        public static readonly string DEVELOPMENT_BASE_URL = "https://localapi.juspay.in";
        public static readonly string SANDBOX_BASE_URL = "https://sandbox.juspay.in";
        public static readonly string PRODUCTION_BASE_URL = "https://api.juspay.in";
        public static string API_VERSION { get; set; } = "2021-03-25";
        #if (NET6_0 || NET7_0)
            public static readonly SecurityProtocolType DEFAULT_SSL_PROTOCOL = SecurityProtocolType.SystemDefault;
        #else
            public static readonly SecurityProtocolType DEFAULT_SSL_PROTOCOL = SecurityProtocolType.Tls12;
        #endif
        public static SecurityProtocolType SSL { get; set; } = DEFAULT_SSL_PROTOCOL;
        public static readonly string juspaySDKVersion  = getJupaySDKVersion();
        public static readonly string SDK_VERSION = getSdkVersion();
        public static string BaseUrl { get; set; } = JuspayEnvironment.SANDBOX_BASE_URL;
        public static string ApiKey { get; set; }
        public static string MerchantId { get; set; }
        public static long ConnectTimeoutInMilliSeconds {
            get => connectTimeoutInMilliSeconds.Milliseconds;
            set => connectTimeoutInMilliSeconds = TimeSpan.FromTicks(value);
        }
        public static long ReadTimeoutInMilliSeconds { 
            get => readTimeoutInMilliSeconds.Milliseconds;
            set => readTimeoutInMilliSeconds = TimeSpan.FromTicks(value);
        }
        private static TimeSpan connectTimeoutInMilliSeconds;
        private static TimeSpan readTimeoutInMilliSeconds;
        private static IJuspayClient juspayClient;
        private static string getJupaySDKVersion() {
            var assemblyName = typeof(JuspayEnvironment).GetTypeInfo().Assembly.FullName;
            if (assemblyName != null) {
                return new AssemblyName(assemblyName)?.Version?.ToString(3);
            }
            return null;
        }
        private static string getSdkVersion() {
            return "2023-06-02";
        }

        public static IJuspayClient JuspayClient
        {
            get
            {
                if (juspayClient == null)
                {
                    juspayClient = BuildDefaultJuspayClient();
                }

                return juspayClient;
            }

            set => juspayClient = value;
        }

        public static JuspayClient BuildDefaultJuspayClient()
        {
            if (ApiKey != null && ApiKey.Length == 0)
            {
                var message = "API key is invalid";
                throw new JuspayException(message);
            }
            IHttpClient httpClient = new SystemHttpClient(connectTimeoutInMilliSeconds, readTimeoutInMilliSeconds);
            return new JuspayClient(httpClient, ApiKey, BaseUrl);
        }

    }
}
