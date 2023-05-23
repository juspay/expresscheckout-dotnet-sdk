
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
        public static readonly string API_VERSION = "2019-05-07";
        // public static const String DEFAULT_SSL_PROTOCOL = "TLSv1.2";
        public static readonly string? juspayNetVersion  = getJupayNetVersion();
        public static readonly string SDK_VERSION = getSdkVersion();
        public static string? BaseUrl { get; set; } = JuspayEnvironment.SANDBOX_BASE_URL;
        public static string? ApiKey { get; set; }
        public static string? MerchantId { get; set; }
        private static TimeSpan? ConnectTimeoutInMilliSeconds {get; set; }
        private static TimeSpan? ReadTimeoutInMilliSeconds {get; set; }
        private static IJuspayClient? juspayClient;
        private static string? getJupayNetVersion() {
            var assembalyName = typeof(JuspayEnvironment).GetTypeInfo().Assembly.FullName;
            if (assembalyName != null) {
                return new AssemblyName(assembalyName)?.Version?.ToString(3);
            }
            return null;
        }
        private static string getSdkVersion() {
            return "";
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

            System.Net.Http.HttpClient httpClient = ConnectTimeoutInMilliSeconds.HasValue ? new HttpClient(new SocketsHttpHandler {ConnectTimeout = ConnectTimeoutInMilliSeconds.Value}) : new HttpClient();
            if (ReadTimeoutInMilliSeconds.HasValue) httpClient.Timeout = ReadTimeoutInMilliSeconds.Value;
            ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls12;
            return new JuspayClient(ApiKey, BaseUrl, httpClient);
        }

    }
}
