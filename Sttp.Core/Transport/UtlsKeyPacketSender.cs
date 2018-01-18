using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Sttp.Transport
{
    public class UtlsSenderState
    {
        public byte ActiveKeyID;
        public short DefaultExpireSeconds = 300;
        public string ReceiverPublicKeyXml;
        public string SenderPrivateKeyXml;
        public byte[] EncryptKeyHash;
        public byte[] SignKeyHash;

        public UtlsCipherMode ActiveCipherMode;
        public UtlsHMACMode ActiveHmacMode;
        public UtlsKeyPacketSender[] ActiveKeyIDs = new UtlsKeyPacketSender[250];

    }

    public class UtlsKeyPacketSender
    {
        private readonly UtlsSenderState m_state;

        public readonly byte KeyID;
        public short ExpireSeconds;
        public int ValidSequence;
        public readonly UtlsCipherMode CipherMode1;
        public readonly UtlsHMACMode HmacMode;

        public readonly byte[] IV;
        public readonly byte[] Key;
        public readonly byte[] HMACKey;

        private byte[] m_buffer;
        private byte[] m_secretData;
        private byte[] m_rngBytes = new byte[48];
        private int m_hmacLength;

        private HMAC m_hmac;
        private Aes m_aes;
        private ICryptoTransform m_encrypt;

        private RSACryptoServiceProvider m_receiverRSA;
        private RSACryptoServiceProvider m_senderRSA;
        private RandomNumberGenerator m_rng = RandomNumberGenerator.Create();
        private SHA512 m_signRSAhash = SHA512.Create();

        public UtlsKeyPacketSender(UtlsSenderState state)
        {
            m_state = state;
            if (state.ActiveKeyID == 249)
            {
                KeyID = 0;
            }
            else
            {
                KeyID = (byte)(state.ActiveKeyID + 1);
            }

            ExpireSeconds = state.DefaultExpireSeconds;
            ValidSequence = 0;
            CipherMode1 = state.ActiveCipherMode;
            HmacMode = state.ActiveHmacMode;
            IV = new byte[16];
            Key = new byte[32];
            HMACKey = new byte[128];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(IV);
                rng.GetBytes(Key);
                rng.GetBytes(HMACKey);
            }

            switch (HmacMode)
            {
                case UtlsHMACMode.HMAC_SHA256_32:
                case UtlsHMACMode.HMAC_SHA256_64:
                case UtlsHMACMode.HMAC_SHA256_96:
                case UtlsHMACMode.HMAC_SHA256_128:
                case UtlsHMACMode.HMAC_SHA256_256:
                    byte[] shortKey = new byte[64];
                    Array.Copy(HMACKey, 0, shortKey, 0, 64);
                    m_hmac = new HMACSHA384(shortKey);
                    break;
                case UtlsHMACMode.HMAC_SHA384_384:
                    m_hmac = new HMACSHA384(HMACKey);
                    break;
                case UtlsHMACMode.HMAC_SHA512_512:
                    m_hmac = new HMACSHA512(HMACKey);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (HmacMode)
            {
                case UtlsHMACMode.HMAC_SHA256_32:
                    m_hmacLength = 4;
                    break;
                case UtlsHMACMode.HMAC_SHA256_64:
                    m_hmacLength = 8;
                    break;
                case UtlsHMACMode.HMAC_SHA256_96:
                    m_hmacLength = 12;
                    break;
                case UtlsHMACMode.HMAC_SHA256_128:
                    m_hmacLength = 16;
                    break;
                case UtlsHMACMode.HMAC_SHA256_256:
                    m_hmacLength = 32;
                    break;
                case UtlsHMACMode.HMAC_SHA384_384:
                    m_hmacLength = 64;
                    break;
                case UtlsHMACMode.HMAC_SHA512_512:
                    m_hmacLength = 128;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            m_aes = Aes.Create();
            m_aes.Mode = CipherMode.ECB; //CTR is a special implementation of ECB
            m_aes.IV = new byte[16]; //IV is not used for CTR;
            m_aes.Padding = PaddingMode.None;

            switch (CipherMode1)
            {
                case UtlsCipherMode.AES_CTR_128:
                    {
                        byte[] shortKey = new byte[16];
                        Array.Copy(Key, 0, shortKey, 0, 16);
                        m_aes.Key = shortKey;
                        break;
                    }
                case UtlsCipherMode.AES_CTR_192:
                    {
                        byte[] shortKey = new byte[24];
                        Array.Copy(Key, 0, shortKey, 0, 24);
                        m_aes.Key = shortKey;
                        break;
                    }
                case UtlsCipherMode.AES_CTR_256:
                    {
                        byte[] shortKey = new byte[32];
                        Array.Copy(Key, 0, shortKey, 0, 32);
                        m_aes.Key = shortKey;
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }

            m_encrypt = m_aes.CreateEncryptor();


            m_receiverRSA = new RSACryptoServiceProvider(0);
            m_receiverRSA.PersistKeyInCsp = false;
            m_receiverRSA.FromXmlString(m_state.ReceiverPublicKeyXml);

            m_senderRSA = new RSACryptoServiceProvider(0);
            m_senderRSA.PersistKeyInCsp = false;
            m_senderRSA.FromXmlString(m_state.SenderPrivateKeyXml);

        }

        public byte[] CreateDataPacket(byte[] data)
        {
            if (ValidSequence >= 20_000_000)
            {
                throw new Exception("A key exchange should have happened by this point");
            }

            int blocks = (data.Length + 15 + m_hmacLength) >> 4;

            byte[] cipherKey = new byte[blocks * 16];
            for (int x = 0; x < blocks; x++)
            {
                Array.Copy(IV, 0, cipherKey, x * 16, 10);

                cipherKey[x * 16 + 10] = KeyID;
                cipherKey[x * 16 + 11] = (byte)(ValidSequence >> 16);
                cipherKey[x * 16 + 12] = (byte)(ValidSequence >> 8);
                cipherKey[x * 16 + 13] = (byte)(ValidSequence);
                cipherKey[x * 16 + 14] = (byte)(x >> 8);
                cipherKey[x * 16 + 15] = (byte)(x);
            }

            if (m_encrypt.TransformBlock(cipherKey, 0, cipherKey.Length, cipherKey, 0) != cipherKey.Length)
                throw new Exception("Error");

            byte[] packet = new byte[4 + m_hmacLength + data.Length];
            packet[0] = KeyID;
            packet[1] = (byte)(ValidSequence >> 16);
            packet[2] = (byte)(ValidSequence >> 8);
            packet[3] = (byte)(ValidSequence);
            Array.Copy(data, 0, packet, 4, data.Length);

            byte[] hash = m_hmac.ComputeHash(packet, 0, data.Length + 4);
            Array.Copy(hash, 0, packet, data.Length + 4, m_hmacLength);

            for (int x = 0; x < data.Length + m_hmacLength; x++)
            {
                packet[4 + x] ^= cipherKey[x];
            }

            ValidSequence++;

            return packet;
        }

        public byte[] CreateKeyPacket()
        {
            m_secretData = new byte[32 + 1 + 2 + 4 + 1 + 1 + 16 + 32 + 128];
            m_buffer = new byte[1500];

            m_rng.GetBytes(m_rngBytes);

            Array.Copy(m_rngBytes, 0, m_secretData, 0, 32);
            m_secretData[33] = KeyID;

            m_secretData[34] = (byte)(ExpireSeconds >> 8);
            m_secretData[35] = (byte)(ExpireSeconds);

            m_secretData[36] = (byte)(ValidSequence >> 24);
            m_secretData[37] = (byte)(ValidSequence >> 16);
            m_secretData[38] = (byte)(ValidSequence >> 8);
            m_secretData[39] = (byte)(ValidSequence);

            m_secretData[40] = (byte)(CipherMode1);
            m_secretData[41] = (byte)(HmacMode);

            IV.CopyTo(m_secretData, 42);
            Key.CopyTo(m_secretData, 58);
            HMACKey.CopyTo(m_secretData, 90);

            byte[] encryptionData = m_receiverRSA.Encrypt(m_secretData, true); //RSA with OAEP padding

            m_buffer[0] = 250;
            m_buffer[1] = 1;
            Array.Copy(m_rngBytes, 32, m_buffer, 2, 16);//InstanceID
            DateTime now = DateTime.UtcNow;
            m_buffer[18] = (byte)(now.Year >> 8);
            m_buffer[19] = (byte)(now.Year);
            m_buffer[20] = (byte)(now.Month);
            m_buffer[21] = (byte)(now.Day);
            m_buffer[22] = (byte)(now.Hour);
            m_buffer[23] = (byte)(now.Minute);
            m_buffer[24] = (byte)(now.Second);

            m_state.EncryptKeyHash.CopyTo(m_buffer, 25);
            m_state.SignKeyHash.CopyTo(m_buffer, 57);

            m_buffer[79] = (byte)(encryptionData.Length >> 8);
            m_buffer[80] = (byte)(encryptionData.Length);

            encryptionData.CopyTo(m_buffer, 81);

            var signature = m_senderRSA.SignData(m_buffer, 0, 81 + encryptionData.Length, m_signRSAhash);
            signature.CopyTo(m_buffer, 81 + encryptionData.Length);

            int returnLength = 81 + encryptionData.Length + signature.Length;
            byte[] rv = new byte[returnLength];
            Array.Copy(m_buffer, 0, rv, 0, returnLength);
            return rv;



        }

    }

}
