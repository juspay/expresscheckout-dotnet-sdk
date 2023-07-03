namespace Juspay
{
    using System.Collections.Generic;
    public interface IEnc
    {
        string Encrypt(Dictionary<string, object> key, string plainText);
        string Decrypt(Dictionary<string, object> key, string encryptedPayload);
    }
}