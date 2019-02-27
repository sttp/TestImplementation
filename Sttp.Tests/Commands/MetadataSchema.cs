using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTP;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sttp.Codec;

namespace Sttp.Tests.Commands
{
    [TestClass]
    public class Metadata_Schema
    {
        [TestMethod]
        public void GetMetadataSchema()
        {
            var cmd = new CommandGetMetadataSchema(Guid.NewGuid(), 1);
            cmd = (CommandGetMetadataSchema)(CtpCommand)cmd;
            Console.WriteLine(cmd.ToString());
        }

        [TestMethod]
        public void MetadataSchema()
        {
            var tbls = new List<MetadataSchemaTable>();
            var tbl = new MetadataSchemaTable();
            tbl.Columns.Add(new MetadataColumn("ID", CtpTypeCode.Integer));
            tbl.Columns.Add(new MetadataColumn("SignalID", CtpTypeCode.Guid));
            tbl.Columns.Add(new MetadataColumn("TagName", CtpTypeCode.String));
            tbl.Columns.Add(new MetadataColumn("Enabled", CtpTypeCode.Boolean));
            tbl.TableName = "Measurements";
            tbl.LastModifiedVersionNumber = 1;
            tbls.Add(tbl);
            tbls.Add(tbl);

            var cmd = new CommandMetadataSchema(Guid.NewGuid(), 1, tbls);
            cmd = (CommandMetadataSchema)(CtpCommand)cmd;
            Console.WriteLine(cmd.ToString());
        }

        [TestMethod]
        public void MetadataSchemaVersion()
        {
            var cmd = new CommandMetadataSchemaVersion(Guid.NewGuid(), 1);
            cmd = (CommandMetadataSchemaVersion)(CtpCommand)cmd;
            Console.WriteLine(cmd.ToString());
        }

        [TestMethod]
        public void MetadataSchemaUpdate()
        {
            var tbls = new List<MetadataSchemaTableUpdate>();
            var tbl = new MetadataSchemaTableUpdate("Measurements",1);
            tbls.Add(tbl);
            tbls.Add(tbl);

            var cmd = new CommandMetadataSchemaUpdate(Guid.NewGuid(), 1, tbls);
            cmd = (CommandMetadataSchemaUpdate)(CtpCommand)cmd;
            Console.WriteLine(cmd.ToString());
        }

    }
}
