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
    using System.Text.Json;
    using System.Reflection;


    public class SystemHttpClient : IHttpClient
    {
        public static TimeSpan DefaultHttpTimeout => TimeSpan.FromSeconds(80);

        private const string JuspayNetTargetFramework =
#if NET5_0
            "net5.0"
#elif NETSTANDARD2_0
            "netstandard2.0"
#elif NET461
            "net461"
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
            ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol |
                SecurityProtocolType.Tls12;
        }

        public SystemHttpClient(
            HttpClient? httpClient = null)
        {
            this.httpClient = httpClient ?? LazyDefaultHttpClient.Value;

        }



        public SystemHttpClient(TimeSpan? ConnectTimeoutInMilliSeconds, TimeSpan? ReadTimeoutInMilliSeconds)
        {
            this.httpClient = ConnectTimeoutInMilliSeconds.HasValue ? new HttpClient(new SocketsHttpHandler {ConnectTimeout = ConnectTimeoutInMilliSeconds.Value}) : new HttpClient();
            if (ReadTimeoutInMilliSeconds.HasValue) httpClient.Timeout = ReadTimeoutInMilliSeconds.Value;
            ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls12;
        }


        public async Task<JuspayResponse> MakeRequestAsync(
            JuspayRequest juspayRequest)
        {
            HttpRequestMessage request = BuildRequestMessage(juspayRequest);
            request.RequestUri = BuildQueryparams(juspayRequest);
            AddRequestOptions(request, juspayRequest);
            AddAuthorizationHeaders(request, juspayRequest);
            var response = await httpClient.SendAsync(request).ConfigureAwait(false);
            return await BuildJuspayResponse(response);
            
        }

        private async Task<JuspayResponse> BuildJuspayResponse(HttpResponseMessage response) {
            var reader = new StreamReader(
                await response.Content.ReadAsStreamAsync().ConfigureAwait(false));
            JuspayResponse responseObj = new JuspayResponse(
                response.StatusCode,
                response.Headers,
                response.IsSuccessStatusCode,
                await reader.ReadToEndAsync().ConfigureAwait(false));
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
                var flattenedData = FlattenObject(input);
                if (juspayRequest.ContentType == "application/json") {
                    var jsonRequest = JsonConvert.SerializeObject(flattenedData);
                    var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                    request.Content = content;
                } else {
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
                if (path != null) path = "";
                var flattenedQueryParams = FlattenObject(queryParams);
                var queryString = string.Join("&", flattenedQueryParams.Select(x => $"{Uri.EscapeDataString(x.Key)}={Uri.EscapeDataString(x.Value.ToString() ?? "")}"));
                path += $"?{queryString}";
                return new Uri(apiBase + path);
            }
            return new Uri(apiBase + path ?? "");
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
        public static void FlattenObjectRecursive(object? obj, string? prefix, Dictionary<string, object> dictionary)
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
                    // Console.WriteLine("Nested dictionary");
                    FlattenDictionary(nestedDictionary, key, flattenedDictionary);
                }
                else
                {
                    var value = entry.Value?.ToString() ?? string.Empty;
                    // Console.WriteLine($"Not nested dictionary key => {key} value => {value}");
                    flattenedDictionary[key] = value;
                }
            }
        }
        private void AddRequestOptions(HttpRequestMessage request, JuspayRequest juspayRequest) {
            if (juspayRequest.RequestOptions != null) {
                RequestOptions requestOptions = juspayRequest.RequestOptions;
                foreach (var x in FlattenObject(requestOptions)) {
                    request.Headers.Add(x.Key, x.Value.ToString());
                }
                if (requestOptions.ReadTimeoutInMilliSeconds.HasValue) {
                    httpClient.Timeout = requestOptions.ReadTimeoutInMilliSeconds.Value;
                }
            } 
            
        }

        private void AddAuthorizationHeaders(HttpRequestMessage request, JuspayRequest juspayRequest) {
            RequestOptions requestOptions = juspayRequest.RequestOptions;
            if (requestOptions != null && requestOptions.ApiKey != null) {
                     request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{requestOptions.ApiKey}:")));
            } else {
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{juspayRequest.ApiKey}:")));
            }
            
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
