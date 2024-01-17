using Xunit;
using System;
using Juspay;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace JuspayTest
{
    public class JWTTest
    {
        public static void readPublicKeyRSATest()
        {
            string publicKey = File.ReadAllText("../../../publicKey2.pem");
            var ex = Record.Exception(() => new JWTRSAKey() { PublicKey = publicKey });
            Assert.Null(ex);
        }
        public static void invalidRSAPublicKeyTest()
        {
            string invalidPublicKey = File.ReadAllText("../../../invalidPublicKey.pem");
            var ex = Assert.Throws<JWKException>(() => new JWTRSAKey() { PublicKey = invalidPublicKey });
        }
        public static void readPrivateKeyPkcs1Test()
        {
            string privateKey = File.ReadAllText("../../../testPrivateKeyPkcs1.pem");
            var ex = Record.Exception(() => new JWTRSAKey() { PrivateKey = privateKey });
            Assert.Null(ex);
        }
        public static void readPrivateKeyPkcs8Test()
        {
            string privateKey = File.ReadAllText("../../../testPrivateKeyPkcs8.pem");
            var ex = Record.Exception(() => new JWTRSAKey() { PrivateKey = privateKey });
            Assert.Null(ex);
        }
        public static void invalidPrivateKeyTest()
        {
           string ivalidPrivatekey = File.ReadAllText("../../../invalidPrivateKey.pem");   
           Assert.Throws<JWKException>(() => new JWTRSAKey() { PrivateKey = ivalidPrivatekey });
        }

        public static void testSigningJWSRSA()
        {
            string payload = "hello world 😀 வணக்கம்";
            string keyId = "key_xxxxx";
            string successResponse = "eyJhbGciOiJSUzI1NiIsImtpZCI6ImtleV94eHh4eCJ9.aGVsbG8gd29ybGQg8J-YgCDgrrXgrqPgrpXgr43grpXgrq7gr40.ZIEuPUIPBwA4mQD1RuYz2HvT76wK9hgCMjVCbdV6RPHJlH_44eFe9Eig01EXnbh6m9ZRIq47h6W8sQy2pIoJaiAfSO72GhmxJQDBtlbzAJqUBrb1YfACOiZbCqnL_zHvHNVomSCLsCBPPeslehnjihtTvgLXQDyH9RyNadvkPxhun5sjq2AvOTXyBWiwHx2DlD6dcN1yOUPy8Px3Nhrq1-qNmHbjb6JvW2TaBKeeWcTz85VZBCILHvM0p3SNEcY1n0tgjtSZGZHDYHgrE0vCfD18bYHDST8j9_SMvyUce1UgfQmd3w4uz1zi0qE1_mD0WC0Pj0m_P72U4D_XX6bf6Q";
            string privateKey = File.ReadAllText("../../../privateKey1.pem");
            string publicKey =  File.ReadAllText("../../../publicPairPrivate.pem");
            var jws = new JWSRSA(publicKey, privateKey);
            Assert.True(successResponse == jws.Encode(payload, keyId));
            Assert.True(payload == jws.Decode(successResponse));
        }
        public static void testVerifySignatureRSAFailure()
        {
            string signedPayload = "eyJhbGciOiJSUzI1NiIsImtpZCI6ImtleV94eHh4eCJ9.aGVsbG8gd29ybGQg8J-YgCDgrrXgrqPgrpXgr43grpXgrq7gr40.NYXOlfiLgR9JzKi9t23CkaHVfrYRQRM2MV1v2piMFHldpKG5MI9CcE444P-9C17YanGFpC4ll_aDz9sWOUEGZHg6l_hp2WtrZqZ4YlC9NaXdq2e7Qen6ABE1R-W6XZGYDHkTgtkcGaNDr-jA4U9JEaTfB0dd5-217hVA5yHnhICW3J3llDgRNibU9TyRtq8ijO0ye0cJNjr47ugm_Dx6slSVB6QayT8sfBTHULsIVL3LX9DQNnWOGWG_ck6usjKzPYMMurcedOgUTkLOJcPIdeeAHrm27OD17L3I8viTCqrqxpoiYTjJvYaa7cGe7rH0-T2QH8NOqWwd2BZSCo93oA";
            string privateKey = File.ReadAllText("../../../privateKey1.pem");
            string publicKey =  File.ReadAllText("../../../publicPairPrivate.pem");
            var jws = new JWSRSA(publicKey, privateKey);
            Assert.Throws<JWSException>(() => jws.Decode(signedPayload));
        }

        public static void testKeyEncryptionRSA()
        {
            string key = "6H6blPsPec6BmobOn/92haednPljSamTWFLCMwhvpzo=";
            string privateKey = File.ReadAllText("../../../privateKey1.pem");
            string publicKey =  File.ReadAllText("../../../publicPairPrivate.pem");
            var jwe = new RSAAESGCM(publicKey, privateKey);
            var encKey = jwe.EncryptKey(Convert.FromBase64String(key));
            Assert.True(key == Convert.ToBase64String(jwe.DecryptKey(encKey)));
        }

        public static void testContentEncryptionAESGCM()
        {
            string stPayload = "hello world 😀 வணக்கம்";
            string privateKey = File.ReadAllText("../../../privateKey1.pem");
            string publicKey =  File.ReadAllText("../../../publicPairPrivate.pem");
            var jwe = new RSAAESGCM(publicKey, privateKey);
            string encodedHeaders = Base64Url.encodeToBase64Url(JsonSerializer.Serialize(new Dictionary<string, string> { { "alg", jwe.getKeyEncAlgorithmName() }, { "enc", jwe.getEncAlgorithmName() }, { "kid", "key_xxxxx" } }));
            byte[] aad = Encoding.UTF8.GetBytes(encodedHeaders);
            byte[] payload = Encoding.UTF8.GetBytes(stPayload);
            string cipherText = Base64Url.encodeToBase64UrlByte(jwe.Encrypt(payload, aad));
            string decryptedText = Encoding.UTF8.GetString(jwe.Decrypt(Base64Url.DecodeBase64Url(cipherText), jwe.Key, jwe.Nonce, jwe.Tag, aad));
            Assert.True(decryptedText == stPayload);
        }

        public static void testJWTRSAAESGCM()
        {
            string payload = "hello world 😀 வணக்கம்";
            string privateKey = File.ReadAllText("../../../privateKey1.pem");
            string publicKey =  File.ReadAllText("../../../publicPairPrivate.pem");
            var juspayJWTRSA = new JuspayJWTRSA("key_xxxxx", publicKey, privateKey);
            string encryptedPayload = juspayJWTRSA.PreparePayload(payload);
            var encryptedJsonObject = JsonSerializer.Deserialize<Dictionary<string, string>>(encryptedPayload); 
            var headers = JsonSerializer.Deserialize<Dictionary<string, string>>(Encoding.UTF8.GetString(Base64Url.DecodeBase64Url((string)encryptedJsonObject["header"])));
            Assert.True(headers["alg"] == "RSA-OAEP-256");
            Assert.True(headers["enc"] == "A256GCM");
            Assert.True(headers["kid"] == "key_xxxxx");
            Assert.True(juspayJWTRSA.ConsumePayload(encryptedPayload) == payload);
        }
    }
}