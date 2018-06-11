using System;
using System.IO;

namespace CTP.Net
{
    public class CtpWriteStream : IDisposable
    {
        private ulong m_channelNumber;
        private CtpSession m_session;
        private Action<ulong, byte[], int, int> m_write;

        public CtpWriteStream(ulong channelNumber, CtpSession session, Action<ulong, byte[], int, int> write)
        {
            m_channelNumber = channelNumber;
            m_session = session;
            m_write = write;
        }

        public void Write(byte[] data, int offset, int length)
        {
            m_write?.Invoke(m_channelNumber, data, offset, length);
        }

        public void Dispose()
        {
            m_session.StopWriteStream(m_channelNumber);
            m_session = null;
            m_write = null;
        }
    }

    public class CtpReadStream : IDisposable
    {
        private ulong m_sessionID;
        private CtpSession m_session;

        public CtpReadStream(ulong sessionID, CtpSession session)
        {
            m_sessionID = sessionID;
            m_session = session;
        }

        internal void WriteToBuffer(byte[] data, int offset, int length)
        {

        }

        public int Read(byte[] data, int offset, int length)
        {
            return 0;
        }

        public void Dispose()
        {
            m_session.StopReadStream(m_sessionID);
            m_session = null;
        }
    }

    public class CtpStream : Stream
    {
        private CtpReadStream m_read;
        private CtpWriteStream m_write;

        public CtpCommandStream CommandStream;

        public int StreamID { get; private set; }

        public CtpStream(CtpReadStream read, CtpWriteStream write)
        {
            m_read = read;
            m_write = write;
        }

        public override void Flush()
        {

        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return 0;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {

        }

        public override bool CanRead => m_read != null;
        public override bool CanSeek => false;
        public override bool CanWrite => m_write != null;
        public override long Length => throw new NotSupportedException();

        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

    }
}