using System;
using System.Drawing.Text;
using System.Linq;
using System.Security.Cryptography;
using CTP.Authentication.SRP;

namespace CTP.SRP
{
    [DocumentName("SrpClientProof")]
    public class SrpClientProof
        : DocumentObject<SrpClientProof>
    {
        [DocumentField()] public byte[] PublicA { get; private set; }
        [DocumentField()] public byte[] ClientProofCipher { get; private set; }
        [DocumentField()] public byte[] ClientProofHMAC { get; private set; }

        public SrpClientProof(byte[] publicA, byte[] clientProofCipher, byte[] clientProofHmac)
        {
            PublicA = publicA;
            ClientProofCipher = clientProofCipher;
            ClientProofHMAC = clientProofHmac;
        }

        private SrpClientProof()
        {

        }

        public ClientProof Decrypt(byte[] privateSessionKey)
        {
            byte[] clearData;
            byte[] iv = new byte[16];
            byte[] key = new byte[32];

            Array.Copy(privateSessionKey, 0, iv, 0, 16);
            Array.Copy(privateSessionKey, 16, key, 0, 32);

            using (var mac = new HMACSHA256(privateSessionKey))
            {
                var hmac = mac.ComputeHash(ClientProofCipher, 0, ClientProofCipher.Length);
                if (!hmac.SequenceEqual(ClientProofHMAC))
                {
                    throw new Exception("Authentication Failed");
                }
            }

            using (var aes = Aes.Create())
            {
                aes.IV = iv;
                aes.Key = key;
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.CBC;
                using (var enc = aes.CreateDecryptor())
                {
                    clearData = enc.TransformFinalBlock(ClientProofCipher, 0, ClientProofCipher.Length);
                }
            }

            return (ClientProof)new CtpDocument(clearData);
        }

        public static explicit operator SrpClientProof(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}