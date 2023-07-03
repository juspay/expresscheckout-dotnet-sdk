namespace Juspay
{
    using System.Collections.Generic;

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
            string signedData = Sign.Sign(Keys["privateKey"] as Dictionary<string, object>, payload);
            string encryptedPayload = Enc.Encrypt(Keys["publicKey"] as Dictionary<string, object>, signedData);
            return encryptedPayload;
        }

        public string ConsumePayload(string encryptedResponse)
        {
            string signedPayload = Enc.Decrypt(Keys["privateKey"] as Dictionary<string, object>, encryptedResponse);
            return Sign.VerifySign(Keys["publicKey"] as Dictionary<string, object>, signedPayload);
        }

        public void Initialize()
        {
            Sign = new JWTSign();
            Enc = new JWEEnc();
        }
    }
}