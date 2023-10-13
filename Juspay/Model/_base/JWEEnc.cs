namespace Juspay
{
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using Jose;
    public class JWEEnc : IEnc
    {
        public string Encrypt(string publicKey, string keyId, string payload)
        {
            #if NETFRAMEWORK
                RSA RSAPublicKey = RSAReader.ReadRsaKeyFromPemFile(publicKey);
            #else
                RSA RSAPublicKey = RSA.Create();
                RSAPublicKey.ImportFromPem(publicKey);
            #endif
            var jweToken = JWT.Encode(payload, RSAPublicKey, JweAlgorithm.RSA_OAEP_256, JweEncryption.A256GCM, null, new Dictionary<string, object> {{ "kid", keyId }});
            return jweToken;
        }

        public string Decrypt(string privateKey, string encPaylaod)
        {
            #if NETFRAMEWORK
                RSA RSAPrivateKey = RSAReader.ReadRsaKeyFromPemFile(privateKey);
            #else
                RSA RSAPrivateKey = RSA.Create();
                RSAPrivateKey.ImportFromPem(privateKey);
            #endif
            string jweToken = JWT.Decode(encPaylaod, RSAPrivateKey, JweAlgorithm.RSA_OAEP_256, JweEncryption.A256GCM);
            return jweToken;
        }
    }
}