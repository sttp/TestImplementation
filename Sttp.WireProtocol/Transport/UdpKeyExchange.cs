using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Sttp.Transport
{
    public class UdpKeyExchange
    {

        public readonly int EpicID;
        public readonly DateTime ValidAfter;
        public readonly DateTime ValidBefore;

        public readonly byte[] IV;
        public readonly byte[] AESKey;
        public readonly byte[] MACKey;

        public readonly byte[] CompletePacket;

        public bool IsValid = false;

        private UdpKeyExchange(string clientPrivate, string serverPublic, byte[] completePacket)
        {
            IV = new byte[16];
            AESKey = new byte[32];
            MACKey = new byte[64];
            CompletePacket = completePacket;

            var ms = new MemoryStream(completePacket);
            var rd = new BinaryReader(ms);

            if (rd.ReadByte() != 1)
                return;
            EpicID = rd.ReadInt32();
            ValidAfter = new DateTime(rd.ReadInt64());
            ValidBefore = new DateTime(rd.ReadInt64());
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

            IsValid = true;
        }

        public UdpKeyExchange(string clientPublic, string serverPrivate, int epicID)
        {
            IV = new byte[16];
            AESKey = new byte[32];
            MACKey = new byte[64];
            EpicID = epicID;
            ValidAfter = DateTime.UtcNow.AddMinutes(-5);
            ValidBefore = DateTime.UtcNow.AddHours(1);

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

                wr.Write(EpicID);
                wr.Write(ValidAfter.Ticks);
                wr.Write(ValidBefore.Ticks);

                wr.Write(nonce);
                wr.Write((short)encryptionData.Length);
                wr.Write(encryptionData);

                var signature = serverRSA.SignData(ms.ToArray(), new SHA512CryptoServiceProvider());
                wr.Write((short)signature.Length);
                wr.Write(signature);

                CompletePacket = ms.ToArray();
            }
        }


        public static bool TryValidate(string clientPrivate, string serverPublic, byte[] completePacket, out UdpKeyExchange keyData)
        {
            var item = new UdpKeyExchange(clientPrivate, serverPublic, completePacket);
            if (item.IsValid)
            {
                keyData = item;
                return true;
            }
            keyData = null;
            return false;
        }
    }
}
