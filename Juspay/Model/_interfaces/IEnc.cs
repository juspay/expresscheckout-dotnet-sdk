namespace Juspay
{
    using System.Collections.Generic;
    public interface IEnc
    {
        string Encrypt(string key, string keyId, string plainText);
        string Decrypt(string key, string encryptedPayload);
    }
}