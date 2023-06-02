
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
        public static readonly string API_VERSION = "2021-03-25";
        // public static const String DEFAULT_SSL_PROTOCOL = "TLSv1.2";
        public static readonly string juspaySDKVersion  = getJupaySDKVersion();
        public static readonly string SDK_VERSION = getSdkVersion();
        public static string BaseUrl { get; set; } = JuspayEnvironment.SANDBOX_BASE_URL;
        public static string ApiKey { get; set; }
        public static string MerchantId { get; set; }
        private static TimeSpan ConnectTimeoutInMilliSeconds {get; set; }
        private static TimeSpan ReadTimeoutInMilliSeconds {get; set; }
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

        private static JuspayClient BuildDefaultJuspayClient()
        {
            if (ApiKey != null && ApiKey.Length == 0)
            {
                var message = "API key is invalid";
                throw new JuspayException(message);
            }
            IHttpClient httpClient = new SystemHttpClient(ConnectTimeoutInMilliSeconds, ReadTimeoutInMilliSeconds);
            return new JuspayClient(httpClient, ApiKey, BaseUrl);
        }

    }
}
