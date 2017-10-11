using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sttp.IO;
using Sttp.WireProtocol;
using Sttp.WireProtocol.BulkTransportPacket;
using MemoryStream = System.IO.MemoryStream;
namespace Sttp.Tests.WireProtocol.BulkTransport
{
    [TestClass]
    public class BulkTransportEncoderTests
    {
        private StreamReader m_sr;
        private BulkTransportDecoder m_decoder;
        private BulkTransportEncoder m_encoder;
        BulkTransportStreamTracking tracker = null;

        private byte[] receivedData = new byte[0];

        [TestInitialize]
        public void Init()
        {
            m_sr = new StreamReader(1500);
            m_decoder = new BulkTransportDecoder();

            void ReceivePacket(byte[] data, int position, int length)
            {
                m_sr.Clear();
                m_sr.Fill(data, position, length);
                var packet = m_decoder.Read(m_sr);

                switch (packet.Command)
                {
                    case BulkTransportCommand.BeginBulkTransport:
                        {
                            var begin = (packet as BulkTransportBeginParams);
                            Assert.IsNotNull(begin);
                            Assert.AreEqual(tracker.Id, begin.Id);
                            Assert.AreEqual(tracker.OriginalSize, begin.OriginalSize);
                            Assert.AreEqual(tracker.IsGZip, begin.IsGZip);
                            Assert.AreEqual(tracker.Mode, begin.Mode);
                            receivedData = new byte[begin.OriginalSize];
                            Array.Copy(begin.Content, 0, receivedData, 0, begin.Content.Length);
                        }
                        break;
                    case BulkTransportCommand.SendFragment:
                        {
                            var fragment = (packet as BulkTransportSendFragmentParams);
                            Assert.IsNotNull(fragment);
                            Assert.AreEqual(tracker.Id, fragment.Id);
                            Array.Copy(fragment.Content, 0, receivedData, fragment.Offset, fragment.Content.Length);
                        }
                        break;
                    case BulkTransportCommand.CancelBulkTransport:
                        var cancel = (packet as BulkTransportSendFragmentParams);
                        Assert.IsNotNull(cancel);
                        Assert.AreEqual(tracker.Id, cancel.Id);
                        break;
                    case BulkTransportCommand.Invalid:
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            m_encoder = new BulkTransportEncoder(ReceivePacket);
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

                var t = m_encoder.SendStream(s, BulkTransportMode.DataPacket, gzip);
                tracker = t.AsyncState as BulkTransportStreamTracking;

                t.Wait();
            }

            MetadataEncoderTests.CompareCollection(b, receivedData);

        }

        [TestMethod]
        public void TestRaw()
        {
            int size = 10000;
            var rand = new Random();
            var b = new byte[size];
            rand.NextBytes(b);

            var t = m_encoder.SendRaw(b, BulkTransportMode.DataPacket, false);
            tracker = t.AsyncState as BulkTransportStreamTracking;

            t.Wait();


            MetadataEncoderTests.CompareCollection(b, receivedData);

        }
    }
}
