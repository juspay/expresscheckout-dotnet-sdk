namespace Juspay
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;


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

        public RequestOptions RequestOptions { get; set; }

        public string ApiKey { get; }
        
        public string ApiBase { get; set; }

        public JuspayRequest(HttpMethod method, string path, object input, object queryParams, RequestOptions requestOptions, ContentType contentType, string apiKey, string baseUrl)
        {

            this.RequestOptions = requestOptions;

            this.Method = method;

            this.Path = path;

            this.QueryParams = queryParams;

            this.ContentType = contentType;

            this.RequestOptions = requestOptions;
            
            this.ApiKey = apiKey;

            this.ApiBase = baseUrl;

            this.Input = input;
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
