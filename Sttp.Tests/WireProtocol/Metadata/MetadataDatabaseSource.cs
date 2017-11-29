using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sttp.Codec;
using Sttp.Data;

namespace Sttp.Tests
{
    [TestClass]
    public class MetadataDatabaseSourceTest
    {
        [TestMethod]
        public void TestLoadFromDataset()
        {
            Load();
        }


        [TestMethod]
        public void TestQuery()
        {
            var db = Load();

            Queue<byte[]> packets = new Queue<byte[]>();

            var writer = new WireEncoder();
            var reader = new WireDecoder();

            writer.NewPacket += (bytes, start, length) => packets.Enqueue(Clone(bytes, start, length));

            writer.GetMetadataSchema(Guid.Empty, 0);

            while (packets.Count > 0)
            {
                var data = packets.Dequeue();
                reader.WriteData(data, 0, data.Length);
            }

            CommandObjects cmd = reader.NextCommand();
            Assert.AreEqual(cmd.CommandCode, CommandCode.GetMetadataSchema);
            Assert.AreEqual(cmd.GetMetadataSchema.SchemaVersion, Guid.Empty);
            Assert.AreEqual(cmd.GetMetadataSchema.Revision, 0L);

            db.ProcessCommand(cmd.GetMetadataSchema, writer);

            while (packets.Count > 0)
            {
                var data = packets.Dequeue();
                reader.WriteData(data, 0, data.Length);
            }
            cmd = reader.NextCommand();



        }


        private MetadataDatabaseSource Load()
        {
            DataSet ds = new DataSet("openPDC");

            using (var fs = new FileStream("c:\\temp\\openPDC-sttp.xml", FileMode.Open))
            {
                ds.ReadXml(fs);
            }

            return new MetadataDatabaseSource(ds);
        }

        private byte[] Clone(byte[] data, int pos, int length)
        {
            byte[] rv = new byte[length];
            Array.Copy(data, pos, rv, 0, length);
            return rv;
        }

    }
}
