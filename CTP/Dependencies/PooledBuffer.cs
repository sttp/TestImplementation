using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GSF;
using GSF.Collections;
using GSF.Diagnostics;

namespace CTP.Collection
{
    public class PooledBuffer
    {
        private byte[] m_data;
        public readonly int Length;
        private int m_poolIndex;

        private PooledBuffer(int poolIndex, int length)
        {
            m_data = BufferPool[poolIndex].Dequeue();
            Length = length;
            m_poolIndex = poolIndex;
        }

        private PooledBuffer(byte[] bytes)
        {
            m_data = bytes;
            Length = bytes.Length;
            m_poolIndex = -1;
        }

        public void CopyTo(Stream stream)
        {
            stream.Write(m_data, 0, Length);
        }

        public IAsyncResult CopyToBeginWrite(Stream stream, AsyncCallback callback, object state)
        {
            return stream.BeginWrite(m_data, 0, Length, callback, state);
        }

        public void Release()
        {
            var item = Interlocked.Exchange(ref m_data, null);
            if (item != null)
            {
                if (m_poolIndex >= 0)
                {
                    BufferPool[m_poolIndex].Enqueue(item);
                }
            }
        }

        public void CopyTo(PooledBuffer newObj)
        {
            m_data.CopyTo(newObj.m_data, 0);
        }


        private static readonly ExpireObjectPool<byte[]>[] BufferPool;

        static PooledBuffer()
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

        public static PooledBuffer Create(byte[] data, int offset, int length)
        {
            data.ValidateParameters(offset, length);
            var obj = Get(length);
            Array.Copy(data, offset, obj.m_data, 0, length);
            return obj;
        }

        private static PooledBuffer Get(int size)
        {
            //ToDo: I can do bitmath manipulation
            if (size <= 1 << 8) return new PooledBuffer(0, size);
            if (size <= 1 << 9) return new PooledBuffer(1, size);
            if (size <= 1 << 10) return new PooledBuffer(2, size);
            if (size <= 1 << 11) return new PooledBuffer(3, size);
            if (size <= 1 << 12) return new PooledBuffer(4, size);
            if (size <= 1 << 13) return new PooledBuffer(5, size);
            if (size <= 1 << 14) return new PooledBuffer(6, size);
            if (size <= 1 << 15) return new PooledBuffer(7, size);
            if (size <= 1 << 16) return new PooledBuffer(8, size);
            if (size <= 1 << 17) return new PooledBuffer(9, size);
            if (size <= 1 << 18) return new PooledBuffer(10, size);
            if (size <= 1 << 19) return new PooledBuffer(11, size);
            if (size <= 1 << 20) return new PooledBuffer(12, size);
            if (size <= 1 << 21) return new PooledBuffer(13, size);
            if (size <= 1 << 22) return new PooledBuffer(14, size);
            if (size <= 1 << 23) return new PooledBuffer(15, size);
            if (size <= 1 << 24) return new PooledBuffer(16, size);
            return new PooledBuffer(new byte[size]);
        }

    }
}
