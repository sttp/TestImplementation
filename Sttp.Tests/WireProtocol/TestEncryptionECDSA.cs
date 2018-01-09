using System;
using System.IO;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sttp.Tests.WireProtocol
{
    [TestClass]
    public class TestEncryptionECDSA
    {
        private static readonly string ServerPublicKey = "RUNTMSAAAAAkWeSIAFfUWK6d+vbjuVwV/Xh0fg2haSvVFblRtlNC1ki02kJIeREstRfhWihNVa7TW5/gPkMHkE3PO1z63iyx";
        private static readonly string ServerPrivateKey = "RUNTMiAAAAAkWeSIAFfUWK6d+vbjuVwV/Xh0fg2haSvVFblRtlNC1ki02kJIeREstRfhWihNVa7TW5/gPkMHkE3PO1z63iyxLx/p8wXr8Q+C7HpMR/qEHwamAhQzRQb+9stzoEXEUC8=";

        private static readonly string ClientPublicKey = "RUNTMSAAAADeFzYpZGpwgr4Jk/JUdnjXyeWfHZkR0sMZqAC3lpv538I2qA2+O4vS5uTzgw/Eiz/vheHlpYgHsaOJEdxUCT8D";
        private static readonly string ClientPrivateKey = "RUNTMiAAAADeFzYpZGpwgr4Jk/JUdnjXyeWfHZkR0sMZqAC3lpv538I2qA2+O4vS5uTzgw/Eiz/vheHlpYgHsaOJEdxUCT8D6fZMPtdcUk5PHUgBPi5P1eQJEoj2eefggz1T4QenBiM=";

        private static readonly string PassPhrase = "1234567";

        [TestMethod]
        public void TestRSA()
        {
            
            var clientKey = CngKey.Import(Convert.FromBase64String(ClientPublicKey), CngKeyBlobFormat.EccPublicBlob);
            var serverKey = CngKey.Import(Convert.FromBase64String(ClientPrivateKey), CngKeyBlobFormat.EccPrivateBlob);

            using (var clientEc = new ECDsaCng(clientKey))
            using (var serverEc = new ECDsaCng(serverKey))
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

                //byte[] encryptionData = clientEc.Encrypt(dataToEncrypt, true);

                var ms = new MemoryStream();
                var wr = new BinaryWriter(ms);
                wr.Write((byte)0);
                wr.Write(epic);
                wr.Write(DateTime.UtcNow.AddMinutes(-5).Ticks);
                wr.Write(DateTime.UtcNow.AddHours(1).Ticks);
                wr.Write(nonce);
                //wr.Write(encryptionData.Length);
                //wr.Write(encryptionData);

                var signature = serverEc.SignData(ms.ToArray());
                wr.Write(signature);
                
                foreach (var b in ms.ToArray())
                {
                    Console.Write(b.ToString("X2") + " ");
                }
                Console.WriteLine();
            }
        }

        [TestMethod]
        public void GenerateKeyPairEC()
        {
            var cngKey = CngKey.Create(CngAlgorithm.ECDsaP256, null, new CngKeyCreationParameters { ExportPolicy = CngExportPolicies.AllowPlaintextExport });

            var privateKey = cngKey.Export(CngKeyBlobFormat.EccPrivateBlob);
            var publicKey = cngKey.Export(CngKeyBlobFormat.EccPublicBlob);

            Console.WriteLine("Public Key");
            Console.WriteLine(Convert.ToBase64String(publicKey));
            Console.WriteLine("Private Key");
            Console.WriteLine(Convert.ToBase64String(privateKey));
        }
      
    }
}
