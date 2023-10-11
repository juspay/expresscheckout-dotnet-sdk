using System.Collections.Generic;

namespace Juspay
{
    public interface ISign
    {
        string Sign(string key, string keyId, string data);
        string VerifySign(string key, string data);
    }
}