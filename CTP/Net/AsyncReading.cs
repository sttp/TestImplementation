using System;
using System.IO;
using System.Threading;

namespace CTP.Net
{
    public class AsyncReading
    {
        private Stream m_stream;
        public byte[] ReadBuffer;
        public int BytesRead;
        private int m_waitingByteCount;
        private Action m_callback;
        private IAsyncResult m_async;

        public AsyncReading(Stream stream)
        {
            m_stream = stream;
            BytesRead = 0;
        }

        public void Reset(Stream stream)
        {
            m_stream = stream;
            BytesRead = 0;
        }

        public void Reset()
        {
            BytesRead = 0;
        }

        public void WaitForBytes(int byteCount, Action callback)
        {
            if (ReadBuffer == null)
            {
                ReadBuffer = new byte[byteCount];
            }
            else if (ReadBuffer.Length < byteCount)
            {
                byte[] newBuffer = new byte[byteCount];
                ReadBuffer.CopyTo(newBuffer, 0);
                ReadBuffer = newBuffer;
            }

            m_waitingByteCount = byteCount;
            m_callback = callback;

            if (byteCount <= BytesRead)
            {
                ThreadPool.QueueUserWorkItem(x => callback());
            }
            else
            {
                m_async = m_stream.BeginRead(ReadBuffer, BytesRead, m_waitingByteCount - BytesRead, Callback, null);
            }
        }

        private void Callback(IAsyncResult ar)
        {
            int bytesRead = m_stream.EndRead(ar);
            if (bytesRead == 0)
                throw new EndOfStreamException();
            BytesRead += bytesRead;
            if (m_waitingByteCount < BytesRead)
            {
                m_async = m_stream.BeginRead(ReadBuffer, BytesRead, m_waitingByteCount - BytesRead, Callback, null);
            }
            else
            {
                m_callback();
            }
        }
    }
}