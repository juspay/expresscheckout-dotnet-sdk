namespace Juspay
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;

    public enum ContentType
    {
        FormUrlEncoded,
        Json
    }

    /// <summary>
    /// Represents a request to Juspay's API.
    /// </summary>
    public class JuspayRequest
    {
        public object Input;

        public object QueryParams { get; set; }

        public string Path { get; set; }

        public ContentType ContentType { get; set; }

        public HttpMethod Method { get; set; }

        public RequestOptions RequestOptions { get; }

        public string ApiKey { get; }
        
        public string ApiBase { get; set; }

        public bool IsJwtSupported { get; set; }

        public JuspayRequest(HttpMethod method, string path, object input, object queryParams, RequestOptions requestOptions, ContentType contentType, string apiKey, string baseUrl, bool isJWTSupported)
        {
            this.RequestOptions = requestOptions;

            this.Method = method;

            this.Path = path;

            this.QueryParams = queryParams;

            this.ContentType = contentType;
            
            this.ApiKey = apiKey;

            this.ApiBase = baseUrl;

            this.Input = input;

            this.IsJwtSupported = isJWTSupported;

            if (requestOptions == null) this.RequestOptions = new RequestOptions();
            else this.RequestOptions = requestOptions;
            
            if (RequestOptions.ApiKey != null) this.ApiKey = requestOptions.ApiKey;
            else this.ApiKey = apiKey;
        }

       
        public override string ToString()
        {
            return string.Format(
                "<{0} Method={1} Uri={2} ContentType={3} RequestOptions={4}>",
                this.GetType().FullName,
                this.Method,
                this.ApiBase + this.Path,
                this.ContentType,
                this.RequestOptions.ToString()
                );
        }

    }
}
