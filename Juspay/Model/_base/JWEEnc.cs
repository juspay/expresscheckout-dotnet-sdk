namespace Juspay
{
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using Jose;
    public class JWEEnc : IEnc
    {
        public string Kid;
        public string Encrypt(Dictionary<string, object> publicKey, string payload)
        {
            #if NETFRAMEWORK
                RSA RSAPublicKey = RSAReader.ReadRsaKeyFromPemFile((string)publicKey["key"]);
            #else
                RSA RSAPublicKey = RSA.Create();
                RSAPublicKey.ImportFromPem((string)publicKey["key"]);
            #endif
            var jweToken = JWT.Encode(payload, RSAPublicKey, JweAlgorithm.RSA_OAEP_256, JweEncryption.A256GCM, null, new Dictionary<string, object> {{ "kid", (string)publicKey["kid"] }});
            return jweToken;
        }

        public string Decrypt(Dictionary<string, object> privateKey, string encPaylaod)
        {
            #if NETFRAMEWORK
                RSA RSAPrivateKey = RSAReader.ReadRsaKeyFromPemFile((string)privateKey["key"]);
            #else
                RSA RSAPrivateKey = RSA.Create();
                RSAPrivateKey.ImportFromPem((string)privateKey["key"]);
            #endif
            string jweToken = JWT.Decode(encPaylaod, RSAPrivateKey, JweAlgorithm.RSA_OAEP_256, JweEncryption.A256GCM);
            return jweToken;
        }
    }
}