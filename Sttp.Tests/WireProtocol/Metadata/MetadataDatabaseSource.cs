using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using CTP;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sttp.Codec;
using Sttp.Core.Data;
using Sttp.Data;
using Sttp.Services;

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

            writer.GetMetadataSchema(Guid.Empty, null);

            while (packets.Count > 0)
            {
                var data = packets.Dequeue();
                reader.FillBuffer(data, 0, data.Length);
            }

            CommandObjects cmd = reader.NextCommand();
            Assert.AreEqual(cmd.CommandName, "GetMetadataSchema");
            Assert.AreEqual(cmd.GetMetadataSchema.LastKnownRuntimeID, Guid.Empty);
            Assert.AreEqual(cmd.GetMetadataSchema.LastKnownVersionNumber, (long?)null);

            db.ProcessCommand(cmd.GetMetadataSchema, writer);

            while (packets.Count > 0)
            {
                var data = packets.Dequeue();
                reader.FillBuffer(data, 0, data.Length);
            }

            cmd = reader.NextCommand();
            Assert.AreEqual(cmd.CommandName, "MetadataSchema");

            Console.WriteLine(cmd.ToXMLString());
        }

        [TestMethod]
        public void TestGetMetadata()
        {
            var db = Load();

            Queue<byte[]> packets = new Queue<byte[]>();

            var writer = new WireEncoder();
            var reader = new WireDecoder();

            writer.NewPacket += (bytes, start, length) => packets.Enqueue(Clone(bytes, start, length));

            //statements.Add(BuildRequest("Vendor", "ID", "Acronym", "Name"));
            var s = BuildRequest("Measurement", db["Measurement"].Columns.Select(x => x.Name).ToArray());
            var s2 = s.ToCtpDocument();
            Console.WriteLine(s2.EncodedSize);
            Console.WriteLine(s2.CompressedSize);
            //Console.WriteLine(s2.ToXML());

            writer.SendCustomCommand(s);

            while (packets.Count > 0)
            {
                var data = packets.Dequeue();
                reader.FillBuffer(data, 0, data.Length);
            }

            CommandObjects cmd = reader.NextCommand();
            //Console.WriteLine(cmd.ToXMLString());

            Assert.AreEqual(cmd.CommandName, "GetMetadata");

            db.ProcessCommand(cmd.GetMetadata, writer);

            while (packets.Count > 0)
            {
                var data = packets.Dequeue();
                reader.FillBuffer(data, 0, data.Length);
            }

            cmd = reader.NextCommand();
            Console.WriteLine(cmd.ToXMLString());

            Stopwatch sw = new Stopwatch();
            sw.Restart();
            Console.WriteLine(cmd.Document.EncodedSize);
            Console.WriteLine(sw.Elapsed.TotalMilliseconds);
            sw.Restart();
            Console.WriteLine(cmd.Document.CompressedSize);
            Console.WriteLine(sw.Elapsed.TotalMilliseconds);
            sw.Restart();

            Assert.AreEqual(cmd.CommandName, "BeginMetadataResponse");

            MetadataQueryTable tbl = new MetadataQueryTable(cmd.BeginMetadataResponse);
            var decoder = new MetadataRowDecoder(cmd.BeginMetadataResponse);
            CtpObject[] values = new CtpObject[cmd.BeginMetadataResponse.Columns.Count];
            for (int x = 0; x < values.Length; x++)
            {
                values[x] = new CtpObject();
            }
            while ((cmd = reader.NextCommand()) != null)
            {
                if (cmd.CommandCode == CommandCode.Raw)
                {
                    decoder.Load(cmd.Raw.Payload);
                    while (decoder.Read(values))
                    {
                        tbl.AddRow(values);
                    }
                }
                else if (cmd.CommandCode == CommandCode.DocumentCommand)
                {
                    break;
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
            var t = tbl.ToTable();
            MakeCSV(t);
        }


        //[TestMethod]
        //public void TestGetMetadataSimple()
        //{
        //    var db = Load();

        //    Queue<byte[]> packets = new Queue<byte[]>();

        //    var writer = new WireEncoder();
        //    var reader = new WireDecoder();

        //    writer.NewPacket += (bytes, start, length) => packets.Enqueue(Clone(bytes, start, length));

        //    var s = new CommandGetMetadataBasic(null, null, "Measurement", db["Measurement"].Columns.Select(x => x.Name));
        //    //statements.Add(BuildRequest("Vendor", "ID", "Acronym", "Name"));
        //    //var s = BuildRequest("Measurement", db["Measurement"].Columns.Select(x => x.Name).ToArray());
        //    var s2 = s.ToSttpMarkup();
        //    Console.WriteLine(s2.EncodedSize);
        //    Console.WriteLine(s2.CompressedSize);
        //    Console.WriteLine(s2.ToYAML());

        //    writer.SendCustomCommand(s);

        //    while (packets.Count > 0)
        //    {
        //        var data = packets.Dequeue();
        //        reader.FillBuffer(data, 0, data.Length);
        //    }

        //    CommandObjects cmd = reader.NextCommand();
        //    //Console.WriteLine(cmd.ToXMLString());

        //    Assert.AreEqual(cmd.CommandName, "GetMetadataBasic");

        //    db.ProcessCommand(cmd.GetMetadataBasic, writer);

        //    while (packets.Count > 0)
        //    {
        //        var data = packets.Dequeue();
        //        reader.FillBuffer(data, 0, data.Length);
        //    }

        //    cmd = reader.NextCommand();

        //    Stopwatch sw = new Stopwatch();
        //    sw.Restart();
        //    Console.WriteLine(cmd.Markup.EncodedSize);
        //    Console.WriteLine(sw.Elapsed.TotalMilliseconds);
        //    sw.Restart();
        //    Console.WriteLine(cmd.Markup.CompressedSize);
        //    Console.WriteLine(sw.Elapsed.TotalMilliseconds);
        //    sw.Restart();

        //    Assert.AreEqual(cmd.CommandName, "Metadata");

        //    MetadataQueryTable tbl = null;
        //    MetadataSubCommandObjects subCmd;
        //    while ((subCmd = cmd.Metadata.NextCommand()) != null)
        //    {
        //        switch (subCmd.SubCommand)
        //        {
        //            case MetadataSubCommand.DefineResponse:
        //                tbl = new MetadataQueryTable(subCmd.DefineResponse);
        //                break;
        //            case MetadataSubCommand.DefineRow:
        //                tbl.ProcessCommand(subCmd.DefineRow);
        //                break;
        //            case MetadataSubCommand.UndefineRow:
        //                tbl.ProcessCommand(subCmd.UndefineRow);
        //                break;
        //            case MetadataSubCommand.Finished:
        //                break;
        //            default:
        //                throw new ArgumentOutOfRangeException();
        //        }
        //    }

        //    var t = tbl.ToTable();
        //    MakeCSV(t);
        //}

        //[TestMethod]
        //public void TestGetMetadataJoin()
        //{
        //    var db = Load();

        //    Queue<byte[]> packets = new Queue<byte[]>();

        //    var writer = new WireEncoder();
        //    var reader = new WireDecoder();

        //    writer.NewPacket += (bytes, start, length) => packets.Enqueue(Clone(bytes, start, length));

        //    //statements.Add(BuildRequest("Vendor", "ID", "Acronym", "Name"));

        //    var s = BuildRequest("Measurement", db["Measurement"].Columns.Select(x => x.Name).ToArray());
        //    s.JoinedTables.Add(new SttpQueryJoinedTable(0, "DeviceID", "Device", 1));
        //    s.ColumnInputs.Add(new SttpQueryColumn(1, "Name", -1));
        //    s.Outputs.Add(new SttpQueryOutputColumns(-1, "DeviceName"));
        //    s.Literals.Add(new SttpQueryLiteral((SttpValue)327, -2));
        //    s.Procedure.Add(new SttpQueryProcedureStep("EQU", new int[] { 3, -2 }.ToList(), -3));
        //    s.WhereBooleanVariable = -3;
        //    s.ValidateAndRemapAllIndexes(out int j, out int j1);
        //    var s2 = s.ToSttpMarkup();
        //    Console.WriteLine(s2.EncodedSize);
        //    Console.WriteLine(s2.CompressedSize);
        //    Console.WriteLine(s2.ToYAML());

        //    writer.SendCustomCommand(s);

        //    while (packets.Count > 0)
        //    {
        //        var data = packets.Dequeue();
        //        reader.FillBuffer(data, 0, data.Length);
        //    }

        //    CommandObjects cmd = reader.NextCommand();

        //    Console.WriteLine(cmd.ToXMLString());

        //    Assert.AreEqual(cmd.CommandName, "GetMetadataAdvance");

        //    db.ProcessCommand(cmd.GetMetadataAdvance, writer);

        //    while (packets.Count > 0)
        //    {
        //        var data = packets.Dequeue();
        //        reader.FillBuffer(data, 0, data.Length);
        //    }

        //    cmd = reader.NextCommand();

        //    Console.WriteLine(cmd.Markup.ToYAML());

        //    Assert.AreEqual(cmd.CommandName, "Metadata");

        //    MetadataQueryTable tbl = null;
        //    MetadataSubCommandObjects subCmd;
        //    while ((subCmd = cmd.Metadata.NextCommand()) != null)
        //    {
        //        switch (subCmd.SubCommand)
        //        {
        //            case MetadataSubCommand.DefineResponse:
        //                tbl = new MetadataQueryTable(subCmd.DefineResponse);
        //                break;
        //            case MetadataSubCommand.DefineRow:
        //                tbl.ProcessCommand(subCmd.DefineRow);
        //                break;
        //            case MetadataSubCommand.UndefineRow:
        //                tbl.ProcessCommand(subCmd.UndefineRow);
        //                break;
        //            case MetadataSubCommand.Finished:
        //                break;
        //            default:
        //                throw new ArgumentOutOfRangeException();
        //        }
        //    }

        //    var t = tbl.ToTable();
        //    MakeCSV(t);
        //}

        private static CommandGetMetadata BuildRequest(string tableName, params string[] columns)
        {
            var s = new CommandGetMetadata(tableName, columns);
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


        private SttpMetadataServer Load()
        {
            DataSet ds = new DataSet("openPDC");

            using (var fs = new FileStream("c:\\temp\\openPDC-sttp.xml", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                ds.ReadXml(fs);
            }

            var db = new SttpMetadataServer();
            db.DefineSchema(ds);
            db.FillData(ds);
            db.CommitData();
            return db;
        }

        private byte[] Clone(byte[] data, int pos, int length)
        {
            byte[] rv = new byte[length];
            Array.Copy(data, pos, rv, 0, length);
            return rv;
        }

    }
}
