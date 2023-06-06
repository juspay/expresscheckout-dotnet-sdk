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
        string ApiBase { get; set; }

        // string MerchantId { get; }

        Task<T> RequestAsync<T>(HttpMethod method, string path, object input, object queryParams, RequestOptions requestOptions, string contentType)
            where T : IJuspayResponseEntity, new();
    }
}