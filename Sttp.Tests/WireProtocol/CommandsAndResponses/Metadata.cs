using System;
using System.Collections.Generic;
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
            Console.WriteLine(cmd.ToString());
            Console.WriteLine(CtpDocument.Serialize(cmd, "GetMetadataSchema").ToYAML());
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
            Console.WriteLine(cmd.ToString());
            Console.WriteLine(CtpDocument.Serialize(cmd, "MetadataSchema").ToYAML());
            Console.WriteLine(CtpDocument.Load<CommandMetadataSchema>(CtpDocument.Serialize(cmd, "MetadataSchema")));

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
            Console.WriteLine(CtpDocument.Serialize(cmd, "MetadataSchemaUpdate").ToYAML());
        }

        [TestMethod]
        public void CommandMetadataSchemaVersion()
        {
            var cmd = new CommandMetadataSchemaVersion(Guid.NewGuid(), 382);
            Console.WriteLine(cmd.ToString());
            Console.WriteLine(CtpDocument.Serialize(cmd, "MetadataSchemaVersion").ToYAML());
        }



    }
}
