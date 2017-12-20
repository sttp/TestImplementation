using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.Codec.CompressionLibraries
{
    public class StringCompression
    {
        private byte[] m_history = new byte[4096];
        private ushort[] m_hashTable = new ushort[256];
        private int m_position = 0;

        public void Compress(string value, ByteWriter wr)
        {
            byte[] data = Encoding.UTF8.GetBytes(value);
            data.CopyTo(m_history, m_position);

            for (int x = m_position; x < m_position + data.Length; x++)
            {
                //m_hashTable[m_history[x]];
            }
        }

        public static int Compress(byte[] source, int srcPos, int length, byte[] destination, int destPos)
        {
            const int Table = 256 * 256;
            int savings = 0;

            int[] hashTable = new int[Table];

            for (int x = srcPos; x < srcPos + length - 2; x++)
            {
                int indx = (source[x] + source[x + 1] * 256) & (Table - 1);

                if (hashTable[indx] > 0 && hashTable[indx] > x - 64 * 1024)
                {
                    int runLen = GetRunLength(source, hashTable[indx], x);
                    if (runLen > 4)
                    {
                        savings += runLen - 3;
                        //Yay, I found one.
                        x += runLen;
                    }
                }
                hashTable[indx] = x;
            }
            return length - savings;
        }

        private static int GetRunLength(byte[] source, int pos1, int pos2)
        {
            int x = 0;
            while (pos1 + x < source.Length && pos2 + x < source.Length && source[pos1 + x] == source[pos2 + x])
            {
                x++;
            }
            return x - 1;
        }
    }

}
