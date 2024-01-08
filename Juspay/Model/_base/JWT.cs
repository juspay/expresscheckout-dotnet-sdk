using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Juspay;

public class Base64Url {
    public static string encodeToBase64Url(string input) {
        byte[] data = Encoding.UTF8.GetBytes(input);
        string base64String = Convert.ToBase64String(data);
        string base64Url = base64String.Replace('+', '-').Replace('/', '_');
        return base64Url.TrimEnd('=');
    }

    public static string encodeToBase64UrlByte (byte[] input) {
        string base64String = Convert.ToBase64String(input);
        string base64Url = base64String.Replace('+', '-').Replace('/', '_');
        return base64Url.TrimEnd('=');
    }

    public static byte[] DecodeBase64Url(string base64Url)
    {
        base64Url = base64Url.Replace('-', '+').Replace('_', '/');
        while (base64Url.Length % 4 != 0)
        {
            base64Url += '=';
        }
        return Convert.FromBase64String(base64Url);
    }
}

public class JWTRSAKey {
    protected RSA privateKey;
    protected RSA publicKey;

    public string PrivateKey {
        set {
            try {
                #if NETFRAMEWORK
                    this.privateKey = RSAReader.ReadRsaKeyFromPemFile(value);
                #else
                    this.privateKey = RSA.Create();
                    this.privateKey.ImportFromPem(value);
                #endif
            } catch (Exception e) {
                throw new JWKException(e.Message);
            }
        }
    }

     public string PublicKey {
        set {
            try {
                #if NETFRAMEWORK
                    this.publicKey = RSAReader.ReadRsaKeyFromPemFile(value);
                #else
                    this.publicKey = RSA.Create();
                    this.publicKey.ImportFromPem(value);
                #endif
            } catch (Exception e) {
                throw new JWKException(e.Message);
            }
        }
    }


}

public interface JwsAlgorithm {
    byte[] Sign(byte[] payload);
    bool VerifySign(byte[] payload, byte[] signature);

    string getAlgorithmName();
}

public interface JweAlgorithm {

    byte[] Key { get; set; }

    byte[] Nonce { get; set; }

    byte[] Tag { get; set; }
    string getKeyEncAlgorithmName();
    string getEncAlgorithmName();

    byte[] Encrypt(byte[] data, byte[] aad);

    byte[] EncryptKey(byte[] key);

    byte[] Decrypt(byte[] cipherText, byte[] key, byte[] nonce, byte[] tag, byte[] aad);

    byte[] DecryptKey(byte[] key);

}

public class RSAAESGCM : JWTRSAKey, JweAlgorithm {

    public RSAAESGCM(string publicKey, string privateKey)
    {
        PublicKey = publicKey;
        PrivateKey = privateKey;
    }
    public string getEncAlgorithmName() { return "A256GCM"; }
    public string getKeyEncAlgorithmName() { return "RSA-OAEP-256"; }

    public byte[] Key { get; set; }

    public byte[] Tag { get; set; }
    
    public byte[] Nonce { get; set; }

    public byte[] Encrypt(byte[] data, byte[] aad)
    {
        try {
            this.Key = new byte[32];
            this.Nonce = new byte[AesGcm.NonceByteSizes.MaxSize];
            this.Tag = new byte[AesGcm.TagByteSizes.MaxSize];
            RandomNumberGenerator.Fill(this.Key);
            RandomNumberGenerator.Fill(this.Nonce);
            AesGcm aesGcm = new AesGcm(this.Key);
            byte[] cipherText = new byte[data.Length];
            aesGcm.Encrypt(this.Nonce, data, cipherText, this.Tag, aad);
            return cipherText;
        } catch (Exception e)
        {
            throw new JWEException(e.Message);
        }
    }
    public byte[] Decrypt(byte[] cipherText, byte[] key, byte[] nonce, byte[] tag, byte[] aad)
    {
         AesGcm aesGcm = new AesGcm(key);
         byte[] plainText = new byte[cipherText.Length];
         aesGcm.Decrypt(nonce, cipherText, tag, plainText, aad);
         return plainText;    
    }

    public byte[] DecryptKey(byte[] key)
    {
        try
        {
             return this.privateKey.Decrypt(key, RSAEncryptionPadding.OaepSHA256);
        } catch (Exception e) {
            throw new JWTException(e.Message);
        }
    }

    public byte[] EncryptKey(byte[] key)
    {
        try
        {
            return this.publicKey.Encrypt(key, RSAEncryptionPadding.OaepSHA256);
        } catch (Exception e)
        {
            throw new JWTException(e.Message);
        }
    }
}

public class RSA256 : JWTRSAKey, JwsAlgorithm {


    public RSA256(string publicKey, string privateKey)
    {
        this.PublicKey = publicKey;
        this.PrivateKey = privateKey;
    }
    public byte[] Sign(byte[] payload) {
        if (privateKey == null) throw new JWTException("private key not initialized");
        try
        {
            return this.privateKey.SignData(payload, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        } catch (Exception e)
        {
            throw new JWSException(e.Message);
        }
    }

    public string getAlgorithmName()
    { 
        return "RS256"; 
    }

    public bool VerifySign(byte[] payload, byte[] signature) {
        if (publicKey == null) throw new JWTException("public key not initialized");
        try
        {
            return this.publicKey.VerifyData(payload, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        } catch (Exception e) 
        {
             throw new JWTException(e.Message);
        }
    }
}
public abstract class JWS {

    public JwsAlgorithm jws;

    public string Encode(string stPayload, string kid) {
        Dictionary<string, string> headers = new Dictionary<string, string> { { "alg", this.jws.getAlgorithmName() }, { "kid", kid } };
        string encodedHeaders = Base64Url.encodeToBase64Url(JsonSerializer.Serialize(headers));
        string encodedPayload = Base64Url.encodeToBase64Url(stPayload);
        string signedData = Base64Url.encodeToBase64UrlByte(jws.Sign(Encoding.UTF8.GetBytes($"{encodedHeaders}.{encodedPayload}")));
        return $"{encodedHeaders}.{encodedPayload}.{signedData}";
    }

    public string Decode(string jwsToken) {
        string [] jwsParts = jwsToken.Split('.');
        try
        {
            string payload = jwsParts[1];
            string headers = jwsParts[0];
            byte[] signature = Base64Url.DecodeBase64Url(jwsParts[2]);
            byte[] data = Encoding.UTF8.GetBytes($"{headers}.{payload}");
            if(!jws.VerifySign(data, signature)) {
                throw new JWSException("unable to verify data");
            }
            return Encoding.UTF8.GetString(Base64Url.DecodeBase64Url(payload));
        } catch (Exception e)
        {
            throw new JWSException(e.Message);
        }
    }
 }

 public class JWSRSA : JWS
 {
    public JWSRSA(string publicKey, string privateKey)
    {
         this.jws = new RSA256(publicKey, privateKey);
    }

 }

 public abstract class JWE
 {

    public JweAlgorithm jwe;

    public string CreateJWE(string stPayload, string kid)
    {
        Dictionary<string, string> headers = new Dictionary<string, string> { { "alg", jwe.getKeyEncAlgorithmName() }, { "enc", jwe.getEncAlgorithmName() }, { "kid", kid } };
        string encodedHeaders = Base64Url.encodeToBase64Url(JsonSerializer.Serialize(headers));
        byte[] aad = Encoding.UTF8.GetBytes(encodedHeaders);
        byte[] payload = Encoding.UTF8.GetBytes(stPayload);
        string cipherText = Base64Url.encodeToBase64UrlByte(jwe.Encrypt(payload, aad));
        string encKey = Base64Url.encodeToBase64UrlByte(jwe.EncryptKey(jwe.Key));
        string nonce = Base64Url.encodeToBase64UrlByte(jwe.Nonce);
        string tag = Base64Url.encodeToBase64UrlByte(jwe.Tag);
        return $"{encodedHeaders}.{encKey}.{nonce}.{cipherText}.{tag}";
    }

    public string DecryptJWE (string jweToken)
    {
        try {
            string[] jweParts = jweToken.Split('.');
            byte[] aad = Encoding.UTF8.GetBytes(jweParts[0]);
            byte[] key = jwe.DecryptKey(Base64Url.DecodeBase64Url(jweParts[1]));
            byte[] nonce = Base64Url.DecodeBase64Url(jweParts[2]);
            byte[] cipherText = Base64Url.DecodeBase64Url(jweParts[3]);
            byte[] tag = Base64Url.DecodeBase64Url(jweParts[4]);
            return Encoding.UTF8.GetString(jwe.Decrypt(cipherText, key, nonce, tag, aad));
        } catch (Exception e) {
            throw new JWEException(e.Message);
        }
    }
 }

 public class JWEAES : JWE
 {
    public JWEAES(string publicKey, string privateKey) {
        this.jwe = new RSAAESGCM(publicKey, privateKey);
    }
 }

#if NETFRAMEWORK
namespace Juspay
{
    using System.Security.Cryptography;
    using System.IO;
    using System;
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.OpenSsl;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Security;
    public class RSAReader
    {
        public static RSA ReadRsaKeyFromPemFile(string pemContents)
        {
            var pemReader = new PemReader(new StringReader(pemContents));
            var keyObject = pemReader.ReadObject();

            if (keyObject is AsymmetricCipherKeyPair keyPair)
            {
                var rsaParams = DotNetUtilities.ToRSAParameters(keyPair.Private as RsaPrivateCrtKeyParameters);
                var rsa = new RSACryptoServiceProvider();
                rsa.ImportParameters(rsaParams);
                return rsa;
            }
            else if (keyObject is RsaKeyParameters keyParameters)
            {
                var rsaParams = DotNetUtilities.ToRSAParameters(keyParameters);
                var rsa = new RSACryptoServiceProvider();
                rsa.ImportParameters(rsaParams);
                return rsa;
            }
            else
            {
                throw new JWTException("INVALID_RSA_KEY_FORMAT");
            }
        }
    }
}
#endif