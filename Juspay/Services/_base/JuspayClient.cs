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
        private IHttpClient httpClient;
        private string apiKey;
        public string ApiBase { set; get; }
    
        public JuspayClient(IHttpClient client, string apiKey, string baseUrl)
        {
            this.apiKey = apiKey ?? JuspayEnvironment.ApiKey ?? throw new Exception("Api Key Not Initialized");
            ApiBase =  baseUrl ?? JuspayEnvironment.BaseUrl ?? throw new Exception("Base Url not Initialized");
            httpClient = client ?? throw new Exception("Http Client not initialized");
        }
        public async Task<T> RequestAsync<T>(HttpMethod apiMethod, string? path, object? input, object? queryParams, RequestOptions requestOptions, string contentType)  where T : IJuspayResponseEntity {
            JuspayRequest juspayRequest = new JuspayRequest(apiMethod, path, input, queryParams, requestOptions, contentType, this.apiKey, ApiBase);
            JuspayResponse responseObj = await httpClient.MakeRequestAsync(juspayRequest);
            if (responseObj.IsSuccessStatusCode)
            {
                T obj;
                try {
                    obj = responseObj.FromJson<T>();
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

        private static JuspayException BuildJuspayException(JuspayResponse response)
        {
            JObject jObject;

            try
            {
                jObject = JObject.Parse(response.RawContent);
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

            var juspayError =  JuspayEntity.FromJson<JuspayError>(response.RawContent);
            return new JuspayException(
                response.StatusCode,
                juspayError,
                response,
                juspayError.UserMessage ?? juspayError.ErrorMessage ?? "");
        }

        private static JuspayException BuildInvalidResponseException(JuspayResponse response)
        {
            return new JuspayException(
                response.StatusCode,
                null,
                response,
                $"Invalid response object from API: \"{response.RawContent}\" statusCode = {response.StatusCode}")
            {
                JuspayResponse = response,
            };
        }

        
     
    }
}