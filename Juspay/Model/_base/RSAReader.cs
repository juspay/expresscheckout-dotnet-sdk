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
                throw new JWTException("Invalid RSA key format.");
            }
        }
    }
}
#endif