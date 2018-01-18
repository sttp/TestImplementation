using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Sttp.Transport
{
    public class UtlsReceiverState
    {
        public Queue<Guid> LastInstanceID = new Queue<Guid>();
        /// <summary>
        /// The number of seconds to allow the KeyPacket to drift before discarding it as too old.
        /// </summary>
        public int LatencyToleranceSec = 15;


    }

    public enum UtlsCipherMode : byte
    {
        AES_CTR_128 = 0,
        AES_CTR_192 = 1,
        AES_CTR_256 = 2,
    }

    public enum UtlsHMACMode : byte
    {
        HMAC_SHA256_32 = 0,
        HMAC_SHA256_64 = 1,
        HMAC_SHA256_96 = 2,
        HMAC_SHA256_128 = 3,
        HMAC_SHA256_256 = 4,
        HMAC_SHA384_384 = 5,
        HMAC_SHA512_512 = 6,
    }

    public class UtlsKeyPacket
    {
        public readonly Stopwatch ElapsedSinceParsing;

        public readonly Guid InstanceID;
        public readonly DateTime CreationTime;
        public readonly byte[] EncryptKeyHash;
        public readonly byte[] SignKeyHash;

        public readonly byte KeyID;
        public readonly short ExpireSeconds;
        public readonly int ValidSequence;
        public readonly UtlsCipherMode CipherMode;
        public readonly UtlsHMACMode HmacMode;
        public readonly byte[] IV;
        public readonly byte[] AESKey;
        public readonly byte[] MACKey;


        public readonly byte[] CompletePacket;
        /// <summary>
        /// Gets if there was a parsing error
        /// </summary>
        private bool m_isValid = false;

        private UtlsKeyPacket(string clientPrivate, string serverPublic, byte[] completePacket)
        {
            ElapsedSinceParsing = Stopwatch.StartNew();

            IV = new byte[16];
            AESKey = new byte[32];
            MACKey = new byte[64];
            CompletePacket = completePacket;

            var ms = new MemoryStream(completePacket);
            var rd = new BinaryReader(ms);

            if (rd.ReadByte() != 1)
                return;
            rd.ReadBytes(32); //Nonce
            int lenEncryptedBlock = rd.ReadInt16();
            byte[] encBlock = rd.ReadBytes(lenEncryptedBlock);

            int lenBeforeSig = (int)ms.Position;
            int lenSig = rd.ReadInt16();
            byte[] sig = rd.ReadBytes(lenSig);

            ms.Position = 0;
            byte[] rawDataBlock = rd.ReadBytes(lenBeforeSig);

            using (var clientRSA = new RSACryptoServiceProvider(0))
            using (var serverRSA = new RSACryptoServiceProvider(0))
            {
                clientRSA.PersistKeyInCsp = false;
                clientRSA.FromXmlString(clientPrivate);

                serverRSA.PersistKeyInCsp = false;
                serverRSA.FromXmlString(serverPublic);

                if (!serverRSA.VerifyData(rawDataBlock, new SHA512CryptoServiceProvider(), sig))
                {
                    return;
                }

                byte[] rawData = clientRSA.Decrypt(encBlock, true); //RSA with OAEP padding

                Array.Copy(rawData, 32, IV, 0, 16);
                Array.Copy(rawData, 32 + 16, AESKey, 0, 32);
                Array.Copy(rawData, 32 + 16 + 32, MACKey, 0, 64);
            }

            m_isValid = true;
        }

        public UtlsKeyPacket(string clientPublic, string serverPrivate, int epicID)
        {
            IV = new byte[16];
            AESKey = new byte[32];
            MACKey = new byte[64];

            using (var clientRSA = new RSACryptoServiceProvider(0))
            using (var serverRSA = new RSACryptoServiceProvider(0))
            {
                clientRSA.PersistKeyInCsp = false;
                clientRSA.FromXmlString(clientPublic);

                serverRSA.PersistKeyInCsp = false;
                serverRSA.FromXmlString(serverPrivate);

                var rng = RandomNumberGenerator.Create();

                byte[] dataToEncrypt = new byte[32 + 16 + 32 + 32 + 32];
                byte[] nonce = new byte[32];

                rng.GetBytes(dataToEncrypt);
                rng.GetBytes(nonce);
                rng.GetBytes(IV);
                rng.GetBytes(AESKey);
                rng.GetBytes(MACKey);

                IV.CopyTo(dataToEncrypt, 32);
                AESKey.CopyTo(dataToEncrypt, 32 + 16);
                MACKey.CopyTo(dataToEncrypt, 32 + 16 + 32);

                byte[] encryptionData = clientRSA.Encrypt(dataToEncrypt, true); //RSA with OAEP padding

                var ms = new MemoryStream();
                var wr = new BinaryWriter(ms);
                wr.Write((byte)1);

                wr.Write(nonce);
                wr.Write((short)encryptionData.Length);
                wr.Write(encryptionData);

                var signature = serverRSA.SignData(ms.ToArray(), new SHA512CryptoServiceProvider());
                wr.Write((short)signature.Length);
                wr.Write(signature);

                CompletePacket = ms.ToArray();
            }
        }


        public static bool TryValidate(string clientPrivate, string serverPublic, byte[] completePacket, out UtlsKeyPacket keyData)
        {
            var item = new UtlsKeyPacket(clientPrivate, serverPublic, completePacket);
            if (item.m_isValid)
            {
                keyData = item;
                return true;
            }
            keyData = null;
            return false;
        }
    }
}
