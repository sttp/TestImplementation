using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sttp.Core;
using Sttp.IO;
using Sttp.WireProtocol;
using Sttp.WireProtocol.BulkTransportPacket;
using MemoryStream = System.IO.MemoryStream;

namespace Sttp.Tests.WireProtocol.BulkTransport
{
    [TestClass]
    public class BulkTransportEncoderTests
    {
        private WireDecoder m_wireDecoder;
        private WireEncoder m_wireEncoder;
        

        private BulkTransportSender m_btx;
        private BulkTransportReceiver m_brx;

        BulkTransportStreamTracking tracker = null;

        private byte[] receivedData = new byte[0];


        [TestInitialize]
        public void Init()
        {
            m_wireDecoder = new WireDecoder();

            m_wireEncoder = new WireEncoder(1500);
            m_wireEncoder.NewPacket += Encoder_NewPacket;

            m_brx = new BulkTransportReceiver();
        }

        private void Encoder_NewPacket(byte[] data, int position, int length)
        {
            m_wireDecoder.WriteData(data, position, length);

            IPacketDecoder decoder;
            while ((decoder = m_wireDecoder.NextPacket()) != null)
            {
                switch (decoder.CommandCode)
                {
                    case CommandCode.BulkTransport:
                        m_brx.Process(decoder as BulkTransportDecoder);
                        break;
                    case CommandCode.NegotiateSession:
                    case CommandCode.Subscribe:
                    case CommandCode.SecureDataChannel:
                    case CommandCode.RuntimeIDMapping:
                    case CommandCode.DataPointPacket:
                    case CommandCode.Fragment:
                    case CommandCode.NoOp:
                    case CommandCode.Invalid:
                    case CommandCode.Metadata:
                        throw new NotSupportedException();
                }
            }
        }

        [DataTestMethod]
        [DataRow(10000, false)]
        [DataRow(10000, true)]
        public void TestStream(int size, bool gzip)
        {
            var rand = new Random();
            var b = new byte[size];
            rand.NextBytes(b);

            using (var s = new MemoryStream())
            {
                s.Write(b, 0, size);
                s.Position = 0;

                var t = m_btx.SendStream(s, BulkTransportMode.DataPacket, gzip);
                tracker = t.AsyncState as BulkTransportStreamTracking;

                t.Wait();
            }

            MetadataEncoderTests.CompareCollection(b, receivedData);

        }

        [DataTestMethod]
        [DataRow(10000, false)]
        [DataRow(10000, true)]
        public void TestRaw(int size, bool gzip)
        {
            var rand = new Random();
            var b = new byte[size];
            rand.NextBytes(b);

            var t = m_btx.SendRaw(b, BulkTransportMode.DataPacket, false);
            tracker = t.AsyncState as BulkTransportStreamTracking;

            t.Wait();

            MetadataEncoderTests.CompareCollection(b, receivedData);
        }
    }
}
