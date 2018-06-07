using System;
using System.IO;

namespace CTP.Net
{
    public class CtpStream : Stream
    {
        public CtpCommandStream CommandStream;
        public int StreamID { get; private set; }

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

        public override bool CanRead => true;
        public override bool CanSeek => true;
        public override bool CanWrite => true;
        public override long Length => throw new NotSupportedException();

        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

    }
}