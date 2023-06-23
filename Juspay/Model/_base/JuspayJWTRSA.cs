namespace Juspay
{
    using System.Collections.Generic;

    public class JuspayJWTRSA : IJuspayJWT
    {
        public JuspayJWTRSA (Dictionary<string, string> keys, ISign sign, IEnc enc)
        {
            Sign = sign;
            Enc = enc;
            Keys = keys;
        }

        public ISign Sign { get; set; }

        public IEnc Enc { get; set; }
        public Dictionary<string, string> Keys { get; set; }
        public string PreparePayload(string payload)
        {
            string signedData = Sign.Sign(Keys["privateKey"], payload);
            string encryptedPayload = Enc.Encrypt(Keys["publicKey"], signedData);
            return encryptedPayload;
        }

        public string ConsumePayload(string encryptedResponse)
        {
            string signedPayload = Enc.Decrypt(Keys["privateKey"], encryptedResponse);
            return Sign.VerifySign(Keys["publicKey"], signedPayload);
        }
    }
}