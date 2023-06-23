namespace Juspay
{
    using System;
    using System.Security.Cryptography;
    using Jose;
    public class JWEEnc : IEnc
    {
        public string Encrypt(string publicKey, string payload)
        {
            RSA RSAPublicKey = RSA.Create();
            RSAPublicKey.ImportFromPem(publicKey);
            var jweToken = JWT.Encode(payload, RSAPublicKey, JweAlgorithm.RSA_OAEP_256, JweEncryption.A256GCM);
            return jweToken;
        }

        public string Decrypt(string privateKey, string encPaylaod)
        {
            RSA RSAPrivateKey = RSA.Create();
            RSAPrivateKey.ImportFromPem(privateKey);
            string jweToken = JWT.Decode(encPaylaod, RSAPrivateKey, JweAlgorithm.RSA_OAEP_256, JweEncryption.A256GCM);
            return jweToken;
        }
    }
}