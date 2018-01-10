using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Sttp.Transport
{
    public class UdpCipher
    {
        //      It still need deduplication detection somewhere.



        private readonly UdpKeyExchange m_keyData;
        private long SequenceID = 0;
        private HMACSHA256 m_hmac;
        private AesCryptoServiceProvider m_aes;

        public UdpCipher(UdpKeyExchange keyData)
        {
            m_keyData = keyData;
            m_hmac = new HMACSHA256(keyData.MACKey);
            m_aes = new AesCryptoServiceProvider();
            m_aes.Key = m_keyData.AESKey;
            m_aes.IV = m_keyData.IV;
            m_aes.Mode = CipherMode.CBC;
            m_aes.Padding = PaddingMode.None;
        }

        public byte[] Encrypt(byte[] data)
        {
            byte[] padding = new byte[32];

            int hmacLen = ComputeHMACLen(data.Length);

            var ms = new MemoryStream();
            var wr = new BinaryWriter(ms);

            const int HMACStart = 1 + 4 + 8 + 2 + 1;
            const int CipherStart = 1 + 4 + 8 + 2;

            wr.Write((byte)0);
            wr.Write(m_keyData.EpicID);
            wr.Write(SequenceID++);
            wr.Write((short)(1 + hmacLen + data.Length));
            //The start of the cipher text
            wr.Write((byte)hmacLen);
            wr.Write(padding, 0, hmacLen);
            wr.Write(data);

            int cipherLength = (byte)(ms.Position - CipherStart);

            //Compute the HMAC
            byte[] hash = m_hmac.ComputeHash(ms.ToArray(), 0, (int)ms.Position);

            ms.Position = HMACStart;
            wr.Write(hash, 0, hmacLen);

            byte[] packet = ms.ToArray();
            using (var crypto = m_aes.CreateEncryptor())
            {
                if (crypto.TransformBlock(packet, CipherStart, cipherLength, packet, CipherStart) != cipherLength)
                {
                    throw new Exception("Error");
                }
            }

            return packet;
        }

        private int ComputeHMACLen(int dataLength)
        {
            //The encrypted data contains:
            // byte hMAC Length
            // byte[16..31] HMAC
            // byte[Data]

            //Therefore, determine how many bytes it takes to get to 16. If this number is less than 16, add 16 to it.

            int rv = 16 - (dataLength + 1) % 16;
            if (rv < 16)
                rv += 16;
            return rv;
        }

        public byte[] Decrypt(byte[] data)
        {
            data = (byte[])data.Clone(); //Since I'm going to be modifying the original data.
            byte[] hmac = new byte[32]; //I have to set this to 0's when computing the HMAC

            var ms = new MemoryStream(data);
            var rd = new BinaryReader(ms);

            const int HMACStart = 1 + 4 + 8 + 2 + 1;
            const int CipherStart = 1 + 4 + 8 + 2;

            if (rd.ReadByte() != 0)
                throw new Exception();
            if (rd.ReadInt32() != m_keyData.EpicID)
                throw new Exception();
            rd.ReadInt64(); //Do something with the sequence number
            int cipherLength = rd.ReadInt16();

            using (var crypto = m_aes.CreateDecryptor())
            {
                if (crypto.TransformBlock(data, CipherStart, cipherLength, data, CipherStart) != cipherLength)
                {
                    throw new Exception("Error");
                }
            }

            int hmacLen = rd.ReadByte();
            ms.Read(hmac, 0, hmacLen);

            Array.Clear(data, HMACStart, hmacLen);

            byte[] hash = m_hmac.ComputeHash(data, 0, CipherStart + cipherLength);

            for (int x = 0; x < hmacLen; x++)
            {
                if (hash[x] != hmac[x])
                    throw new Exception("HMAC Failed");
            }

            byte[] rv = new byte[cipherLength - 1 - hmacLen];
            Array.Copy(data, CipherStart + 1 + hmacLen, rv, 0, rv.Length);
            return rv;
        }

    }
}
