namespace Juspay
{
    using System.Collections.Generic;
    public interface IJuspayJWT
    {
        Dictionary<string, object> Keys { get; set; }

        string PreparePayload(string payload);

        string ConsumePayload(string encPaylaod);

        void Initialize();

        ISign Sign { get; set; }

        IEnc Enc { get; set; }

    }    
    
}