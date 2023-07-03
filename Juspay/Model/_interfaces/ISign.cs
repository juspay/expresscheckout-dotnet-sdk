using System.Collections.Generic;

namespace Juspay
{
    public interface ISign
    {
        string Sign(Dictionary<string, object> key, string data);
        string VerifySign(Dictionary<string, object> key, string data);
    }
}