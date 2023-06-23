namespace Juspay
{
    using Jose;
    using System;
    using System.Security.Cryptography;
    public class JWTSign : ISign
    {
        public string Sign (string privateKey, string payload)
        {
            RSA RSAPrivateKey = RSA.Create();
            RSAPrivateKey.ImportFromPem(privateKey);
            return JWT.Encode(payload, RSAPrivateKey, JwsAlgorithm.RS256);
        }

        public string VerifySign(string publicKey, string signedPayload)
        {
            RSA RSAPublicKey = RSA.Create();
            RSAPublicKey.ImportFromPem(publicKey);
            return JWT.Decode(signedPayload, RSAPublicKey, JwsAlgorithm.RS256);
        }
    }
}