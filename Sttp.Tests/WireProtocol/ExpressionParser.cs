using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sttp.Tests.WireProtocol
{
    [TestClass]
    public class ExpressionParser
    {
        [TestMethod]
        public unsafe void Test()
        {
            var cmd = new SttpQueryExpression();
            cmd.BaseTable = "DataPoint";
            cmd.DefineDirectColumn("PointID", "PointID");
            cmd.DefineDirectColumn("PointTag", "PointTag");
            cmd.DefineDirectColumn("Phase Designation", "Phase Designation");
            cmd.DefineIndirectColumn("Protocol", "Protocol", SttpQueryJoinPath.Create("ProducerTableID", "PMU"));
            cmd.DefineIndirectColumn("Rated MVA", "Rated MVA", SttpQueryJoinPath.Create("ProducerTableID", "PMU"));
            cmd.DefineIndirectColumn("Nominal Voltage", "Nominal Voltage", SttpQueryJoinPath.Create("ProducerTableID", "PMU"));
            cmd.DefineIndirectColumn("SubName", "Name", SttpQueryJoinPath.Create("ProducerTableID", "PMU"), SttpQueryJoinPath.Create("SubstationTableID", "Substation"));
            cmd.DefineValue("MVA to KVA", (SttpValue)1000.0);
            cmd.DefineValue("Min Voltage", (SttpValue)345000.0);
            cmd.DefineValue("Desired Protocol", (SttpValue)"C37.118");
            cmd.DefineProcedures("MUL", new[] { "Rated MVA", "MVA to KVA" }, new[] { "Rated KVA" });
            cmd.DefineProcedures(">=", new[] { "Nominal Voltage", "Min Voltage" }, new[] { "Condition1" });
            cmd.DefineProcedures("=", new[] { "Protocol", "Desired Protocol" }, new[] { "IsC37" });
            cmd.DefineOutputs("PointID", "PointID", SttpValueTypeCode.Guid);
            cmd.DefineOutputs("PointTag", "PointTag", SttpValueTypeCode.String);
            cmd.DefineOutputs("Phase Designation", "Phase", SttpValueTypeCode.String);
            cmd.DefineOutputs("IsC37", "IsC37", SttpValueTypeCode.Bool);
            cmd.DefineOutputs("Rated KVA", "Rated KVA", SttpValueTypeCode.Double);
            cmd.DefineOutputs("Nominal Voltage", "Nominal Voltage", SttpValueTypeCode.Double);
            cmd.DefineOutputs("SubName", "SubName", SttpValueTypeCode.String);
            cmd.WhereBooleanVariable = "Condition1";

            Console.Write(cmd.ToString());
        }
    }
}
