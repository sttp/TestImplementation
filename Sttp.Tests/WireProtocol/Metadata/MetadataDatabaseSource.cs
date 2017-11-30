using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Schema;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sttp.Codec;
using Sttp.Codec.Metadata;
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
        public void TestGetMetadataSchema()
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
            Assert.AreEqual(cmd.CommandCode, CommandCode.MetadataSchema);

            StringBuilder sb = new StringBuilder();
            cmd.MetadataSchema.GetFullOutputString("", sb);
            Console.WriteLine(sb);
        }

        [TestMethod]
        public void TestGetMetadata()
        {
            var db = Load();

            Queue<byte[]> packets = new Queue<byte[]>();

            var writer = new WireEncoder();
            var reader = new WireDecoder();

            writer.NewPacket += (bytes, start, length) => packets.Enqueue(Clone(bytes, start, length));

            var statements = new List<SttpQueryStatement>();

            //statements.Add(BuildRequest("Vendor", "ID", "Acronym", "Name"));

            statements.Add(BuildRequest("Measurement", db["Measurement"].Columns.Select(x => x.Name).ToArray()));

            var raw = new List<SttpQueryRaw>();
            writer.GetMetadata(Guid.Empty, 0, false, statements, raw);

            while (packets.Count > 0)
            {
                var data = packets.Dequeue();
                reader.WriteData(data, 0, data.Length);
            }

            CommandObjects cmd = reader.NextCommand();

            StringBuilder sb = new StringBuilder();
            cmd.GetMetadata.GetFullOutputString("", sb);
            Console.WriteLine(sb);

            Assert.AreEqual(cmd.CommandCode, CommandCode.GetMetadata);

            db.ProcessCommand(cmd.GetMetadata, writer);

            while (packets.Count > 0)
            {
                var data = packets.Dequeue();
                reader.WriteData(data, 0, data.Length);
            }

            cmd = reader.NextCommand();
            Assert.AreEqual(cmd.CommandCode, CommandCode.Metadata);

            MetadataQueryTable tbl = null;
            MetadataSubCommandObjects subCmd;
            while ((subCmd = cmd.Metadata.NextCommand()) != null)
            {
                switch (subCmd.SubCommand)
                {
                    case MetadataSubCommand.DefineResponse:
                        tbl = new MetadataQueryTable(subCmd.DefineResponse);
                        break;
                    case MetadataSubCommand.DefineRow:
                        tbl.ProcessCommand(subCmd.DefineRow);
                        break;
                    case MetadataSubCommand.UndefineRow:
                        tbl.ProcessCommand(subCmd.UndefineRow);
                        break;
                    case MetadataSubCommand.Finished:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            var t = tbl.ToTable();
            MakeCSV(t);
        }

        private static SttpQueryStatement BuildRequest(string tableName, params string[] columns)
        {
            var s = new SttpQueryStatement();
            s.DirectTable = tableName;

            for (int x = 0; x < columns.Length; x++)
            {
                s.ColumnInputs.Add(new SttpQueryColumn(-1, columns[x], x));
                s.Outputs.Add(new SttpOutputColumns(x, columns[x]));
            }
            return s;
        }

        private static void MakeCSV(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().
                                                 Select(column => column.ColumnName);
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in dt.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => string.Concat("\"", field.ToString().Replace("\"", "\"\""), "\""));
                sb.AppendLine(string.Join(",", fields));
            }

            File.WriteAllText("c:\\temp\\openPDC-sttp.csv", sb.ToString());
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
