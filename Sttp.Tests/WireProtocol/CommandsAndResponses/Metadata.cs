using System;
using System.Collections.Generic;
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
            CommandBase cmd = new CommandGetMetadataSchema(Guid.NewGuid(), 382);
            Console.WriteLine(cmd.ToString());
        }

        [TestMethod]
        public void CommandMetadataSchema()
        {
            var tbl = new List<MetadataSchemaTable>();
            var t = new MetadataSchemaTable();
            t.TableName = "Measurement";
            t.LastModifiedSequenceNumber = 382;
            t.Columns.Add(new MetadataColumn("ID", SttpValueTypeCode.Int64));
            t.Columns.Add(new MetadataColumn("Name", SttpValueTypeCode.String));
            t.Columns.Add(new MetadataColumn("DeviceID", SttpValueTypeCode.Int64));
            t.ForeignKeys.Add(new MetadataForeignKey("DeviceID", "Device"));
            tbl.Add(t);
            t = new MetadataSchemaTable();
            t.TableName = "Device";
            t.LastModifiedSequenceNumber = 382;
            t.Columns.Add(new MetadataColumn("ID", SttpValueTypeCode.Int64));
            t.Columns.Add(new MetadataColumn("Name", SttpValueTypeCode.String));
            tbl.Add(t);
            var cmd = new CommandMetadataSchema(Guid.NewGuid(), 382, tbl);
            Console.WriteLine(cmd.ToString());
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
            Console.WriteLine(cmd.ToString());
        }

        [TestMethod]
        public void CommandMetadataSchemaVersion()
        {
            var cmd = new CommandMetadataSchemaVersion(Guid.NewGuid(), 382);
            Console.WriteLine(cmd.ToString());
        }



    }
}
