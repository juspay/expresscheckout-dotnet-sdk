namespace Juspay {
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Reflection;
    using Newtonsoft.Json;
    using System.Text;
    using Newtonsoft.Json.Linq;
    using System.Threading.Tasks;

    public interface IJuspayClient
    {

        Task<JuspayResponse> RequestAsync(HttpMethod method, string path, object input, object queryParams, RequestOptions requestOptions, ContentType contentType, bool isJWTSupported);
    }

    public class JuspayClient : IJuspayClient
    {
        private IHttpClient httpClient;
    
        public JuspayClient(IHttpClient client)
        {
            httpClient = client ?? throw new JuspayException("HTTP_CLIENT_NOT_INITIALIZED");
        }
        public async Task<JuspayResponse> RequestAsync(HttpMethod apiMethod, string path, object input, object queryParams, RequestOptions requestOptions, ContentType contentType, bool isJWTSupported) {
            string apiBase = JuspayEnvironment.BaseUrl ?? throw new JuspayException("BASE_URL_NOT_INITIALIZED");
            JuspayRequest juspayRequest = new JuspayRequest(apiMethod, path, input, queryParams, requestOptions, contentType, JuspayEnvironment.ApiKey, apiBase, isJWTSupported);
            JuspayResponse responseObj = await httpClient.MakeRequestAsync(juspayRequest);
            if (responseObj.ResponseBase.IsSuccessStatusCode)
            {
                return responseObj;
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

            switch (response.ResponseBase.StatusCode)
            {
                case 400:
                case 404:
                    return new InvalidRequestException(response.ResponseBase.StatusCode, juspayError, response);
                case 401:
                    return new AuthenticationException(response.ResponseBase.StatusCode, juspayError, response);
                case 403:
                    return new AuthorizationException(response.ResponseBase.StatusCode, juspayError, response);
                default:
                    return new JuspayException(
                                response.ResponseBase.StatusCode,
                                juspayError,
                                response,
                                juspayError.UserMessage ?? juspayError.ErrorMessage ?? "");
            }
        }

        private static JuspayException BuildInvalidResponseException(JuspayResponse response)
        {
            return new JuspayException(
                response.ResponseBase.StatusCode,
                null,
                response,
                $"Invalid response object from API: \"{response.RawContent}\" statusCode = {response.ResponseBase.StatusCode}")
            {
                JuspayResponse = response,
            };
        }

        
     
    }
}