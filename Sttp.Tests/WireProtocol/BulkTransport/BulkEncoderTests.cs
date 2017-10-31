//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.IO;
//using System.IO.Compression;
//using System.Text;
//using System.Threading;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Sttp.Core;
//using Sttp.IO;
//using Sttp.WireProtocol;
//using Sttp.WireProtocol.BulkTransportPacket;
//using MemoryStream = System.IO.MemoryStream;

//namespace Sttp.Tests.WireProtocol.BulkTransport
//{
//    [TestClass]
//    public class BulkTransportEncoderTests
//    {
//        private WireDecoder m_wireDecoder;
//        private WireEncoder m_wireEncoder;

//        private AutoResetEvent finished;
//        private BulkTransportSender m_btx;
//        private BulkTransportReceiver m_brx;

//        BulkTransportStreamTracking tracker = null;

//        private byte[] receivedData = new byte[0];


//        [TestInitialize]
//        public void Init()
//        {
//            finished = new AutoResetEvent(false);
//            m_wireDecoder = new WireDecoder();

//            m_wireEncoder = new WireEncoder();
//            m_wireEncoder.NewPacket += Encoder_NewPacket;
//            m_btx = new BulkTransportSender(m_wireEncoder.BeginBulkTransferPacket());
//            m_brx = new BulkTransportReceiver();
//            m_brx.OnStreamFinished += BulkTransportReceiver_OnStreamFinished;
//        }

//        private void BulkTransportReceiver_OnStreamFinished(object sender, EventArgs<Guid> e)
//        {
//            var r = sender as BulkTransportReceiver;
//            Assert.IsNotNull(r);

//            var streamParams = r.ActiveStreams[e.Argument];

//            var stream = streamParams.Item2;
//            stream.Position = 0;

//            if (streamParams.Item1.Compression == BulkTransportCompression.GZipStream)
//            {
//                stream = new GZipStream(stream, CompressionMode.Decompress, true);
//            }

//            receivedData = new byte[(int)streamParams.Item1.OriginalSize];
//            stream.Read(receivedData, 0, (int)streamParams.Item1.OriginalSize);

//            stream?.Close();
//            r.DisposeStream(streamParams.Item1.Id);
//        }

//        private void Encoder_NewPacket(byte[] data, int position, int length)
//        {
//            m_wireDecoder.WriteData(data, position, length);

//            IPacketDecoder decoder;
//            while ((decoder = m_wireDecoder.NextPacket()) != null)
//            {
//                switch (decoder.CommandCode)
//                {
//                    case CommandCode.BulkTransport:
//                        m_brx.Process(decoder as BulkTransportDecoder);
//                        break;
//                    case CommandCode.NegotiateSession:
//                    case CommandCode.Subscribe:
//                    case CommandCode.SecureDataChannel:
//                    case CommandCode.RuntimeIDMapping:
//                    case CommandCode.DataPointPacket:
//                    case CommandCode.NoOp:
//                    case CommandCode.Invalid:
//                        throw new NotSupportedException();
//                }
//            }
//        }

//        [DataTestMethod]
//        [DataRow(10000, BulkTransportCompression.None)]
//        [DataRow(10000, BulkTransportCompression.GZipPacket)]
//        [DataRow(10000, BulkTransportCompression.GZipStream)]
//        [DataRow(1024 * 1024, BulkTransportCompression.None)]
//        [DataRow(1024 * 1024, BulkTransportCompression.GZipPacket)]
//        [DataRow(1024 * 1024, BulkTransportCompression.GZipStream)]
//        public void TestStream(int size, BulkTransportCompression compression)
//        {
//            var rand = new Random();
//            var b = new byte[size];
//            rand.NextBytes(b);

//            using (var s = new MemoryStream(b))
//            {
//                var t = m_btx.SendStream(s, BulkTransportMode.DataPacket, compression);
//                tracker = t.AsyncState as BulkTransportStreamTracking;

//                t.Wait();
//            }

//            MetadataEncoderTests.CompareCollection(b, receivedData);
//        }

//        [DataTestMethod]
//        [DataRow(10000, BulkTransportCompression.None)]
//        [DataRow(10000, BulkTransportCompression.GZipPacket)]
//        [DataRow(10000, BulkTransportCompression.GZipStream)]
//        [DataRow(1024 * 1024, BulkTransportCompression.None)]
//        [DataRow(1024 * 1024, BulkTransportCompression.GZipPacket)]
//        [DataRow(1024 * 1024, BulkTransportCompression.GZipStream)]
//        public void TestRaw(int size, BulkTransportCompression compression)
//        {
//            var rand = new Random();
//            var b = new byte[size];
//            rand.NextBytes(b);

//            var t = m_btx.SendRaw(b, BulkTransportMode.DataPacket, compression, new Progress<int>(p => Debug.WriteLine($"Progress: {p} %")));
//            tracker = t.AsyncState as BulkTransportStreamTracking;

//            t.Wait();

//            MetadataEncoderTests.CompareCollection(b, receivedData);
//        }
//    }
//}
