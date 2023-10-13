using Jose;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
namespace Juspay
{
    public class JWTSign : ISign
    {
        public string Sign (string privateKey, string keyId, string payload)
        {
            #if NETFRAMEWORK
                RSA RSAPrivateKey = RSAReader.ReadRsaKeyFromPemFile(privateKey);
            #else
                RSA RSAPrivateKey = RSA.Create();
                RSAPrivateKey.ImportFromPem(privateKey);
            #endif
            return JWT.Encode(payload, RSAPrivateKey, JwsAlgorithm.RS256, new Dictionary<string, object> { { "kid", keyId }});
        }

        public string VerifySign(string publicKey, string signedPayload)
        {
            #if NETFRAMEWORK
                RSA RSAPublicKey = RSAReader.ReadRsaKeyFromPemFile(publicKey);
            #else
                RSA RSAPublicKey = RSA.Create();
                RSAPublicKey.ImportFromPem(publicKey);
            #endif
            return JWT.Decode(signedPayload, RSAPublicKey, JwsAlgorithm.RS256);
        }
    }
}