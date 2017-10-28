using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Sttp.IO;
using Sttp.WireProtocol;
using Sttp.WireProtocol.BulkTransportPacket;

namespace Sttp.Core
{
    public class BulkTransportSender
    {
        private const int MaximumPayloadSize = 1460;
        private const int MaximumReadSize = 1000;

        private BulkTransportEncoder m_encoder;

        public BulkTransportEncoder Encoder => m_encoder;

        public BulkTransportSender(BulkTransportEncoder encoder)
        {
            m_encoder = encoder;
        }

        /// <summary>
        /// <remarks>
        /// this might belong in a higher level API, included here for functional tests
        /// </remarks>
        /// </summary>
        public Task<BulkTransportStreamTracking> SendRaw(byte[] content, BulkTransportMode mode, BulkTransportCompression compression, IProgress<int> progress = null, CancellationToken cancel = default(CancellationToken))
        {
            Stream stream = new MemoryStream(content, false);
            var t = SendStream(stream, mode, compression, progress, cancel);
            t.ContinueWith(s => { try { stream?.Dispose(); } catch {/**/} }, cancel);
            return t;

        }

        internal static int CompressPacket(byte[] raw, byte[] compressed)
        {
            if (raw == null || raw.Length > 1024 * 1024)
                throw new ArgumentOutOfRangeException(nameof(raw), "Compression limited to 1MB");

            using (MemoryStream memory = new MemoryStream(compressed, true))
            {
                using (GZipStream gzip = new GZipStream(memory, CompressionMode.Compress, true))
                {
                    gzip.Write(raw, 0, raw.Length);
                }
                return (int)memory.Position;
            }
        }

        internal static Stream GetCompressedStream(Guid id, long size, Stream source)
        {
            var compressedStream = BulkTransportReceiver.CreateOutputStream(id, size, ".sttp_tx");
            int bufferSize = 4096;
            int bytesRead;
            byte[] buffer = new byte[bufferSize];
            using (var compressor = new GZipStream(compressedStream, CompressionMode.Compress, true))
            {
                while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
                {
                    compressor.Write(buffer, 0, bytesRead);
                }
            }

            compressedStream.Position = 0;
            return compressedStream;
        }

        /// <summary>
        /// <remarks>
        /// this might belong in a higher level API, included here for functional tests
        /// </remarks>
        /// </summary>
        public Task<BulkTransportStreamTracking> SendStream(System.IO.Stream stream, BulkTransportMode mode, BulkTransportCompression compression, IProgress<int> progress = null, CancellationToken cancel = default(CancellationToken))
        {
            var streamTracker = new BulkTransportStreamTracking
            {
                Id = Guid.NewGuid(),
                Compression = compression,
                Mode = mode,
                OriginalSize = stream.Length - stream.Position,
                Position = stream.Position,
                BaseStreamOffset = stream.Position,
                RemainingBytes = stream.Length - stream.Position
            };

            

            var task = new Task<BulkTransportStreamTracking>(t =>
            {
                var tracker = t as BulkTransportStreamTracking;

                if (compression == BulkTransportCompression.GZipStream)
                {
                    // compress the stream and use that as the source. 
                    // todo: this has a lot of potential overhead
                    stream = GetCompressedStream(streamTracker.Id, streamTracker.OriginalSize, stream);
                    streamTracker.Position = 0;
                    streamTracker.BaseStreamOffset = 0;
                    streamTracker.RemainingBytes = stream.Length;
                }

                bool gz = (tracker.Compression == BulkTransportCompression.GZipPacket);


                byte[] buffer = new byte[MaximumReadSize];
                byte[] compressed = new byte[MaximumReadSize * 2]; // allow some room
                int count = 0;
                int compressedSize = 0;

                while ((count = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    if (cancel.IsCancellationRequested)
                    {
                        m_encoder.CancelSend(tracker.Id);
                        throw new OperationCanceledException(cancel);
                    }
                    
                    if (gz)
                    {
                        compressedSize = CompressPacket(buffer, compressed);
                    }

                    byte[] payload = gz ? compressed : buffer;
                    int payloadSize = gz ? compressedSize : count;

                    if (tracker.Position == 0)
                    {
                        // first message
                        m_encoder.SendBegin(tracker.Id, tracker.Mode, tracker.Compression, tracker.OriginalSize, payload, 0, payloadSize);
                    }
                    else
                    {
                        m_encoder.SendFragment(tracker.Id, tracker.RemainingBytes, payload, 0, payloadSize);
                    }

                    tracker.Position += count;
                    tracker.RemainingBytes -= count;
                    progress?.Report((int)(100.0 * tracker.Position / tracker.OriginalSize));
                }

                if (stream is FileStream fs)
                {
                    string fileName = fs.Name;
                    fs.Close();
                    File.Delete(fileName);
                }

                return tracker;
            }, streamTracker);

            task.Start();

            return task;
        }
    }


    public class BulkTransportStreamTracking : BulkTransportBeginSendParams
    {
        public long Position;
        public long BaseStreamOffset;
        public long RemainingBytes;

        private new byte[] Content { get; set; } // hide this property
    }
}
