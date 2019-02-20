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
using Sttp.Services;

namespace Sttp.Tests
{
    [TestClass]
    public class MetadataDatabaseSourceTest
    {
        [TestMethod]
        public void BenchmarkBig()
        {
            var db = Load();
            var schema = db.GetMetadataSchema();
            Console.Write(schema.ToString());

            schema = (CommandMetadataSchema)schema.ToCommand();
            var sw = new Stopwatch();
            sw.Restart();
            sw.Restart();
            const int cnt = 10000;
            for (int x = 0; x < cnt; x++)
            {
                schema = (CommandMetadataSchema)schema.ToCommand();
            }
            Console.WriteLine(cnt / sw.Elapsed.TotalSeconds);
            Console.WriteLine(schema.ToCommand().DataLength);
            //File.WriteAllBytes("C:\\temp\\TestFile.bin", schema.ToDocument().ToArray());
        }
        [TestMethod]
        public void BenchmarkSmall()
        {
            var schema = new CommandLittle();
            var sw = new Stopwatch();
            sw.Restart();
            sw.Restart();
            const int cnt = 10000;
            for (int x = 0; x < cnt; x++)
            {
                schema = (CommandLittle)schema.ToCommand();
            }
            Console.WriteLine(cnt / sw.Elapsed.TotalSeconds);
        }

        [CommandName("GetMetadata")]
        public class CommandLittle
            : CommandObject<CommandLittle>
        {
            [CommandField()] public int Value1 { get; private set; }
            [CommandField()] public int Value2 { get; private set; }
            [CommandField()] public int Value3 { get; private set; }
            [CommandField()] public int Value4 { get; private set; }
            [CommandField()] public int Value5 { get; private set; }

            //Exists to support CtpSerializable
            public CommandLittle()
            {

            }

            public static explicit operator CommandLittle(CtpCommand obj)
            {
                return FromCommand(obj);
            }

        }

        [TestMethod]
        public void TestLoadFromDataset()
        {
            Load();
        }


        //[TestMethod]
        //public void TestGetMetadataSchema()
        //{
        //    var db = Load();

        //    Queue<byte[]> packets = new Queue<byte[]>();

        //    var ms = new MemoryStream();
        //    var codec = new WireCodec(ms);
        //    var writer = new CtpEncoder();
        //    var reader = new CtpDecoder();

        //    writer.NewPacket += (bytes, start, length) => packets.Enqueue(Clone(bytes, start, length));
        //    writer.Send(0, new CommandGetMetadataSchema(Guid.Empty, null));

        //    while (packets.Count > 0)
        //    {
        //        var data = packets.Dequeue();
        //        reader.FillBuffer(data, 0, data.Length);
        //    }


        //    if (!reader.ReadCommand()) throw new Exception();
        //    var document = reader.Results.DocumentPayload;
        //    Assert.AreEqual(document.RootElement, "GetMetadataSchema");
        //    var cmd = (CommandGetMetadataSchema)document;
        //    Assert.AreEqual(cmd.LastKnownRuntimeID, Guid.Empty);
        //    Assert.AreEqual(cmd.LastKnownVersionNumber, (long?)null);

        //    db.ProcessCommand(cmd, codec);

        //    reader.FillBuffer(ms.ToArray(), 0, (int)ms.Position);

        //    if (!reader.ReadCommand()) throw new Exception();
        //    document = reader.Results.DocumentPayload;
        //    Assert.AreEqual(document.RootElement, "MetadataSchema");

        //    Console.WriteLine(document.ToYAML());
        //}

        [TestMethod]
        public void TestGetMetadata()
        {
            //var db = Load();

            //Queue<byte[]> packets = new Queue<byte[]>();

            //var writer = new WireEncoder();
            //var reader = new WireDecoder();

            //writer.NewPacket += (bytes, start, length) => packets.Enqueue(Clone(bytes, start, length));

            ////statements.Add(BuildRequest("Vendor", "ID", "Acronym", "Name"));
            //var s = BuildRequest("Measurement", db["Measurement"].Columns.Select(x => x.Name).ToArray());
            //var s2 = s.ToCtpDocument();
            //Console.WriteLine(s2.Length);
            ////Console.WriteLine(s2.ToXML());

            //writer.SendCustomCommand(s);

            //while (packets.Count > 0)
            //{
            //    var data = packets.Dequeue();
            //    reader.FillBuffer(data, 0, data.Length);
            //}

            //CommandObjects cmd = reader.NextCommand();
            ////Console.WriteLine(cmd.ToXMLString());

            //Assert.AreEqual(cmd.CommandName, "GetMetadata");

            //db.ProcessCommand(cmd.GetMetadata, writer);

            //while (packets.Count > 0)
            //{
            //    var data = packets.Dequeue();
            //    reader.FillBuffer(data, 0, data.Length);
            //}

            //cmd = reader.NextCommand();
            //Console.WriteLine(cmd.ToXMLString());

            //Stopwatch sw = new Stopwatch();
            //sw.Restart();
            //Console.WriteLine(cmd.Document.Length);
            //Console.WriteLine(sw.Elapsed.TotalMilliseconds);
            //sw.Restart();

            //Assert.AreEqual(cmd.CommandName, "BeginMetadataResponse");

            //MetadataQueryTable tbl = new MetadataQueryTable(cmd.BeginMetadataResponse);
            //var decoder = new MetadataRowDecoder(cmd.BeginMetadataResponse);
            //CtpObject[] values = new CtpObject[cmd.BeginMetadataResponse.Columns.Count];
            //for (int x = 0; x < values.Length; x++)
            //{
            //    values[x] = new CtpObject();
            //}
            //while ((cmd = reader.NextCommand()) != null)
            //{
            //    if (cmd.CommandCode == CommandCode.Binary)
            //    {
            //        decoder.Load(cmd.Raw.Payload);
            //        while (decoder.Read(values))
            //        {
            //            tbl.AddRow(values);
            //        }
            //    }
            //    else if (cmd.CommandCode == CommandCode.Document)
            //    {
            //        break;
            //    }
            //    else
            //    {
            //        throw new ArgumentOutOfRangeException();
            //    }
            //}
            //var t = tbl.ToTable();
            //MakeCSV(t);
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
