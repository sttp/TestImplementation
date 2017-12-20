using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.Codec.CompressionLibraries
{
    //Looks like LZ4 might have 50% compression ratio for metadata, but is 6 times faster than DEFLATE at 70%,
    //but it appears the .NET native implementation is only 1.5 times slower
    //as apposed to the 6 times slower with DotNetZip.
    public static class LZ4
    {
        public static byte[] Compress(byte[] data)
        {
            int len = Compress(data, 0, data.Length, data, 0);
            return new byte[len];
        }

        //public static int Compress(byte[] source, int srcPos, int length, byte[] destination, int destPos)
        //{
        //    const int Table = 256 * 256;
        //    int savings = 0;

        //    for (int x = srcPos + 1; x < srcPos + length - 2; x++)
        //    {
        //        int lookBackTo = Math.Max(srcPos, x - 256);
        //        byte current = source[x];
        //        int maxFind = 0;
        //        for (int y = lookBackTo; y < x; y++)
        //        {
        //            if (source[y] == current)
        //            {
        //                int runLen = GetRunLength(source, y, x);
        //                maxFind = Math.Max(maxFind, runLen);
        //            }
        //        }

        //        if (maxFind > 2)
        //        {
        //            savings += maxFind - 2;
        //            x += maxFind;
        //        }
        //    }
        //    return length - savings;

        //}

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
