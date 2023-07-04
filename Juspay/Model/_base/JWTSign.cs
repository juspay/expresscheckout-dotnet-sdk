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
            #if NETFRAMEWORK
                RSA RSAPrivateKey = RSAReader.ReadRsaKeyFromPemFile((string)privateKey["key"]);
            #else
                RSA RSAPrivateKey = RSA.Create();
                RSAPrivateKey.ImportFromPem((string)privateKey["key"]);
            #endif
            return JWT.Encode(payload, RSAPrivateKey, JwsAlgorithm.RS256, new Dictionary<string, object> { { "kid", (string)privateKey["kid"] }});
        }

        public string VerifySign(Dictionary<string, object> publicKey, string signedPayload)
        {
            #if NETFRAMEWORK
                RSA RSAPublicKey = RSAReader.ReadRsaKeyFromPemFile((string)publicKey["key"]);
            #else
                RSA RSAPublicKey = RSA.Create();
                RSAPublicKey.ImportFromPem((string)publicKey["key"]);
            #endif
            return JWT.Decode(signedPayload, RSAPublicKey, JwsAlgorithm.RS256);
        }
    }
}