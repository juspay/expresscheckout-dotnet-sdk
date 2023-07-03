using Jose;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
namespace Juspay
{
    public class JWTSign : ISign
    {
        public string Sign (Dictionary<string, object> privateKey, string payload)
        {
            RSA RSAPrivateKey = RSA.Create();
            RSAPrivateKey.ImportFromPem((string)privateKey["key"]);
            return JWT.Encode(payload, RSAPrivateKey, JwsAlgorithm.RS256, new Dictionary<string, object> { { "kid", (string)privateKey["kid"] }});
        }

        public string VerifySign(Dictionary<string, object> publicKey, string signedPayload)
        {
            RSA RSAPublicKey = RSA.Create();
            RSAPublicKey.ImportFromPem((string)publicKey["key"]);
            return JWT.Decode(signedPayload, RSAPublicKey, JwsAlgorithm.RS256);
        }
    }
}