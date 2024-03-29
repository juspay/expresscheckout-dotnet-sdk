namespace Juspay
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using System.Text;
    using System.Reflection;

    public interface IHttpClient
    {

        Task<JuspayResponse> MakeRequestAsync(JuspayRequest request);

    }
    public class SystemHttpClient : IHttpClient
    {
        public static TimeSpan DefaultHttpTimeout => TimeSpan.FromSeconds(80);

        private const string JuspayNetTargetFramework =
#if NET7_0
            "net7.0"
#elif NET6_0
            "net6.0"
#elif NET5_0
            "net5.0"
#elif NETSTANDARD2_0
            "netstandard2.0"
#elif NET461
            "net461"
#elif NET452
            "net452"
#else
            "unknown"
#endif
        ;

        private static readonly Lazy<HttpClient> LazyDefaultHttpClient
            = new Lazy<HttpClient>(BuildDefaultSystemNetHttpClient);

        private readonly HttpClient httpClient;

        public static HttpClient BuildDefaultSystemNetHttpClient()
        {

            return new HttpClient
            {
                Timeout = DefaultHttpTimeout,
            };
        }
        static SystemHttpClient()
        {
            ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol;
        }

        public SystemHttpClient(
            HttpClient httpClient = null)
        {
            this.httpClient = httpClient ?? LazyDefaultHttpClient.Value;

        }



        public SystemHttpClient(TimeSpan ConnectTimeoutInMilliSeconds, TimeSpan ReadTimeoutInMilliSeconds)
        {
            #if (NET6_0 || NET7_0)
                this.httpClient = ConnectTimeoutInMilliSeconds != TimeSpan.Zero ? new HttpClient(new SocketsHttpHandler { ConnectTimeout = ConnectTimeoutInMilliSeconds}) : new HttpClient();
            #else
                this.httpClient = new HttpClient();
            #endif
            if (ReadTimeoutInMilliSeconds != TimeSpan.Zero) httpClient.Timeout = ReadTimeoutInMilliSeconds;
        }


        public async Task<JuspayResponse> MakeRequestAsync(
            JuspayRequest juspayRequest)
        {
            HttpRequestMessage request = BuildRequestMessage(juspayRequest);
            request.RequestUri = BuildQueryparams(juspayRequest);
            AddRequestOptions(request, juspayRequest);
            AddAuthorizationHeaders(request, juspayRequest);
            AddClientUserAgentString(request);
            var logRequest = new Dictionary <string, object> {{ "request", request.ToString() }};
            if (request.Content != null)
            {

                logRequest["body"] = await request.Content.ReadAsStringAsync();

            }
            JuspayEnvironment.Instance.SerializedLog(logRequest, JuspayEnvironment.JuspayLogLevel.Debug);
            ServicePointManager.SecurityProtocol = JuspayEnvironment.Instance.SSL;
            try
            {
                var response = await httpClient.SendAsync(request).ConfigureAwait(false);
                return await BuildJuspayResponse(juspayRequest, response);
            }   
            catch (Exception ex)
            {
                if (ex is HttpRequestException || ex is TaskCanceledException)
                {
                    throw new JuspayException(ex.InnerException.Message, ex);
                }
                throw;
            }
            
        }

        private async Task<JuspayResponse> BuildJuspayResponse(JuspayRequest request, HttpResponseMessage response) {
            var reader = new StreamReader(
                await response.Content.ReadAsStreamAsync().ConfigureAwait(false));
            var rawResponse = await reader.ReadToEndAsync().ConfigureAwait(false);
            JuspayEnvironment.Instance.SerializedLog(new Dictionary<string, string> { { "response", rawResponse }, { "status_code", ((int)response.StatusCode).ToString() }, { "headers", response.Headers.ToString()  } }, JuspayEnvironment.JuspayLogLevel.Debug);
            if (request.IsJwtSupported && request.RequestOptions != null && request.RequestOptions.JuspayJWT != null && response.IsSuccessStatusCode) {
                rawResponse = request.RequestOptions.JuspayJWT.ConsumePayload(rawResponse);
                JuspayEnvironment.Instance.SerializedLog(new Dictionary<string, string> { { "decrypted_response", rawResponse } }, JuspayEnvironment.JuspayLogLevel.Debug);
            }
            JuspayResponse responseObj = new JuspayResponse(
                (int)response.StatusCode,
                response.Headers,
                response.IsSuccessStatusCode,
                rawResponse);
            return responseObj;
        }


        private HttpRequestMessage BuildRequestMessage(JuspayRequest juspayRequest)
        {
            HttpMethod apiMethod = juspayRequest.Method;
            string path = juspayRequest.Path;
            string apiBase = juspayRequest.ApiBase;
            HttpRequestMessage request = new HttpRequestMessage(apiMethod, apiBase + path ?? "");
            object input = juspayRequest.Input;
            if (apiMethod == HttpMethod.Post && input != null) {
                if (juspayRequest.ContentType == ContentType.Json) {
                    var jsonRequest = JsonConvert.SerializeObject(input); 
                    if (juspayRequest.RequestOptions != null) {
                        RequestOptions requestOptions = juspayRequest.RequestOptions;
                        if (juspayRequest.IsJwtSupported && requestOptions.JuspayJWT != null)
                        {
                            jsonRequest = requestOptions.JuspayJWT.PreparePayload(jsonRequest);
                        }
                    }
                    var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                    request.Content = content;
                } else {
                    var flattenedData = FlattenObject(input);
                    var content = new FormUrlEncodedContent(flattenedData.Select(kv => new KeyValuePair<string, string>(kv.Key, kv.Value.ToString() ?? "")));
                    request.Content = content;
                }
            }
            return request;
        }

        private Uri BuildQueryparams(JuspayRequest juspayRequest) {
            object queryParams = juspayRequest.QueryParams;
            string path = juspayRequest.Path;
            string apiBase = juspayRequest.ApiBase;
            if (queryParams != null)
            {
                if (path == null) path = "";
                var flattenedQueryParams = FlattenObject(queryParams);
                var queryString = string.Join("&", flattenedQueryParams.Select(x => $"{Uri.EscapeDataString(x.Key)}={Uri.EscapeDataString(x.Value.ToString() ?? "")}"));
                path += $"?{queryString}";
                return new Uri(apiBase + path);
            }
            return new Uri(apiBase + path ?? "");
        }

        private void AddClientUserAgentString(HttpRequestMessage request)
        {
            string userAgent = $"Juspay .NetBindings/{JuspayEnvironment.Instance.juspaySDKVersion}";
            var values = new Dictionary<string, object>
            {
                { "sdk_version", JuspayEnvironment.Instance.juspaySDKVersion },
                { "lang", ".net" },
                { "juspay_net_target_framework", JuspayNetTargetFramework },
            };
            request.Headers.Add("X-User-Agent", JsonConvert.SerializeObject(values, Formatting.None));
            request.Headers.Add("User-Agent", userAgent);
        }

      

        public static Dictionary<string, object> FlattenObject(object obj)
        {
            var dictionary = new Dictionary<string, object>();
            if (obj is Dictionary<string, object>) {
                FlattenDictionary(obj as Dictionary<string, object>, "", dictionary);
            }
            else {
                FlattenObjectRecursive(obj, "", dictionary);
            }
            return dictionary;
        }
        public static void FlattenObjectRecursive(object obj, string prefix, Dictionary<string, object> dictionary)
        {
            if (obj == null)
                return;
            var properties = obj.GetType().GetProperties();
            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var jsonPropertyAttribute = property.GetCustomAttribute<JsonPropertyAttribute>();
                if (jsonPropertyAttribute != null)
                {
                    propertyName = jsonPropertyAttribute.PropertyName;
                } else {
                    continue;
                }
                var value = property.GetValue(obj);
                var propertyKey = string.IsNullOrEmpty(prefix) ? propertyName : $"{prefix}.{propertyName}";
                if (property.PropertyType == typeof(Dictionary<string, object>)) {
                    var dictionaryValue = property.GetValue(obj) as Dictionary<string,object>;
                    FlattenDictionary(dictionaryValue, propertyKey, dictionary);
                }else if (property.PropertyType.IsClass && property.PropertyType != typeof(string) && !property.PropertyType.IsGenericType)
                {
                    FlattenObjectRecursive(value, propertyKey, dictionary);
                }
                else if (propertyKey != null && value != null)
                {
                    dictionary.Add(propertyKey, value);
                }
            }
        }
        public static void FlattenDictionary(Dictionary<string, object> dictionary, string prefix,
            Dictionary<string, object> flattenedDictionary)
        {
            foreach (var entry in dictionary)
            {
                var key = string.IsNullOrEmpty(prefix) ? entry.Key : $"{prefix}.{entry.Key}";
                if (entry.Value is Dictionary<string, object> nestedDictionary)
                {
                    FlattenDictionary(nestedDictionary, key, flattenedDictionary);
                }
                else
                {
                    var value = entry.Value;
                    if (value != null)
                    {
                        var valueType = value?.GetType();
                        if (valueType != null && valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(List<>))
                        {
                            flattenedDictionary[key] = JsonConvert.SerializeObject(value);
                        }
                        else
                        {
                            flattenedDictionary[key] = value;
                        }
                    }
                    
                }
            }
        }
        private void AddRequestOptions(HttpRequestMessage request, JuspayRequest juspayRequest) {
            if (juspayRequest.RequestOptions != null) {
                RequestOptions requestOptions = juspayRequest.RequestOptions;
                foreach (var x in FlattenObject(requestOptions)) {
                    request.Headers.Add(x.Key, x.Value.ToString());
                }
                if (requestOptions.ReadTimeout != TimeSpan.Zero) {
                    httpClient.Timeout = requestOptions.ReadTimeout;
                }
                ServicePointManager.SecurityProtocol = requestOptions.SSL;
            }
        }

        private void AddAuthorizationHeaders(HttpRequestMessage request, JuspayRequest juspayRequest) {
            RequestOptions requestOptions = juspayRequest.RequestOptions;
            if (!(requestOptions != null && requestOptions.JuspayJWT != null) && juspayRequest.ApiKey != null && juspayRequest.ApiKey != "")
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{juspayRequest.ApiKey}:")));
            
        }
        private string ReplacePathParams (Dictionary<string, string> pathParams, string url) {
            if (pathParams != null && pathParams.Count > 0) {
                foreach (var param in pathParams) {
                    url = url.Replace($"{{{param.Key}}}", Uri.EscapeDataString(param.Value.ToString()));
                }
            }
            return url;
        }
    }
}
