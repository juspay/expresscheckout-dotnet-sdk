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

        public virtual string? BasePath { get; }

        public virtual string? BaseUrl => this.Client.ApiBase;

         protected virtual string InstanceUrl(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new JuspayException("The id cannot be null or whitespace." + nameof(id));
            }
            Console.WriteLine(this.BasePath);
            return $"{this.BasePath}/{Uri.EscapeDataString(id)}";
        }

        public async Task<TModelReturned> CreateAsync(object input, RequestOptions requestOptions, string contentType = "application/x-www-form-urlencoded")
        {
            return await this.Client.RequestAsync<TModelReturned>(HttpMethod.Post, this.BasePath, input, null, requestOptions, contentType).ConfigureAwait(false);
        }
        public async Task<TModelReturned> GetAsync(string id, object? input, object queryParams, RequestOptions requestOptions, string contentType = "") {
            return await this.Client.RequestAsync<TModelReturned>(HttpMethod.Get, this.InstanceUrl(id), input, queryParams, requestOptions, contentType).ConfigureAwait(false);
        }
        // public async Task<TModelReturned> UpdateAsync(string url, Dictionary<string, string> pathParams, object input, RequestOptions requestOptions) {
        //     return await this.Client.RequestAsync<TModelReturned>(input, url, pathParams, requestOptions).ConfigureAwait(false);
        // }
        public TModelReturned Create(object input, RequestOptions requestOptions, string contentType = "application/x-www-form-urlencoded") {
             return CreateAsync(input, requestOptions, contentType).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        public TModelReturned Get(string id, object? input, object queryParams, RequestOptions requestOptions, string contentType = "") {
            return GetAsync(id, input, queryParams, requestOptions, contentType).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        // public TModelReturned Update(string id,  object input, RequestOptions requestOptions) {
        //     return UpdateAsync(url, pathParams, client, input, requestOptions).ConfigureAwait(false).GetAwaiter().GetResult();
        // }
    }
}