//using System;
//using System.IO;
//using System.Threading;

//namespace CTP.Net
//{
//    public class AsyncReading
//    {
//        public SessionToken Session;
//        public byte[] ReadBuffer;
//        public int ValidBytes => m_bytesRead;
//        private int m_bytesRead;
//        private int m_waitingByteCount;
//        private Action<AsyncReading> m_callback;
//        private IAsyncResult m_async;

//        public AsyncReading(SessionToken session)
//        {
//            Session = session;
//            m_bytesRead = 0;
//        }

//        public void Reset()
//        {
//            m_bytesRead = 0;
//        }

//        public void WaitForBytes(int byteCount, Action<AsyncReading> callback)
//        {
//            if (ReadBuffer == null)
//            {
//                ReadBuffer = new byte[byteCount];
//            }
//            else if (ReadBuffer.Length < byteCount)
//            {
//                byte[] newBuffer = new byte[byteCount];
//                ReadBuffer.CopyTo(newBuffer, 0);
//                ReadBuffer = newBuffer;
//            }

//            m_waitingByteCount = byteCount;
//            m_callback = callback;

//            if (byteCount <= m_bytesRead)
//            {
//                ThreadPool.QueueUserWorkItem(x => callback(this));
//            }
//            else
//            {
//                m_async = Session.FinalStream.BeginRead(ReadBuffer, m_bytesRead, m_waitingByteCount - m_bytesRead, Callback, null);
//            }
//        }

//        private void Callback(IAsyncResult ar)
//        {
//            int bytesRead = Session.FinalStream.EndRead(ar);
//            if (bytesRead == 0)
//                throw new EndOfStreamException();
//            m_bytesRead += bytesRead;
//            if (m_waitingByteCount < m_bytesRead)
//            {
//                m_async = Session.FinalStream.BeginRead(ReadBuffer, m_bytesRead, m_waitingByteCount - m_bytesRead, Callback, null);
//            }
//            else
//            {
//                m_callback(this);
//            }
//        }
//    }
//}