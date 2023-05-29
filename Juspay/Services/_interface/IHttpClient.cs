namespace Juspay
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IHttpClient
    {

        Task<JuspayResponse> MakeRequestAsync(JuspayRequest request);

    }
}
