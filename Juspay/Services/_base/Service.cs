namespace Juspay {
    using System.Threading.Tasks;
    using System.Net.Http;
    using System.Net;
    using System;
    public class Service {
        private IJuspayClient client;
        protected Service()
        {
        }
        protected Service(IJuspayClient client)
        {
            this.client = client;
        }
         public IJuspayClient Client
        {
            get => this.client ?? JuspayEnvironment.Instance.JuspayClient;
            set => this.client = value;
        }

        public virtual string BasePath { get; set; }

         protected virtual string InstanceUrl(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new JuspayException("ID_REQUIRED_IN_INSTANCE_URL" + nameof(id));
            }
            return $"{this.BasePath}/{Uri.EscapeDataString(id)}";
        }

        private object getInput(JuspayEntity input) {
            if (input != null && input.Data != null) {
                return input.Data;
            }
            return input;
        }

        protected bool shouldUseJwt(RequestOptions requestOptions)
        {
            if ((requestOptions != null && requestOptions.JuspayJWT != null) || JuspayEnvironment.Instance.JuspayJWT != null)
            {
                return true;
            }
            return false;
        }
        public async Task<JuspayResponse> CreateAsync(JuspayEntity input, RequestOptions requestOptions, ContentType contentType = ContentType.FormUrlEncoded, bool isJWTSupported = false, string prefix = "", object queryParams = null)
        {
            return await this.Client.RequestAsync(HttpMethod.Post, this.BasePath + prefix, getInput(input), queryParams , requestOptions, contentType, isJWTSupported).ConfigureAwait(false);
        }
        public async Task<JuspayResponse> GetAsync(string id, JuspayEntity input, object queryParams, RequestOptions requestOptions, ContentType contentType = ContentType.FormUrlEncoded,  bool isJWTSupported = false, string prefix = "") {
            return await this.Client.RequestAsync(HttpMethod.Get, this.InstanceUrl(id) + prefix, input, queryParams, requestOptions, contentType, isJWTSupported).ConfigureAwait(false);
        }
        public JuspayResponse Create(JuspayEntity input, RequestOptions requestOptions, ContentType contentType = ContentType.FormUrlEncoded,  bool isJWTSupported = false, string prefix = "", object queryParams = null) {
             return CreateAsync(input, requestOptions, contentType, isJWTSupported, prefix, queryParams).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        public JuspayResponse Get(string id, JuspayEntity input, object queryParams, RequestOptions requestOptions, ContentType contentType = ContentType.FormUrlEncoded,  bool isJWTSupported = false, string prefix = "") {
            return GetAsync(id, input, queryParams, requestOptions, contentType, isJWTSupported, prefix).ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}