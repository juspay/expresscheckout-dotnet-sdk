namespace Juspay
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class JuspayJWTRSA : IJuspayJWT
    {
        public JuspayJWTRSA (Dictionary<string, object> keys)
        {
            Keys = keys;
        }

        public ISign Sign { get; set; }

        public IEnc Enc { get; set; }

        public Dictionary<string, object> Keys { get; set; }
        public string PreparePayload(string payload)
        {
            try
            {
                string signedData = Sign.Sign(Keys["privateKey"] as Dictionary<string, object>, payload);
              #if !NETFRAMEWORK
                    string[] split = signedData.Split(".");
                #else
                    string[] split = signedData.Split(new[] { "." }, StringSplitOptions.None);
                #endif
                signedData = $"{{\"header\":\"{split[0]}\",\"payload\":\"{split[1]}\",\"signature\":\"{split[2]}\"}}";
                string encryptedPayload = Enc.Encrypt(Keys["publicKey"] as Dictionary<string, object>, signedData);
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
            var encryptedJsonObject = JsonConvert.DeserializeObject<dynamic>(encryptedResponse); 
            string signedPayload = Enc.Decrypt(Keys["privateKey"] as Dictionary<string, object>, $"{encryptedJsonObject.header}.{encryptedJsonObject.encryptedKey}.{encryptedJsonObject.iv}.{encryptedJsonObject.encryptedPayload}.{encryptedJsonObject.tag}");
            var signedJsonObject = JsonConvert.DeserializeObject<dynamic>(signedPayload);
            return Sign.VerifySign(Keys["publicKey"] as Dictionary<string, object>, $"{signedJsonObject.header}.{signedJsonObject.payload}.{signedJsonObject.signature}");
           }
           catch (Exception e)
           {
            throw new JWTException(e.Message ?? "CONSUMER_PAYLOAD_JWT_EXCEPTION");
           }
        }

        public void Initialize()
        {
            Sign = new JWTSign();
            Enc = new JWEEnc();
        }
    }
}