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
    public class Metadata
    {
        [TestMethod]
        public unsafe void NumericConsts()
        {
            float value;
            value = -1; Console.WriteLine($"//{value} = 0x{(*(uint*)&value).ToString("X8")}");
            value = 0; Console.WriteLine($"//{value} = 0x{(*(uint*)&value).ToString("X8")}");
            value = 1; Console.WriteLine($"//{value} = 0x{(*(uint*)&value).ToString("X8")}");
        }
        [TestMethod]
        public unsafe void NumericConsts2()
        {
            double value;
            value = -1; Console.WriteLine($"//{value} = 0x{(*(ulong*)&value).ToString("X16")}");
            value = 0; Console.WriteLine($"//{value} = 0x{(*(ulong*)&value).ToString("X16")}");
            value = 1; Console.WriteLine($"//{value} = 0x{(*(ulong*)&value).ToString("X16")}");
        }

        [TestMethod]
        public void GetMetadata()
        {
            var cmd = new CommandGetMetadata("Measurement", new string[] { "ID", "SignalID", "TagName" });
            cmd.DebugToConsole();
            cmd = (CommandGetMetadata)(CtpCommand)cmd;
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

            cmd.DebugToConsole();
            cmd = (CommandMetadataSchema)(CtpCommand)cmd;
        }

        [TestMethod]
        public void MetadataSchemaVersion()
        {
            var cmd = new CommandMetadataSchemaVersion(Guid.NewGuid(), 1);
            cmd.DebugToConsole();
            cmd = (CommandMetadataSchemaVersion)(CtpCommand)cmd;
        }

        [TestMethod]
        public void MetadataSchemaUpdate()
        {
            var tbls = new List<MetadataSchemaTableUpdate>();
            var tbl = new MetadataSchemaTableUpdate("Measurements", 1);
            tbls.Add(tbl);
            tbls.Add(tbl);

            var cmd = new CommandMetadataSchemaUpdate(Guid.NewGuid(), 1, tbls);
            cmd.DebugToConsole();
            cmd = (CommandMetadataSchemaUpdate)(CtpCommand)cmd;
        }

    }
}
