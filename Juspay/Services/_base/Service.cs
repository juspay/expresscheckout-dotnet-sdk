namespace Juspay {
    using System.Threading.Tasks;
    using System.Net.Http;
    using System.Net;
    public class Service<TModelReturned> where TModelReturned : IJuspayEntity{
        private IJuspayClient? client;
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

        public virtual string? BasePath { get; set; }

        public virtual string? BaseUrl => this.Client.ApiBase;

         protected virtual string InstanceUrl(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new JuspayException("The id cannot be null or whitespace." + nameof(id));
            }
            return $"{this.BasePath}/{Uri.EscapeDataString(id)}";
        }

        private object getInput(JuspayEntity input) {
            if (input != null && input.DictionaryObject != null) {
                return input.DictionaryObject;
            }
            return input;
        }
        public async Task<TModelReturned> CreateAsync(JuspayEntity input, RequestOptions requestOptions, string contentType = "application/x-www-form-urlencoded", string prefix = "")
        {
            return await this.Client.RequestAsync<TModelReturned>(HttpMethod.Post, this.BasePath + prefix, getInput(input), null, requestOptions, contentType).ConfigureAwait(false);
        }
        public async Task<TModelReturned> GetAsync(string id, JuspayEntity? input, object queryParams, RequestOptions requestOptions, string contentType = "", string prefix = "") {
            return await this.Client.RequestAsync<TModelReturned>(HttpMethod.Get, this.InstanceUrl(id) + prefix, input, queryParams, requestOptions, contentType).ConfigureAwait(false);
        }
        // public async Task<TModelReturned> UpdateAsync(string url, Dictionary<string, string> pathParams, object input, RequestOptions requestOptions) {
        //     return await this.Client.RequestAsync<TModelReturned>(input, url, pathParams, requestOptions).ConfigureAwait(false);
        // }
        public TModelReturned Create(JuspayEntity input, RequestOptions requestOptions, string contentType = "application/x-www-form-urlencoded", string prefix = "") {
             return CreateAsync(input, requestOptions, contentType, prefix).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        public TModelReturned Get(string id, JuspayEntity? input, object queryParams, RequestOptions requestOptions, string contentType = "", string prefix = "") {
            return GetAsync(id, input, queryParams, requestOptions, contentType, prefix).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        // public TModelReturned Update(string id,  object input, RequestOptions requestOptions) {
        //     return UpdateAsync(url, pathParams, client, input, requestOptions).ConfigureAwait(false).GetAwaiter().GetResult();
        // }
    }
}