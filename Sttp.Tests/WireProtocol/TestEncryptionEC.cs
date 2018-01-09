using System;
using System.IO;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sttp.Tests.WireProtocol
{
    [TestClass]
    public class TestEncryptionEC
    {
        private static readonly string ServerPublicKey = "RUNLMSAAAACowsltqGSVsBq2o5zKWIh0x8kiIVmM53RRtD1k023T02rRYU9FgaJ9kpRfA8EiSvBekoMig1tJmhHSWdIbh8P7";
        private static readonly string ServerPrivateKey = "RUNLMiAAAACowsltqGSVsBq2o5zKWIh0x8kiIVmM53RRtD1k023T02rRYU9FgaJ9kpRfA8EiSvBekoMig1tJmhHSWdIbh8P7ZqEor1Pk+/I65HiKSnPnyAJ0Y+djauj+FmsMZOIo4hY=";

        private static readonly string ClientPublicKey = "RUNLMSAAAABBZLzBCzE6T3hBl085FfE7b2z5dtlX43k1/DqjXnmIBOCI584W9/VnskZVUQlls2S0HEjKlEmjZheVXsN/dBxN";
        private static readonly string ClientPrivateKey = "RUNLMiAAAABBZLzBCzE6T3hBl085FfE7b2z5dtlX43k1/DqjXnmIBOCI584W9/VnskZVUQlls2S0HEjKlEmjZheVXsN/dBxNj2XBNF76tVrywRWFiHcdleuqrx5nI0zbWmvoWA5+iZw=";

        private static readonly string PassPhrase = "1234567";

        [TestMethod]
        public void TestRSA()
        {
            var clientKey = CngKey.Import(Convert.FromBase64String(ClientPublicKey), CngKeyBlobFormat.EccPublicBlob);
            var serverKey = CngKey.Import(Convert.FromBase64String(ClientPrivateKey), CngKeyBlobFormat.EccPrivateBlob);

            using (var clientEc = new ECDiffieHellmanCng(clientKey))
            using (var serverEc = new ECDiffieHellmanCng(serverKey))
            {
                var rng = RandomNumberGenerator.Create();
                byte[] epic = new byte[4];
                byte[] key = new byte[32];
                byte[] mac = new byte[32];
                byte[] nonce = new byte[32];
                byte[] iv = new byte[32];
                byte[] dataToEncrypt = new byte[64];

                rng.GetBytes(epic);
                rng.GetBytes(key);
                rng.GetBytes(mac);
                rng.GetBytes(nonce);
                rng.GetBytes(iv);

                key.CopyTo(dataToEncrypt, 0);
                mac.CopyTo(dataToEncrypt, 32);

                byte[] keyData =  serverEc.DeriveKeyMaterial(clientKey);
                foreach (var b in keyData)
                {
                    Console.Write(b.ToString("X2") + " ");
                }

                Console.WriteLine();
                return;

                //byte[] encryptionData = clientEc.Encrypt(dataToEncrypt, true);

                //var ms = new MemoryStream();
                //var wr = new BinaryWriter(ms);
                //wr.Write((byte)0);
                //wr.Write(epic);
                //wr.Write(DateTime.UtcNow.AddMinutes(-5).Ticks);
                //wr.Write(DateTime.UtcNow.AddHours(1).Ticks);
                //wr.Write(nonce);
                //wr.Write(encryptionData.Length);
                //wr.Write(encryptionData);

                //var signature = serverEc.SignData(ms.ToArray(), new SHA1CryptoServiceProvider());
                //wr.Write(signature);

                //foreach (var b in ms.ToArray())
                //{
                //    Console.Write(b.ToString("X2") + " ");
                //}
                //Console.WriteLine();
            }
        }

        [TestMethod]
        public void GenerateKeyPairEC()
        {
            var cngKey = CngKey.Create(CngAlgorithm.ECDiffieHellmanP256, null, new CngKeyCreationParameters { ExportPolicy = CngExportPolicies.AllowPlaintextExport });

            var privateKey = cngKey.Export(CngKeyBlobFormat.EccPrivateBlob);
            var publicKey = cngKey.Export(CngKeyBlobFormat.EccPublicBlob);

            Console.WriteLine("Public Key");
            Console.WriteLine(Convert.ToBase64String(publicKey));
            Console.WriteLine("Private Key");
            Console.WriteLine(Convert.ToBase64String(privateKey));
        }
      
    }
}
