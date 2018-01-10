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
        //Note: This class doesn't work yet. but it's getting there.
        //      It still need deduplication detection.
        


        private readonly UdpKeyExchange m_keyData;
        private long SequenceID = 0;

        public UdpCipher(UdpKeyExchange keyData)
        {
            m_keyData = keyData;
        }

        public byte[] Encrypt(byte[] data)
        {
            byte[] padding = new byte[16];

            var ms = new MemoryStream();
            var wr = new BinaryWriter(ms);

            wr.Write((byte)0);
            wr.Write(m_keyData.EpicID);
            wr.Write(SequenceID++);

            int encryptedLength = (16 + data.Length + 1 + 15) / 16 * 16;
            wr.Write((short)encryptedLength);
            wr.Write(data);
            wr.Write(padding, 0, encryptedLength - data.Length - 16);
            wr.Write((byte)encryptedLength - data.Length - 16);

            var hash = new HMACSHA384(m_keyData.MACKey);
            wr.Write(hash.ComputeHash(ms.ToArray(), 0, (byte)ms.Position));

            using (var aes = new AesCryptoServiceProvider())
            {
                aes.Key = m_keyData.AESKey;
                aes.IV = m_keyData.IV; //XOR with the sequence number maybe?
                aes.Mode = CipherMode.CBC;
                using (var wr2 = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    wr2.Write(ms.ToArray(), 0, (byte)ms.Position);
                }
            }

            return ms.ToArray();
        }

        public byte[] Decrypt(byte[] data)
        {


            return null;
        }

    }
}
