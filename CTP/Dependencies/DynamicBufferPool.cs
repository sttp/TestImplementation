using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSF;
using GSF.Collections;
using GSF.Diagnostics;

namespace CTP.Collection
{
    public static class DynamicBufferPool
    {
        private static readonly ExpireObjectPool<byte[]>[] BufferPool;

        static DynamicBufferPool()
        {
            BufferPool = new ExpireObjectPool<byte[]>[17];

            //256 bytes to 16MB;
            for (int x = 0; x < 17; x++)
            {
                int bytes = 1 << (x + 8);
                using (Logger.AppendStackMessages("DynamicBufferPool", bytes.ToString()))
                {
                    BufferPool[x] = new ExpireObjectPool<byte[]>(() => new byte[bytes], 60);
                }
            }
        }

        public static byte[] Get(int size)
        {
            //ToDo: I can do bitmath manipulation
            if (size <= 1 << 8) return BufferPool[0].Dequeue();
            if (size <= 1 << 9) return BufferPool[1].Dequeue();
            if (size <= 1 << 10) return BufferPool[2].Dequeue();
            if (size <= 1 << 11) return BufferPool[3].Dequeue();
            if (size <= 1 << 12) return BufferPool[4].Dequeue();
            if (size <= 1 << 13) return BufferPool[5].Dequeue();
            if (size <= 1 << 14) return BufferPool[6].Dequeue();
            if (size <= 1 << 15) return BufferPool[7].Dequeue();
            if (size <= 1 << 16) return BufferPool[8].Dequeue();
            if (size <= 1 << 17) return BufferPool[9].Dequeue();
            if (size <= 1 << 18) return BufferPool[10].Dequeue();
            if (size <= 1 << 19) return BufferPool[11].Dequeue();
            if (size <= 1 << 20) return BufferPool[12].Dequeue();
            if (size <= 1 << 21) return BufferPool[13].Dequeue();
            if (size <= 1 << 22) return BufferPool[14].Dequeue();
            if (size <= 1 << 23) return BufferPool[15].Dequeue();
            if (size <= 1 << 24) return BufferPool[16].Dequeue();
            return new byte[size];
        }

        public static void Release(byte[] data)
        {
            int size = data?.Length ?? 0;
            if (size == 1 << 8) BufferPool[0].Enqueue(data);
            else if (size == 1 << 9) BufferPool[1].Enqueue(data);
            else if (size == 1 << 10) BufferPool[2].Enqueue(data);
            else if (size == 1 << 11) BufferPool[3].Enqueue(data);
            else if (size == 1 << 12) BufferPool[4].Enqueue(data);
            else if (size == 1 << 13) BufferPool[5].Enqueue(data);
            else if (size == 1 << 14) BufferPool[6].Enqueue(data);
            else if (size == 1 << 15) BufferPool[7].Enqueue(data);
            else if (size == 1 << 16) BufferPool[8].Enqueue(data);
            else if (size == 1 << 17) BufferPool[9].Enqueue(data);
            else if (size == 1 << 18) BufferPool[10].Enqueue(data);
            else if (size == 1 << 19) BufferPool[11].Enqueue(data);
            else if (size == 1 << 20) BufferPool[12].Enqueue(data);
            else if (size == 1 << 21) BufferPool[13].Enqueue(data);
            else if (size == 1 << 22) BufferPool[14].Enqueue(data);
            else if (size == 1 << 23) BufferPool[15].Enqueue(data);
            else if (size == 1 << 24) BufferPool[16].Enqueue(data);

        }




    }
}
