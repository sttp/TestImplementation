using System;
using System.Collections.Generic;
using System.Diagnostics;
using CTP;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sttp.Codec;

namespace Sttp.Tests.CommandsAndResponses
{
    [TestClass]
    public class Metadata
    {
        [TestMethod]
        public void CommandGetMetadataSchema()
        {
            var cmd = new CommandGetMetadataSchema(Guid.NewGuid(), 382);
            Console.WriteLine(((CtpDocument)cmd).ToYAML());
        }

        [TestMethod]
        public void CommandMetadataSchema()
        {
            var tbl = new List<MetadataSchemaTable>();
            var t = new MetadataSchemaTable();
            t.TableName = "Measurement";
            t.LastModifiedVersionNumber = 382;
            t.Columns.Add(new MetadataColumn("ID", CtpTypeCode.Int64));
            t.Columns.Add(new MetadataColumn("Name", CtpTypeCode.String));
            t.Columns.Add(new MetadataColumn("DeviceID", CtpTypeCode.Int64));
            tbl.Add(t);
            t = new MetadataSchemaTable();
            t.TableName = "Device";
            t.LastModifiedVersionNumber = 382;
            t.Columns.Add(new MetadataColumn("ID", CtpTypeCode.Int64));
            t.Columns.Add(new MetadataColumn("Name", CtpTypeCode.String));
            tbl.Add(t);
            var cmd = new CommandMetadataSchema(Guid.NewGuid(), 382, tbl);
            Console.WriteLine(((CtpDocument)cmd).ToYAML());
            Console.WriteLine(((CtpDocument)(CommandMetadataSchema)(CtpDocument)cmd).ToYAML());
        }

        [TestMethod]
        public void BenchmarkSave()
        {
            var tbl = new List<MetadataSchemaTable>();
            var t = new MetadataSchemaTable();
            t.TableName = "Measurement";
            t.LastModifiedVersionNumber = 382;
            t.Columns.Add(new MetadataColumn("ID", CtpTypeCode.Int64));
            t.Columns.Add(new MetadataColumn("Name", CtpTypeCode.String));
            t.Columns.Add(new MetadataColumn("DeviceID", CtpTypeCode.Int64));
            tbl.Add(t);
            t = new MetadataSchemaTable();
            t.TableName = "Device";
            t.LastModifiedVersionNumber = 382;
            t.Columns.Add(new MetadataColumn("ID", CtpTypeCode.Int64));
            t.Columns.Add(new MetadataColumn("Name", CtpTypeCode.String));
            tbl.Add(t);
            var cmd = new CommandMetadataSchema(Guid.NewGuid(), 382, tbl);
            cmd.ToDocument();

            Stopwatch sw = Stopwatch.StartNew();
            for (int x = 0; x < 1_000_000; x++)
            {
                cmd.ToDocument();
            }
            Console.WriteLine(sw.Elapsed.TotalSeconds);

        }

        [TestMethod]
        public void CommandMetadataSchemaUpdate()
        {
            var tbl = new List<MetadataSchemaTableUpdate>();
            var t = new MetadataSchemaTableUpdate("Measurement", 382);
            tbl.Add(t);
            t = new MetadataSchemaTableUpdate("Device", 382);
            tbl.Add(t);
            var cmd = new CommandMetadataSchemaUpdate(Guid.NewGuid(), 382, tbl);
            Console.WriteLine(((CtpDocument)cmd).ToYAML());
        }

        [TestMethod]
        public void CommandMetadataSchemaVersion()
        {
            var cmd = new CommandMetadataSchemaVersion(Guid.NewGuid(), 382);
            Console.WriteLine(((CtpDocument)cmd).ToYAML());
        }



    }
}
