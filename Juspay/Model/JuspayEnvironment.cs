[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]
namespace Juspay {
    using System;
    using System.Reflection;
    using Newtonsoft.Json;
    using System.Net;
    using log4net;
    using log4net.Core;
    using log4net.Repository.Hierarchy;

    public abstract class JuspayEnvironment {
        
        #if NET6_0 || NET7_0 || NET5_0 || (NET47_OR_GREATER && !NET481)
            public static readonly SecurityProtocolType DEFAULT_SSL_PROTOCOL = SecurityProtocolType.SystemDefault;
        #else
            public static readonly SecurityProtocolType DEFAULT_SSL_PROTOCOL = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        #endif
        public static SecurityProtocolType SSL { get; set; } = DEFAULT_SSL_PROTOCOL;
        public static readonly string juspaySDKVersion  = getJupaySDKVersion();
        public static string BaseUrl { get; set; }
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

        public static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public enum JuspayLogLevel
        {
            All,
            Debug,
            Error,
            Info,
            Off,
        }

        public static void SerializedLog(object message, JuspayLogLevel level)
        {
            string jMessage = JsonConvert.SerializeObject(message);
            switch (level) 
            {
                case JuspayLogLevel.Debug:
                    log.Debug(jMessage);
                    break;
                case JuspayLogLevel.Error:
                    log.Error(jMessage);
                    break;
                default:
                    log.Info(jMessage);
                    break;
            }
        }
        public static void SetLogLevel(JuspayLogLevel level)
        {
            Level logLevel;

            switch (level)
            {
                case JuspayLogLevel.All:
                    logLevel = Level.All;
                    break;
                case JuspayLogLevel.Debug:
                    logLevel = Level.Debug;
                    break;
                case JuspayLogLevel.Error:
                    logLevel = Level.Error;
                    break;
                case JuspayLogLevel.Info:
                    logLevel = Level.Info;
                    break;
                case JuspayLogLevel.Off:
                    logLevel = Level.Off;
                    break;
                default:
                    logLevel = Level.All;
                    break;
            }
            ((Hierarchy)LogManager.GetRepository()).Root.Level = logLevel;
        }

        public static bool SetLogFile(string filePath)
        { 
            log4net.Repository.ILoggerRepository RootRep = log.Logger.Repository;
            foreach (log4net.Appender.IAppender iApp in RootRep.GetAppenders())
            {
                if (iApp.Name.ToString() == "FileAppender")
                {
                    log4net.Appender.FileAppender fApp = (log4net.Appender.FileAppender)iApp;
                    fApp.File = filePath;
                    fApp.ActivateOptions();
                    return true;
                }
            }
             return false;
        }
        
        public static IJuspayJWT JuspayJWT { get; set; }
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
                throw new JuspayException("INVALID_API_KEY");
            }
            IHttpClient httpClient = new SystemHttpClient(connectTimeoutInMilliSeconds, readTimeoutInMilliSeconds);
            return new JuspayClient(httpClient);
        }

    }
}
