[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]
namespace Juspay {
    using System;
    using System.Reflection;
    using Newtonsoft.Json;
    using System.Net;
    using log4net;
    using log4net.Core;
    using log4net.Repository.Hierarchy;
    using System.Threading;

    public class JuspayEnvironment {

        private JuspayEnvironment() {
            juspayClient = new Lazy<IJuspayClient>(() => BuildDefaultJuspayClient());
            SSL = DEFAULT_SSL_PROTOCOL;
            juspaySDKVersion  = getJupaySDKVersion();
        }
        
        private static readonly Lazy<JuspayEnvironment> juspayEnvInstance = new Lazy<JuspayEnvironment>(() => new JuspayEnvironment());

        public static JuspayEnvironment Instance => juspayEnvInstance.Value;

        #if  NETCOREAPP3_0_OR_GREATER || NET47_OR_GREATER
            public readonly SecurityProtocolType DEFAULT_SSL_PROTOCOL = SecurityProtocolType.SystemDefault;
        #else
            public readonly SecurityProtocolType DEFAULT_SSL_PROTOCOL = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        #endif
        public SecurityProtocolType SSL { get; set; }
        public readonly string juspaySDKVersion;
        public string BaseUrl { get; set; }
        public string ApiKey { get; set; }
        public string MerchantId { get; set; }
        public long ConnectTimeoutInMilliSeconds {
            get => connectTimeoutInMilliSeconds.Milliseconds;
            set => connectTimeoutInMilliSeconds = TimeSpan.FromTicks(value);
        }
        public long ReadTimeoutInMilliSeconds { 
            get => readTimeoutInMilliSeconds.Milliseconds;
            set => readTimeoutInMilliSeconds = TimeSpan.FromTicks(value);
        }

        public readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public enum JuspayLogLevel
        {
            All,
            Debug,
            Error,
            Info,
            Off,
        }

        public void SerializedLog(object message, JuspayLogLevel level)
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
        public void SetLogLevel(JuspayLogLevel level)
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

        public bool SetLogFile(string filePath)
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
        
        public IJuspayJWT JuspayJWT { get; set; }
        private TimeSpan connectTimeoutInMilliSeconds;
        private TimeSpan readTimeoutInMilliSeconds;
        private Lazy<IJuspayClient> juspayClient;
        private string getJupaySDKVersion() {
            var assemblyName = typeof(JuspayEnvironment).GetTypeInfo().Assembly.FullName;
            if (assemblyName != null) {
                return new AssemblyName(assemblyName)?.Version?.ToString(3);
            }
            return null;
        }

        public IJuspayClient JuspayClient {
            get => juspayClient.Value;
            set => juspayClient = new Lazy<IJuspayClient>(() => value);
        }
        public JuspayClient BuildDefaultJuspayClient()
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
