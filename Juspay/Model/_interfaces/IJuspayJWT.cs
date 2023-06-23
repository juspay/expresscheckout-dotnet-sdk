namespace Juspay
{
    using System.Collections.Generic;
    public interface IJuspayJWT
    {
        Dictionary<string, string> Keys { get; set; }

        string PreparePayload(string payload);

        string ConsumePayload(string encPaylaod);

        ISign Sign { get; set; }

        IEnc Enc { get; set; }

    }    
    
}