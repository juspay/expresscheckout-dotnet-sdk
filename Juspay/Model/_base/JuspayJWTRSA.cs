namespace Juspay
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public interface IJuspayJWT
    {

        string PreparePayload(string payload);

        string ConsumePayload(string encPaylaod);

    }

    public class JuspayJWT : IJuspayJWT
    {
        

        public string KeyId { get; set; }
        public JWS Jws { get; set; }

        public JWE Jwe { get; set; }
        public string PreparePayload(string payload)
        {
            try
            {
                string signedData = Jws.Encode(payload, KeyId);
                #if !NETFRAMEWORK
                    string[] split = signedData.Split(".");
                #else
                    string[] split = signedData.Split(new[] { "." }, StringSplitOptions.None);
                #endif
                signedData = $"{{\"header\":\"{split[0]}\",\"payload\":\"{split[1]}\",\"signature\":\"{split[2]}\"}}";
                string encryptedPayload = Jwe.CreateJWE(signedData, KeyId);
                #if !NETFRAMEWORK
                    string[] encSplit = encryptedPayload.Split(".");
                #else
                    string[] encSplit = encryptedPayload.Split(new[] { "." }, StringSplitOptions.None);
                #endif
                return $"{{\"header\":\"{encSplit[0]}\",\"encryptedKey\":\"{encSplit[1]}\",\"iv\":\"{encSplit[2]}\",\"encryptedPayload\":\"{encSplit[3]}\",\"tag\":\"{encSplit[4]}\"}}";
            }
            catch (Exception e)
            {
                throw new JWTException(e.Message ?? "PREPARE_PAYLOAD_JWT_EXCEPTION");
            }
        }

        public string ConsumePayload(string encryptedResponse)
        {
           try
           {
            var encryptedJsonObject = JsonConvert.DeserializeObject<Dictionary<string, string>>(encryptedResponse); 
            string signedPayload = Jwe.DecryptJWE($"{encryptedJsonObject["header"]}.{encryptedJsonObject["encryptedKey"]}.{encryptedJsonObject["iv"]}.{encryptedJsonObject["encryptedPayload"]}.{encryptedJsonObject["tag"]}");
            var signedJsonObject = JsonConvert.DeserializeObject<Dictionary<string, string>>(signedPayload);
            return Jws.Decode($"{signedJsonObject["header"]}.{signedJsonObject["payload"]}.{signedJsonObject["signature"]}");
           }
           catch (Exception e)
           {
            throw new JWTException(e.Message ?? "CONSUMER_PAYLOAD_JWT_EXCEPTION");
           }
        }
    }

    public class JuspayJWTRSA : JuspayJWT
    {
       
        public JuspayJWTRSA(string keyId, string publicKey, string privateKey)
        {
            this.KeyId = keyId;
            this.Jwe = new JWEAES(publicKey, privateKey);
            this.Jws = new JWSRSA(publicKey, privateKey);
        }
    }

    // JuspayAES : IJuspayJWT
}