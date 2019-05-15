﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace Juspay.ExpressCheckout.Base
{
    public sealed class ECApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public HttpResponseHeaders Headers { get; set; }
        public String RawResponse { get; set; }
        public JObject Response { get; set; }
    }

    public sealed class Config
    {
        private static Environment Env;
        private static string MerchantId;
        private static string ApiKey;
        private static string ApiVersion;
             
        public static void Configure(Environment env, string merchantId, string apiKey, string apiVersion = "2017-07-26")
        {
            if(env == null || merchantId == null || apiKey == null)
            {
                throw new ArgumentException("ERROR: Please specify environment, merchantId and API Key");
            }

            ApiVersion = apiVersion;
            Env = env;
            MerchantId = merchantId;
            ApiKey = apiKey;
        }

        public static Environment GetEnv()
        {
            return Env;
        }

        public static string GetMerchantId()
        {
            return MerchantId;
        }

        public static string GetApiKey()
        {
            return ApiKey;
        }

        public static string GetApiVersion()
        {
            return ApiVersion;
        }

        public enum Environment {
            SANDBOX,
            PRODUCTION
        }

        // Generates the API url by considering the configured environment and the path of the API call
        public static Uri GenerateApiUrl(string path, IDictionary<string, string> query = null)
        {
            var uri = new UriBuilder();
            uri.Scheme = "https";
            uri.Path = path;

            switch (Env)
            {
                case Environment.SANDBOX:
                    uri.Host = "sandbox.juspay.in";
                    break;
                case Environment.PRODUCTION:
                    uri.Host = "api.juspay.in";
                    break;

                default:
                    throw new InvalidOperationException("Environment is not configured correctly");
            }

            // for
            if(query != null)
            {   
                var qbuilder = new StringBuilder();
                foreach(var item in query)
                {
                    qbuilder
                        .Append(item.Key)
                        .Append('=')
                        .Append(item.Value)
                        .Append('&');
                    
                }

                uri.Query = qbuilder.ToString();
            }

            return uri.Uri;
        }
    }

    sealed class HTTPUtils
    {
        private static readonly HttpClient Client = new HttpClient();

        static HTTPUtils()
        {
            Client.DefaultRequestHeaders.Add("Version", Config.GetApiVersion());
        }

       
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        private static string GenerateAuthHeader()
        {            
            return Base64Encode(String.Format("{0}:", Config.GetApiKey()));
        }

        private static HttpRequestMessage EmptyRequest()
        {
            return new HttpRequestMessage
            {
                Headers =
                {
                    { HttpRequestHeader.Authorization.ToString(), "Basic " + GenerateAuthHeader() },
                    { "Version", Config.GetApiVersion() }
                }
            };

        }

        public static async Task<HttpResponseMessage> DoPost(string path, IDictionary<string, string> payload)
        {
            try
            {
                var request = EmptyRequest();
                request.Method = HttpMethod.Post;
                request.RequestUri = Config.GenerateApiUrl(path);
                request.Headers.Add(HttpRequestHeader.ContentType.ToString(), "application/x-www-form-urlencoded");
                request.Content = new FormUrlEncodedContent(payload);

                return await Client.SendAsync(request);
            } 
            catch (ArgumentNullException e)
            {
                return null;
            }   
        }

        public static async Task<HttpResponseMessage> DoGet(string path, IDictionary<string, string> payload)
        {
            try
            {
                var request = EmptyRequest();
                request.Method = HttpMethod.Get;
                request.RequestUri = Config.GenerateApiUrl(path, payload);

                return await Client.SendAsync(request);
            }
            catch (ArgumentNullException e)
            {
                return null;
            }
        }

        public static async Task<HttpResponseMessage> DoDelete(string path, IDictionary<string, string> payload)
        {
            try
            {
                var request = EmptyRequest();
                request.Method = HttpMethod.Delete;
                request.RequestUri = Config.GenerateApiUrl(path, payload);

                return await Client.SendAsync(request);
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public static async Task<ECApiResponse> ParseAndWrapResponseJObject(HttpResponseMessage message)
        {
            var response = new ECApiResponse() {
                StatusCode = message.StatusCode,
                Headers = message.Headers,
                RawResponse = await message.Content.ReadAsStringAsync()
            };

            try
            {
                response.Response = JObject.Parse(response.RawResponse);
                return response;
            }
            catch(Exception e)
            {
                return response;
            }
        }
    }
}
