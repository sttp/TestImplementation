using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using Sttp.IO;
using Sttp.WireProtocol.BulkTransportPacket;

namespace Sttp.WireProtocol
{
    /// <summary>
    /// Responsible for encoding each command into bytes. One instance should be created per Request.
    /// </summary>
    public class BulkTransportEncoder
    {
        private StreamWriter m_stream;
        private Action<byte[], int, int> m_sendPacket;

        public BulkTransportEncoder(Action<byte[], int, int> sendPacket)
        {
            m_stream = new StreamWriter(1500);
            m_sendPacket = sendPacket;
        }

        /// <summary>
        /// <remarks>
        /// this might belong in a higher level API, included here for functional tests
        /// </remarks>
        /// </summary>
        public Task<BulkTransportStreamTracking> SendRaw(byte[] content, BulkTransportMode mode, bool gzip, IProgress<long> progress = null, CancellationToken cancel = default(CancellationToken))
        {
            var streamTracker = new BulkTransportStreamTracking
            {
                Id = Guid.NewGuid(),
                IsGZip = gzip,
                Mode = mode,
                OriginalSize = content.LongLength,
                Position = 0,
                BaseStreamOffset = 0
            };

            if (gzip)
            {
                throw new NotImplementedException();
            }

            var task = new Task<BulkTransportStreamTracking>(t =>
            {
                var tracker = t as BulkTransportStreamTracking;

                var bytesToRead = (ushort)Math.Min(tracker.Remaining, 1000);
                if (bytesToRead > 0)
                {
                    if (cancel.IsCancellationRequested)
                    {
                        CancelCommand(tracker.Id);
                        throw new OperationCanceledException(cancel);
                    }

                    SendBegin(tracker.Id, tracker.OriginalSize, tracker.Mode, tracker.IsGZip, content, tracker.Position, bytesToRead);
                    tracker.Position += bytesToRead;
                    progress?.Report(tracker.Position);
                }
                else
                {
                    SendBegin(tracker.Id, 0, tracker.Mode, tracker.IsGZip, new byte[0], 0, 0);
                    progress?.Report(tracker.Position);
                }

                while ((bytesToRead = (ushort)Math.Min(tracker.Remaining, 1000)) > 0)
                {
                    if (cancel.IsCancellationRequested)
                    {
                        CancelCommand(tracker.Id);
                        throw new OperationCanceledException(cancel);
                    }

                    SendFragment(tracker.Id, tracker.Offset, content, tracker.Position, bytesToRead);
                    tracker.Position += bytesToRead;
                    progress?.Report(tracker.Position);
                }
                return tracker;
            }, streamTracker);

            task.Start();

            return task;
        }

        /// <summary>
        /// <remarks>
        /// this might belong in a higher level API, included here for functional tests
        /// </remarks>
        /// </summary>
        public Task<BulkTransportStreamTracking> SendStream(System.IO.Stream stream, BulkTransportMode mode, bool gzip, IProgress<long> progress = null, CancellationToken cancel = default(CancellationToken))
        {

            var streamTracker = new BulkTransportStreamTracking
            {
                Id = Guid.NewGuid(),
                IsGZip = gzip,
                Mode = mode,
                OriginalSize = stream.Length - stream.Position,
                Position = stream.Position,
                BaseStreamOffset = stream.Position
            };

            if (gzip)
            {
                throw new NotImplementedException();
            }

            var task = new Task<BulkTransportStreamTracking>(t =>
            {
                var tracker = t as BulkTransportStreamTracking;

                var bytesToRead = (ushort)Math.Min(tracker.Remaining, 1000);
                if (bytesToRead > 0)
                {
                    if (cancel.IsCancellationRequested)
                    {
                        CancelCommand(tracker.Id);
                        throw new OperationCanceledException(cancel);
                    }

                    SendBegin(tracker.Id, tracker.TotalSize, tracker.Mode, tracker.IsGZip, stream, tracker.Position, bytesToRead);
                    tracker.Position += bytesToRead;
                }
                else
                {
                    SendBegin(tracker.Id, 0, tracker.Mode, tracker.IsGZip, new byte[0], 0, 0);
                }

                while ((bytesToRead = (ushort)Math.Min(tracker.Remaining, 1000)) > 0)
                {
                    if (cancel.IsCancellationRequested)
                    {
                        CancelCommand(tracker.Id);
                        throw new OperationCanceledException(cancel);
                    }
                    
                    SendFragment(tracker.Id, tracker.Offset, stream, tracker.Position, bytesToRead);
                    tracker.Position += bytesToRead;
                }

                return tracker;
            }, streamTracker);

            task.Start();

            return task;
        }

        public void SendBegin(Guid id, long originalSize, BulkTransportMode mode, bool gzip, byte[] source, long position, int length)
        {
            m_stream.Clear();
            m_stream.Write(BulkTransportCommand.BeginBulkTransport);
            m_stream.Write(id);
            m_stream.Write(originalSize);
            m_stream.Write(mode);
            m_stream.Write(gzip);
            m_stream.Write(source, position, length);
            m_sendPacket(m_stream.ToArray(), 0, m_stream.Length);
        }

        public void SendBegin(Guid id, long originalSize, BulkTransportMode mode, bool gzip, System.IO.Stream source, long position, int length)
        {
            m_stream.Clear();
            m_stream.Write(BulkTransportCommand.BeginBulkTransport);
            m_stream.Write(id);
            m_stream.Write(originalSize);
            m_stream.Write(mode);
            m_stream.Write(gzip);
            m_stream.Write(source, position, length);
            m_sendPacket(m_stream.ToArray(), 0, m_stream.Length);
        }


        public void SendFragment(Guid id, long offset, byte[] content, long position, int length)
        {
            m_stream.Clear();
            m_stream.Write(BulkTransportCommand.SendFragment);
            m_stream.Write(id);
            m_stream.Write(offset);
            m_stream.Write(content, position, length);
            m_sendPacket(m_stream.ToArray(), 0, m_stream.Length);
        }

        public void SendFragment(Guid id, long offset, System.IO.Stream content, long position, int length)
        {
            m_stream.Clear();
            m_stream.Write(BulkTransportCommand.SendFragment);
            m_stream.Write(id);
            m_stream.Write(offset);
            m_stream.Write(content, position, length);
            m_sendPacket(m_stream.ToArray(), 0, m_stream.Length);
        }

        public void CancelCommand(Guid id)
        {
            m_stream.Clear();
            m_stream.Write(BulkTransportCommand.CancelBulkTransport);
            m_stream.Write(id);
            m_sendPacket(m_stream.ToArray(), 0, m_stream.Length);
        }
    }

    public class BulkTransportStreamTracking : BulkTransportBeginParams
    {
        public long Position;
        public long BaseStreamOffset;
        public long Remaining => OriginalSize - Position;
        public long Offset => Position - BaseStreamOffset;
        public long TotalSize => OriginalSize - BaseStreamOffset;
         

        private new byte[] Content { get; set; } // hide this property
    }
}
