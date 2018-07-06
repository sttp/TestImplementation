using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CTP.SRP;

namespace CTP.Authentication.SRP
{
    [DocumentName("ClientProof")]
    public class ClientProof
        : DocumentObject<ClientProof>
    {
        public string UserToken;
        public string[] RequestedAccess;
        public byte[] ServerProof;

        public ClientProof(string userToken, string[] requestedAccess, byte[] serverProof)
        {
            UserToken = userToken;
            RequestedAccess = requestedAccess;
            ServerProof = serverProof;
        }

        private ClientProof()
        {

        }

        public static explicit operator ClientProof(CtpDocument obj)
        {
            return FromDocument(obj);
        }

        public SrpClientProof Encrypt(byte[] publicA, byte[] privateSessionKey)
        {
            byte[] clearData = ToDocument().ToArray();
            byte[] cipherData;
            byte[] hmac;
            byte[] iv = new byte[16];
            byte[] key = new byte[32];

            Array.Copy(privateSessionKey, 0, iv, 0, 16);
            Array.Copy(privateSessionKey, 16, key, 0, 32);

            using (var aes = Aes.Create())
            {
                aes.IV = iv;
                aes.Key = key;
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.CBC;
                using (var enc = aes.CreateEncryptor())
                {
                    cipherData = enc.TransformFinalBlock(clearData, 0, clearData.Length);
                }
            }

            using (var mac = new HMACSHA256(privateSessionKey))
            {
                hmac = mac.ComputeHash(cipherData, 0, cipherData.Length);
            }

            return new SrpClientProof(publicA, cipherData, hmac);
        }



    }
}
