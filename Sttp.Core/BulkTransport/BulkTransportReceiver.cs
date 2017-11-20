﻿//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.IO.Compression;
//using Sttp.WireProtocol;

//namespace Sttp.Core
//{
//    public class BulkTransportReceiver
//    {

//        #region Temporary for testing
//        public event EventHandler<EventArgs<Guid>> OnStreamFinished;

//        private Dictionary<Guid, Tuple<CmdBeginSend, Stream>> _activeStreams = new Dictionary<Guid, Tuple<CmdBeginSend, Stream>>();

//        public Dictionary<Guid, Tuple<CmdBeginSend, Stream>> ActiveStreams => _activeStreams;

//        #endregion Temporary for testing

//        public BulkTransportReceiver()
//        {
//        }

//        public void Process(Decoder decoder)
//        {
//            Cmd command;
//            while ((command = decoder.NextCommand()) != null)
//            {
//                switch (command.SubCommand)
//                {
//                    case SubCommand.BeginSend:
//                    {
//                        var begin = command.BeginSend;
//                            var stream = new Tuple<CmdBeginSend, Stream>(begin, CreateOutputStream(begin.Id, begin.OriginalSize, ".sttp_rx"));
//                            lock (ActiveStreams)
//                            {
//                                ActiveStreams[begin.Id] = stream;
//                            }

//                            int size = AppendDataToStream(stream, begin.Content);
//                            stream.Item1.Content = new byte[0];

//                            if (size == begin.OriginalSize || begin.OriginalSize <= 1000)
//                            {
//                                stream.Item2.Seek(0, SeekOrigin.Begin);
//                                OnStreamFinished?.Invoke(this, new EventArgs<Guid>(stream.Item1.Id));
//                            }
//                        }
//                        break;
//                    case SubCommand.CancelSend:
//                        {
//                            var cancel = command.CancelSend;
//                            DisposeStream(cancel.Id);
//                        }
//                        break;
//                    case SubCommand.SendFragment:
//                        {
//                            var fragment = command.SendFragment;
//                            if (ActiveStreams.TryGetValue(fragment.Id, out Tuple<CmdBeginSend, Stream> stream))
//                            {
//                                int size = AppendDataToStream(stream, fragment.Content);
//                                if (fragment.BytesRemaining - size <= 0)
//                                {
//                                    stream.Item2.Seek(0, SeekOrigin.Begin);
//                                    OnStreamFinished?.Invoke(this, new EventArgs<Guid>(stream.Item1.Id));
//                                }
//                            }
//                            else
//                            {
//                                throw new ArgumentOutOfRangeException(nameof(decoder), fragment.Id, "Stream with id not found");
//                            }
//                        }
//                        break;
//                    case SubCommand.Invalid:
//                    default:
//                        throw new ArgumentOutOfRangeException();
//                }
//            }
//        }

//        public void DisposeStream(Guid id)
//        {

//            if (ActiveStreams.TryGetValue(id, out Tuple<CmdBeginSend, Stream> stream))
//            {
//                lock (ActiveStreams)
//                {
//                    if (_activeStreams.ContainsKey(id))
//                        _activeStreams.Remove(id);
//                    else
//                        return;
//                    ;
//                }

//                if (stream.Item2 == null)
//                    return;

//                string fileName = null;
//                if (stream.Item2 is FileStream fs)
//                {
//                    fileName = fs.Name;
//                }

//                stream?.Item2?.Close();
//                stream?.Item2?.Dispose();

//                if (fileName != null && File.Exists(fileName))
//                {
//                    File.Delete(fileName);
//                }
//            }
//        }


//        private static int AppendDataToStream(Tuple<CmdBeginSend, Stream> stream, byte[] content)
//        {
//            switch (stream.Item1.Compression)
//            {
//                case BulkTransportCompression.None:
//                case BulkTransportCompression.GZipStream: // for stream, we fill up our stream before decompressing
//                    stream.Item2.Write(content, 0, content.Length);
//                    return content.Length;
//                case BulkTransportCompression.GZipPacket:
//                    {
//                        byte[] decompressedData = new byte[1000];
//                        using (MemoryStream ms = new MemoryStream(content, false))
//                        using (GZipStream decompress = new GZipStream(ms, CompressionMode.Decompress, false))
//                        {
//                            int size = decompress.Read(decompressedData, 0, 1000);
//                            stream.Item2.Write(decompressedData, 0, size);
//                            return size;
//                        }
//                    }
//                default:
//                    throw new ArgumentOutOfRangeException();
//            }

//        }

//        internal static Stream CreateOutputStream(Guid id, long size, string ext)
//        {
//            if (size >= 1024 * 1024) // 1 MB
//            {
//                // save to temp file on disk
//                var tempPath = Path.Combine(Path.GetTempPath(), "STTP");
//                if (!Directory.Exists(tempPath))
//                {
//                    Directory.CreateDirectory(tempPath);
//                }

//                return new FileStream(Path.Combine(tempPath, id.ToString() + ext), FileMode.CreateNew, FileAccess.ReadWrite);
//            }

//            return new MemoryStream();
//        }
//    }
//}