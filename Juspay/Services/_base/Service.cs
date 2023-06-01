namespace Juspay {
    using System.Threading.Tasks;
    using System.Net.Http;
    using System.Net;
    using System;
    public class Service<TModelReturned> where TModelReturned : IJuspayResponseEntity {
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
            get => this.client ?? JuspayEnvironment.JuspayClient;
            set => this.client = value;
        }

        public virtual string BasePath { get; set; }

        public virtual string BaseUrl => this.Client.ApiBase;

         protected virtual string InstanceUrl(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new JuspayException("The id cannot be null or whitespace." + nameof(id));
            }
            return $"{this.BasePath}/{Uri.EscapeDataString(id)}";
        }

        private object getInput(JuspayEntity input) {
            if (input != null && input.Data != null) {
                return input.Data;
            }
            return input;
        }
        public async Task<TModelReturned> CreateAsync(JuspayEntity input, RequestOptions requestOptions, string contentType = "application/x-www-form-urlencoded", string prefix = "")
        {
            return await this.Client.RequestAsync<TModelReturned>(HttpMethod.Post, this.BasePath + prefix, getInput(input), null, requestOptions, contentType).ConfigureAwait(false);
        }
        public async Task<TModelReturned> GetAsync(string id, JuspayEntity input, object queryParams, RequestOptions requestOptions, string contentType = "", string prefix = "") {
            return await this.Client.RequestAsync<TModelReturned>(HttpMethod.Get, this.InstanceUrl(id) + prefix, input, queryParams, requestOptions, contentType).ConfigureAwait(false);
        }
        public TModelReturned Create(JuspayEntity input, RequestOptions requestOptions, string contentType = "application/x-www-form-urlencoded", string prefix = "") {
             return CreateAsync(input, requestOptions, contentType, prefix).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        public TModelReturned Get(string id, JuspayEntity input, object queryParams, RequestOptions requestOptions, string contentType = "", string prefix = "") {
            return GetAsync(id, input, queryParams, requestOptions, contentType, prefix).ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}