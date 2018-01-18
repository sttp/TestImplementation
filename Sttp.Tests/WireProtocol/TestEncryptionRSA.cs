using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sttp.Transport;

namespace Sttp.Tests.WireProtocol
{
    [TestClass]
    public class TestEncryptionRSA
    {
        private static readonly string ServerPublicKey = "<RSAKeyValue><Modulus>rMsNTk5GT1qu1a8Uwlk7XZCZNqde6ZpmlUpUdLXT5kALDzDnRe2eJNscLi+Y7GPB/VTL6MMP73SaxZkGuRXuyJLEx/VZgSgKOTd3TEoYhtSD8PHCOQQFvn0ihfE3bR56AHEsTSsDTOmcwK5dyiWkY0Qdmvez/N1+xrpXgY58Dyj5B3XAA4vp9f9GaGqBrxM0XTe4X6APg7nazH8FUlFKxD1BxTxmradmb4sLIKtxuJ3FlgKu583x/Se8HLoZRXc6GuEhurllp837TbtEqTVwN0bifRtgvOQWTNV2R1XBpK+WpYM71/NuEnGtcs6K1qYcy/Pdf6rFj9Sev2X/Vj4A/w==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        private static readonly string ServerPrivateKey = "<RSAKeyValue><Modulus>rMsNTk5GT1qu1a8Uwlk7XZCZNqde6ZpmlUpUdLXT5kALDzDnRe2eJNscLi+Y7GPB/VTL6MMP73SaxZkGuRXuyJLEx/VZgSgKOTd3TEoYhtSD8PHCOQQFvn0ihfE3bR56AHEsTSsDTOmcwK5dyiWkY0Qdmvez/N1+xrpXgY58Dyj5B3XAA4vp9f9GaGqBrxM0XTe4X6APg7nazH8FUlFKxD1BxTxmradmb4sLIKtxuJ3FlgKu583x/Se8HLoZRXc6GuEhurllp837TbtEqTVwN0bifRtgvOQWTNV2R1XBpK+WpYM71/NuEnGtcs6K1qYcy/Pdf6rFj9Sev2X/Vj4A/w==</Modulus><Exponent>AQAB</Exponent><P>760EHIjHjxVu4xvDRjKfGWMotAjzgHTvEvqDbZ8Z+pwNvA4FVlhAVAOoy9oULtxwDx1nbJZ7zvMl0LCA0qpZAShnp0k2hnD8Uqy5NCEFdrzW/YJCNAUXW22gjs4FRxQq7MeQCgOPf4ochDLJ5+LhYZhWcOqIQmO7hE0EawksxxU=</P><Q>uI/e63pxwTmOlTc5Iv+WMUSrwciDMnJ5qlSa7t7dhZDhsWylTFhhpxQLBvqNLniGqiABlMfiXuAJkrJj3FNtG0R2JBO04YcEhtUAmXbLkgBUGbQq5im5rbvg63Jrwjv493uuMJq5ipRJBA8iW/2NptdZ2lErrO3qjJmaAKHv7MM=</Q><DP>Gy0MBUxBDGlKvHeeaaw9u/wxVlCZg5w+q3E6S8i5gmAFcqhIVKUHDj+n+Q1OMo4OKriTzI03lpydUg9hXAGyu93f+ujjRNU01w78pPuFihQZcH8/kHRvVI+Wn/qeQJP51gU+wNPshnKbjyXPHPrtwUorqvv83UI/td2hvZ05tY0=</DP><DQ>KvRpKJsd4J/ZvIbWyHt95EecRgS57ELSaD86s/+wxQZcUOP9cNBdIfq7OkuUEk3A4dWDKLKA0B4KfFcCgOP1z9PWPz1K6vZ2qj7m3dDVPkzPRhA1r83kRjgk+AdwZwt4PXlOqEdKiaNLfyNalthjYIJcikA8DwpsfX6+ZglKgus=</DQ><InverseQ>wk1DQR9sbU5YM5GL8EYAQTqCdo+tMnJvMqGmxxy/zOwQsVlEH1zLIKEBFgMkcsH92QZVaZHddLMyr3MGkaPlj3MPbgUwYJdYsgBv/5JfrkWhmxXkQ/OP6gNHi2KYkzTqpEti1S3oGahOVJa14xf3Pir2nmjf7ZE4QLdGpIcrNQM=</InverseQ><D>WY54HADHwjM+8HNo7/3tneXm8wO2Vp720iOPOFpRNEI3OS1ggMbSM0n5BhsXSFW/4nwDn2USKkqmOQnCCUXL19rx8K1CMYW+Hr+XrkutAJy+sa+ruX9TNtKGhSNrVbv5AaemjWxgrolZ/CejZrUxyV0xdvMxLzrhjUBhQD3IINaAsi1u/oPRLVTrKgJA4N7nVWLXyPS5Ei+ceDIT7tOHA/qLsAZyrP4VD5b1j7udq7SV2onz/oqWAia8Xby18+hheKI0LjQoauiHvsN3oeUHGKu1JLII0XQgWBIcSrpaqz8oOnSHWQrt+TwePoLYwp3ziIphEaaqjdn1HMj7yTNeWQ==</D></RSAKeyValue>";

        private static readonly string ClientPublicKey = "<RSAKeyValue><Modulus>sY4gWmZIXW+wHq1rRRZruqBuWGWSFIZZwWkuV/Y4j5rshTamH5n282GGxMoxoaujw8PgSh81e1tgsAg7XdQWR+xd+pwW5srEWtA7BYG9HzFpixe25pvaTU2VlT21i3duAucR93Yp2RXpdapaXrbDG16IDZVk/tcI/Gso+GHnrd4SE8yrJvlis0cJfpYLiMx7mgl40u19lZHZj6jUbtnJF3zv9mQJRVPTj74dSaywGH7gPoyqCMXeZcfnfAil494szVJxQOC5CLDwT8UMiYGb1wgoBZgsHMNaGc+aS0Al3k5hahLM9ZzMiafTKHyVi3Fk3I6Oca8yOQ1z4cKanw4VDw==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        private static readonly string ClientPrivateKey = "<RSAKeyValue><Modulus>sY4gWmZIXW+wHq1rRRZruqBuWGWSFIZZwWkuV/Y4j5rshTamH5n282GGxMoxoaujw8PgSh81e1tgsAg7XdQWR+xd+pwW5srEWtA7BYG9HzFpixe25pvaTU2VlT21i3duAucR93Yp2RXpdapaXrbDG16IDZVk/tcI/Gso+GHnrd4SE8yrJvlis0cJfpYLiMx7mgl40u19lZHZj6jUbtnJF3zv9mQJRVPTj74dSaywGH7gPoyqCMXeZcfnfAil494szVJxQOC5CLDwT8UMiYGb1wgoBZgsHMNaGc+aS0Al3k5hahLM9ZzMiafTKHyVi3Fk3I6Oca8yOQ1z4cKanw4VDw==</Modulus><Exponent>AQAB</Exponent><P>3j8uZVXRsNQvyyKu5X84z6KR4QeOlp4IBdaqaMEYsWDbQSzKMjMyuZoAQRZD/MD1uqeqF7mug2yjR9gAYyQqZpY6iwyKmn8c0C1z9Wssl4Rks/uZ30svjaxe+lOCM43mxuSpg9JT+hyCuMbfMoj3E1FRHNOrU2gcPlU9Ii7mou0=</P><Q>zIVcyosOOcBmcQHulmG3shEYRmx34/SlLnS0+h00P29oEByAezL0cjNzlfAlEPCeWGyOrhI0/mdr7rJ5L8OZJbyAKFmvzsUxhQ1BR6eIRAZddpO3B0bpKhIc0bRaPjs68cIPmoXciiIUZMafJ52WCRdY8W4C7zl+JrAyqr4cbGs=</Q><DP>BymCFUHuwAQWhjVX7Y1sB0T17oqPzcrJ92Yzl6yhXxX/+tm2qmd7+v87gt1003Bket9sW9dgCFw35tjPKmK/+w5cVJ7S2KfABfVxBzes+DUMCcDR9KA3qBkl9ms6hu7LwR0dWWIt0qGYlNvT1Y0UaUr1kjhADTANuPSGQ9O/GwE=</DP><DQ>An/qx2r1rI0Gc4EjimD9XsbItiujN30I+81fXOM4fUH+UpwWkerog/DPC9kYiuF1/fytrcD5NtbKwesaghPw7j4kjtmxxWQxm7+yuY55ouM1Bzr89LDQLrxz77g1lichLE1D5Y6XSHTsLOMklNZfPBM1+hVvIb9hJFXo+J9EN6s=</DQ><InverseQ>esNowcE5zUV2qkprWfM2Nx6jL9NwTuVNnHKdSAQbO03znr8W89I4RWRuhLHwb6jDlgBS/OnCCdGm7S+7h2JToY36OL1i1YVzjf3xVj/7PMwnpqDQhrvkB/y+T8mZWFNY0PwXrNPgbh+8zddj2DRRJ0BeL8TEkFrep7JErqQkFtw=</InverseQ><D>ITApx/v6ZsyrpPBRbraEKHw4Y/mIVqHOQSLB8NsrM1yH/VZ7ssh6qn9+S2Zn5IpKsBr635/5xTcNWZoQTreH+qWp42Atv7IDBd6KSbs4eI4p5j/mhjB3m5926FCCkLEgNRav+wtxQwyfeMkA31dWHNWMpM1Z9XCIU3ZMUDEstwjNnrjkq/OyyjnOCJU3dO7fRTpn4U9Y9r96TMtVmOlau+V/TWfCDnMwYFa1z3ji8E3PT6wiq2a8Wtik6mIN8GWhvYiDik0v98tepY8w/knDX/5tJCCqVQMUGfIOSfZnANY6LVmkE/Tbhnx3FaOTwoYRLOh3NYHXRv3Qt0oa/L6msQ==</D></RSAKeyValue>";

        [TestMethod]
        public void TestRSA()
        {
            using (var clientRSA = new RSACryptoServiceProvider(0))
            using (var serverRSA = new RSACryptoServiceProvider(0))
            {
                clientRSA.PersistKeyInCsp = false;
                clientRSA.FromXmlString(ClientPublicKey);

                serverRSA.PersistKeyInCsp = false;
                serverRSA.FromXmlString(ServerPrivateKey);

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

                byte[] encryptionData = clientRSA.Encrypt(dataToEncrypt, true);

                var ms = new MemoryStream();
                var wr = new BinaryWriter(ms);
                wr.Write((byte)0);
                wr.Write(epic);
                wr.Write(DateTime.UtcNow.AddMinutes(-5).Ticks);
                wr.Write(DateTime.UtcNow.AddHours(1).Ticks);
                wr.Write(nonce);
                wr.Write(encryptionData.Length);
                wr.Write(encryptionData);

                var signature = serverRSA.SignData(ms.ToArray(), new SHA1CryptoServiceProvider());
                wr.Write(signature);

                foreach (var b in ms.ToArray())
                {
                    Console.Write(b.ToString("X2") + " ");
                }
                Console.WriteLine();
            }
        }

        [TestMethod]
        public void TestMakeKey()
        {
            var key = new UtlsKeyPacket(ClientPublicKey, ServerPrivateKey, 0);

            Console.WriteLine(key.CompletePacket.Length);
            foreach (var b in key.CompletePacket)
            {
                Console.Write(b.ToString("X2") + " ");
            }
            Console.WriteLine();

            UtlsKeyPacket key2;

            if (!UtlsKeyPacket.TryValidate(ClientPrivateKey, ServerPublicKey, key.CompletePacket, out key2))
            {
                throw new Exception();
            }

            if (!key.IV.SequenceEqual(key2.IV))
                throw new Exception();

            if (!key.AESKey.SequenceEqual(key2.AESKey))
                throw new Exception();

            if (!key.MACKey.SequenceEqual(key2.MACKey))
                throw new Exception();


            var cipher = new UdpCipher(key2);
            byte[] cipherText = cipher.Encrypt(System.Text.Encoding.ASCII.GetBytes("Hello World"));

            Console.WriteLine("Encrypted Packet");
            Console.WriteLine(cipherText);
            foreach (var b in cipherText)
            {
                Console.Write(b.ToString("X2") + " ");
            }
            Console.WriteLine();

            var cipher2 = new UdpCipher(key);
            byte[] clearText = cipher2.Decrypt(cipherText);
            Console.WriteLine(System.Text.Encoding.ASCII.GetString(clearText));

        }




        [TestMethod]
        public void GenerateKeyPairRSA()
        {
            //Comment this out to generate another RSA key.
            //return;
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                Console.WriteLine("Public Key");
                Console.WriteLine(rsa.ToXmlString(false));
                Console.WriteLine("Private Key");
                Console.WriteLine(rsa.ToXmlString(true));
            }
        }


    }
}
