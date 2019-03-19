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


            Console.WriteLine("-------Schema----------");

            for (int x = 0; x < cmd.Schema.NodeCount; x++)
            {
                Console.WriteLine(cmd.Schema[x]);
            }

            Console.WriteLine("-------Data----------");


            var rdr3 = cmd.ToCommand().MakeDataReader();
            while (!rdr3.IsEmpty)
            {
                Console.WriteLine(rdr3.Read());
            }

            Console.WriteLine("-------Reader----------");
            var rdr = cmd.ToCommand().MakeReader();

            Console.WriteLine(rdr.ToString());
            while (rdr.Read())
            {
                Console.WriteLine(rdr.ToString());
            }
            Console.WriteLine(rdr.ToString());

            Console.WriteLine("-------Text----------");

            Console.WriteLine(cmd.ToString());

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

            var rdr = cmd.ToCommand().MakeReader();
            while (rdr.Read())
            {
                Console.WriteLine($"{rdr.NodeType,15} {rdr.ElementName,15} {rdr.ValueName,25} {rdr.Value}");
            }

            Console.WriteLine("-------Schema----------");

            for (int x = 0; x < cmd.Schema.NodeCount; x++)
            {
                Console.WriteLine(cmd.Schema[x]);
            }

            Console.WriteLine("-------Data----------");


            var rdr3 = cmd.ToCommand().MakeDataReader();
            while (!rdr3.IsEmpty)
            {
                Console.WriteLine(rdr3.Read());
            }

            Console.WriteLine("-------Text----------");


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
            var tbl = new MetadataSchemaTableUpdate("Measurements", 1);
            tbls.Add(tbl);
            tbls.Add(tbl);

            var cmd = new CommandMetadataSchemaUpdate(Guid.NewGuid(), 1, tbls);
            cmd = (CommandMetadataSchemaUpdate)(CtpCommand)cmd;
            Console.WriteLine(cmd.ToString());
        }

    }
}
