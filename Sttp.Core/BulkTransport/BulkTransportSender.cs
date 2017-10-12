using System;
using System.Collections.Generic;
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
    public class BulkTransportReceiver
    {

        #region Temporary for testing
        public event EventHandler<EventArgs<Guid>> OnStreamFinished;

        private Dictionary<Guid, Tuple<BulkTransportBeginParams, Stream>> _activeStreams = new Dictionary<Guid, Tuple<BulkTransportBeginParams, Stream>>();

        public Dictionary<Guid, Tuple<BulkTransportBeginParams, Stream>> ActiveStreams => _activeStreams;

        #endregion Temporary for testing

        public BulkTransportReceiver()
        {
        }

        public void Process(BulkTransportDecoder decoder)
        {
            IBulkTransportParams command;
            while ((command = decoder.Read()) != null)
            {
                switch (command.Command)
                {
                    case BulkTransportCommand.BeginBulkTransport:
                        {
                            var begin = (command as BulkTransportBeginParams);
                            var stream = new Tuple<BulkTransportBeginParams, Stream>(begin, CreateOutputStream(begin.Id, begin.OriginalSize));
                            lock (ActiveStreams)
                            {
                                ActiveStreams[begin.Id] = stream;
                            }

                            int size = AppendDataToStream(stream, begin.Content);
                            stream.Item1.Content = new byte[0];

                            if (size == begin.OriginalSize)
                            {
                                stream.Item2.Seek(0, SeekOrigin.Begin);
                                OnStreamFinished?.Invoke(this, new EventArgs<Guid>(stream.Item1.Id));
                            }
                        }
                        break;
                    case BulkTransportCommand.CancelBulkTransport:
                        {
                            var cancel = (command as BulkTransportSendFragmentParams);
                            DisposeStream(cancel.Id);
                        }
                        break;
                    case BulkTransportCommand.SendFragment:
                        {
                            var fragment = (command as BulkTransportSendFragmentParams);
                            if (ActiveStreams.TryGetValue(fragment.Id, out Tuple<BulkTransportBeginParams, Stream> stream))
                            {
                                int size = AppendDataToStream(stream, fragment.Content);
                                if (fragment.Offset + size == stream.Item1.OriginalSize)
                                {
                                    stream.Item2.Seek(0, SeekOrigin.Begin);
                                    OnStreamFinished?.Invoke(this, new EventArgs<Guid>(stream.Item1.Id));
                                }
                            }
                            else
                            {
                                throw new ArgumentOutOfRangeException(nameof(decoder), fragment.Id, "Stream with id not found");
                            }
                        }
                        break;
                    case BulkTransportCommand.Invalid:
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void DisposeStream(Guid id)
        {

            if (ActiveStreams.TryGetValue(id, out Tuple<BulkTransportBeginParams, Stream> stream))
            {
                lock (ActiveStreams)
                {
                    if (_activeStreams.ContainsKey(id))
                        _activeStreams.Remove(id);
                    else
                        return;
                }
                
                if (stream.Item2 == null)
                    return;

                string fileName = null;
                if (stream.Item2 is FileStream fs)
                {
                    fileName = fs.Name;
                }

                stream?.Item2?.Close();
                stream?.Item2?.Dispose();

                if (fileName != null && File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
            }
        }


        private static int AppendDataToStream(Tuple<BulkTransportBeginParams, Stream> stream, byte[] content)
        {
            if (stream.Item1.IsGZip)
            {
                byte[] decompressedData = new byte[1000];
                using (MemoryStream ms = new MemoryStream(content, false))
                using (GZipStream decompress = new GZipStream(ms, CompressionMode.Decompress, false))
                {
                    int size = decompress.Read(decompressedData, 0, 1000);
                    stream.Item2.Write(decompressedData, 0, size);
                    return size;
                }
            }
            else
            {
                stream.Item2.Write(content, 0, content.Length);
                return content.Length;
            }
        }

        private Stream CreateOutputStream(Guid id, long size)
        {
            if (size <= 1024 * 1024) // 1 MB
            {
                return new MemoryStream(new byte[size]);
            }
            else
            {
                // save to temp file on disk
                return new FileStream(Path.Combine(Path.GetTempPath(), "STTP", id.ToString()), FileMode.CreateNew, FileAccess.ReadWrite);
            }
        }

        private void CreateOutputStream(BulkTransportMode mode)
        {
            switch (mode)
            {
                case BulkTransportMode.DataPacket:
                    break;
                case BulkTransportMode.MetadataPacket:
                    // send to metadata processor
                    break;
                case BulkTransportMode.UserDefined:
                    break;
                case BulkTransportMode.Invalid:
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }
    }


    public class BulkTransportSender
    {
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

            var task = new Task<BulkTransportStreamTracking>(t =>
            {
                var tracker = t as BulkTransportStreamTracking;

                if (gzip)
                {
                    byte[] compressedRaw = new byte[1000];

                    using (var reader = new MemoryStream(content, false))
                    using (var compressedStream = new MemoryStream(compressedRaw))
                    using (GZipStream compressor = new GZipStream(compressedStream, CompressionMode.Compress))
                    {

                        byte[] buffer = new byte[64];
                        int count = 0;
                        while ((count = reader.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            compressor.Write(buffer, 0, count);

                            // if we are full, send and reset
                            int pos = (int)compressedStream.Position;
                            if (pos >= compressedRaw.Length - buffer.Length)
                            {
                                if (tracker.Position == 0)
                                {
                                    m_encoder.SendBegin(tracker.Id, tracker.OriginalSize, tracker.Mode, tracker.IsGZip, compressedRaw, 0, pos);
                                }
                                else
                                {
                                    m_encoder.SendFragment(tracker.Id, tracker.Offset, compressedRaw, 0, pos);
                                }

                                tracker.Position += pos;
                                progress?.Report(tracker.Position);
                                compressedStream.Position = 0; // reset
                            }
                        }
                    }
                }
                else
                {

                    var bytesToRead = (ushort)Math.Min(tracker.Remaining, 1000);
                    if (bytesToRead > 0)
                    {
                        if (cancel.IsCancellationRequested)
                        {
                            m_encoder.CancelCommand(tracker.Id);
                            throw new OperationCanceledException(cancel);
                        }

                        m_encoder.SendBegin(tracker.Id, tracker.OriginalSize, tracker.Mode, tracker.IsGZip, content, tracker.Position, bytesToRead);
                        tracker.Position += bytesToRead;
                        progress?.Report(tracker.Position);
                    }
                    else
                    {
                        m_encoder.SendBegin(tracker.Id, 0, tracker.Mode, tracker.IsGZip, new byte[0], 0, 0);
                        progress?.Report(tracker.Position);
                    }

                    while ((bytesToRead = (ushort)Math.Min(tracker.Remaining, 1000)) > 0)
                    {
                        if (cancel.IsCancellationRequested)
                        {
                            m_encoder.CancelCommand(tracker.Id);
                            throw new OperationCanceledException(cancel);
                        }

                        m_encoder.SendFragment(tracker.Id, tracker.Offset, content, tracker.Position, bytesToRead);
                        tracker.Position += bytesToRead;
                        progress?.Report(tracker.Position);
                    }
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
                        m_encoder.CancelCommand(tracker.Id);
                        throw new OperationCanceledException(cancel);
                    }

                    m_encoder.SendBegin(tracker.Id, tracker.TotalSize, tracker.Mode, tracker.IsGZip, stream, tracker.Position, bytesToRead);
                    tracker.Position += bytesToRead;
                }
                else
                {
                    m_encoder.SendBegin(tracker.Id, 0, tracker.Mode, tracker.IsGZip, new byte[0], 0, 0);
                }

                while ((bytesToRead = (ushort)Math.Min(tracker.Remaining, 1000)) > 0)
                {
                    if (cancel.IsCancellationRequested)
                    {
                        m_encoder.CancelCommand(tracker.Id);
                        throw new OperationCanceledException(cancel);
                    }

                    m_encoder.SendFragment(tracker.Id, tracker.Offset, stream, tracker.Position, bytesToRead);
                    tracker.Position += bytesToRead;
                }

                return tracker;
            }, streamTracker);

            task.Start();

            return task;
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
