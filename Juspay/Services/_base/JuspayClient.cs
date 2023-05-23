namespace Juspay {
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Reflection;
    using Newtonsoft.Json;
    using System.Text;
    using System.Text.Json;
    using Newtonsoft.Json.Linq;
    public class JuspayClient : IJuspayClient
    {
        private HttpClient httpClient;
        public string ApiBase { get; set; }
        public string ApiKey { get; set; }
        public JuspayClient(string? apiKey, string? baseUrl, HttpClient client)
        {
            httpClient = client ?? throw new Exception("Http Client not initialized");
            ApiKey = apiKey ?? JuspayEnvironment.ApiKey ?? throw new Exception("Api Key Not Initialized");
            ApiBase =  baseUrl ?? JuspayEnvironment.BaseUrl ?? throw new Exception("Base Url not Initialized");
        }
        public async Task<T> RequestAsync<T>(HttpMethod apiMethod, string? path, object? input, object? queryParams, RequestOptions requestOptions, string contentType)  where T : IJuspayEntity {
            var request = new HttpRequestMessage(apiMethod, ApiBase + path ?? "");
            if (apiMethod == HttpMethod.Post && input != null) {
                var flattenedData = FlattenObject(input);
                if (contentType == "application/json") {
                    var jsonRequest = JsonConvert.SerializeObject(flattenedData);
                    var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                    request.Content = content;
                } else {
                    var content = new FormUrlEncodedContent(flattenedData.Select(kv => new KeyValuePair<string, string>(kv.Key, kv.Value.ToString() ?? "")));
                    request.Content = content;
                }
            }
             if (queryParams != null)
            {
                if (path != null) path = "";
                var flattenedQueryParams = FlattenObject(queryParams);
                var queryString = string.Join("&", flattenedQueryParams.Select(x => $"{Uri.EscapeDataString(x.Key)}={Uri.EscapeDataString(x.Value.ToString() ?? "")}"));
                path += $"?{queryString}";
                request.RequestUri = new Uri(ApiBase + path);
            }
            AddRequestOptions(request, requestOptions);
            var response = await httpClient.SendAsync(request).ConfigureAwait(false);
            var reader = new StreamReader(
                await response.Content.ReadAsStreamAsync().ConfigureAwait(false));
            var responseObj = new JuspayResponse(
                response.StatusCode,
                response.Headers,
                await reader.ReadToEndAsync().ConfigureAwait(false));
            if (response.IsSuccessStatusCode)
                {
                    // var jsonResponse = await response.Content.ReadAsStringAsync();
                    // var responseObject = JsonConvert.DeserializeObject<T>(jsonResponse);
                    T obj;
                    try {
                        obj = JuspayEntity.FromJson<T>(responseObj);
                        return obj;
                    } catch (Newtonsoft.Json.JsonException) {
                        throw BuildInvalidResponseException(responseObj);
                    }
                }
            else
                {
                    throw BuildJuspayException(responseObj);
                }
        }
        // public async Task<T> SendPostRequest<T>(object requestData, string url, Dictionary<string, string> queryParams = null, RequestOptions requestOptions = null)
        // {
        //     string pathUrl = ReplacePathParams(pathParams, url);
        //     var flattenedData = FlattenObject(requestData);
        //     var jsonRequest = JsonConvert.SerializeObject(flattenedData);
        //     var content = new FormUrlEncodedContent(flattenedData.Select(kv => new KeyValuePair<string, string>(kv.Key, kv.Value.ToString())));
        //     var request = new HttpRequestMessage(HttpMethod.Post, ApiBase + pathUrl);
        //     AddRequestOptions(request, requestOptions);
        //     request.Content = content;
        //     var response = await httpClient.SendAsync(request).ConfigureAwait(false);
        //     var reader = new StreamReader(
        //         await response.Content.ReadAsStreamAsync().ConfigureAwait(false));
        //     var responseObj = new JuspayResponse(
        //         response.StatusCode,
        //         response.Headers,
        //         await reader.ReadToEndAsync().ConfigureAwait(false));
        //     return ProcessResponse<T>(responseObj);
            
        // }
        // public async Task<T> SendGetRequest<T>(string url, Dictionary<string, string> pathParams, object queryParams, RequestOptions requestOptions)
        // {
        //     string pathUrl = ReplacePathParams(pathParams, url);
        //     // Append query parameters to the URL if provided
        //     if (queryParams != null)
        //     {
        //         var flattenedQueryParams = FlattenObject(queryParams);
        //         var queryString = string.Join("&", flattenedQueryParams.Select(x => $"{Uri.EscapeDataString(x.Key)}={Uri.EscapeDataString(x.Value.ToString())}"));
        //         url += $"?{queryString}";
        //     }
        //     var request = new HttpRequestMessage(HttpMethod.Get, BaseUrl + pathUrl);
        //     AddRequestOptions(request, requestOptions);
        //     using var response = await httpClient.SendAsync(request);
        //     if (response.IsSuccessStatusCode)
        //     {
        //         var jsonResponse = await response.Content.ReadAsStringAsync();
        //         var responseObject = JsonConvert.DeserializeObject<T>(jsonResponse);
        //         return responseObject;
        //     }
        //     else
        //     {
        //         var statusCode = response.StatusCode;
        //         var responseContent = await response.Content.ReadAsStringAsync();
        //         var exceptionMessage = $"Request failed with status code: {statusCode}\nResponse: {responseContent}";
        //         throw new Exception(exceptionMessage);
        //     }
        // }

        private static JuspayException BuildJuspayException(JuspayResponse response)
        {
            JObject jObject;

            try
            {
                jObject = JObject.Parse(response.Content);
            }
            catch (Newtonsoft.Json.JsonException)
            {
                return BuildInvalidResponseException(response);
            }

            var errorToken = jObject["error_code"];
            if (errorToken == null)
            {
                return BuildInvalidResponseException(response);
            }

            var juspayError =  JuspayEntity.FromJson<JuspayError>(response);
            juspayError.JuspayResponse = response;

            return new JuspayException(
                response.StatusCode,
                juspayError,
                juspayError.UserMessage ?? juspayError.ErrorMessage ?? "");
        }

        private static JuspayException BuildInvalidResponseException(JuspayResponse response)
        {
            return new JuspayException(
                response.StatusCode,
                new JuspayError { JuspayResponse = response },
                $"Invalid response object from API: \"{response.Content}\" statusCode = {response.StatusCode}")
            {
                JuspayResponse = response,
            };
        }
        private static Dictionary<string, object> FlattenObject(object obj)
        {
            var dictionary = new Dictionary<string, object>();
            FlattenObjectRecursive(obj, "", dictionary);
            return dictionary;
        }
        private static void FlattenObjectRecursive(object? obj, string? prefix, Dictionary<string, object> dictionary)
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
                if (property.PropertyType.IsClass && property.PropertyType != typeof(string) && !property.PropertyType.IsGenericType)
                {
                    FlattenObjectRecursive(value, propertyKey, dictionary);
                }
                else if (propertyKey != null && value != null)
                {
                    dictionary.Add(propertyKey, value);
                }
            }
        }
        private void AddRequestOptions(HttpRequestMessage request, RequestOptions requestOptions) {
            if (requestOptions != null) {
                foreach (var x in FlattenObject(requestOptions)) {
                    request.Headers.Add(x.Key, x.Value.ToString());
                }
                if (requestOptions.ReadTimeoutInMilliSeconds.HasValue) {
                    httpClient.Timeout = requestOptions.ReadTimeoutInMilliSeconds.Value;
                }
            } 
            if (requestOptions != null && requestOptions.ApiKey != null) {
                     request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{requestOptions.ApiKey}:")));
            } else {
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{ApiKey}:")));
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