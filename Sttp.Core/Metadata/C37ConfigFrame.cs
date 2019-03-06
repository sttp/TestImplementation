using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTP;

namespace Sttp.Metadata
{
    [CommandName("C37ConfigFrame")]
    public class CommandC37ConfigFrame
        : CommandObject<CommandC37ConfigFrame>
    {
        [CommandField()]
        public int PDCIDCode { get; set; }

        [CommandField()]
        public long SamplesPerDay { get; set; }

        [CommandField()]
        public List<C37PMUConfig> PMUs { get; set; }

        public CommandC37ConfigFrame()
        {
            PMUs = new List<C37PMUConfig>();
        }

        public static explicit operator CommandC37ConfigFrame(CtpCommand obj)
        {
            return FromCommand(obj);
        }

        //public byte[] GetCommandData()
        //{
        //    var wr = new CtpObjectWriter();
        //    wr.Write(PDCIDCode);
        //    wr.Write(SamplesPerDay);
        //    wr.Write(PMUs.Count);
        //    foreach (var pmu in PMUs)
        //    {
        //        wr.Write(pmu.PMUIDCode);
        //        wr.Write(pmu.StationName);
        //        wr.Write(pmu.NominalFrequency);
        //        wr.Write(pmu.Measurements.Count);
        //        foreach (var m in pmu.Measurements)
        //        {
        //            wr.Write(m.MeasurementID);
        //            wr.Write(m.ChannelName.Count);
        //            foreach (var c in m.ChannelName)
        //            {
        //                wr.Write(c);
        //            }
        //            wr.Write(m.MeasurementCategory);
        //            wr.Write(m.MeasurementIndex);
        //            wr.Write(m.Units);
        //        }
        //    }

        //    return wr.ToArray();
        //}

        //public static CtpCommandSchema GetSchema()
        //{
        //    var wr = new CommandSchemaWriter();
        //    wr.DefineElement("C37ConfigFrame");
        //     wr.DefineValue("PDCIDCode");
        //     wr.DefineValue("SamplesPerDay");
        //     wr.DefineArray("PMUs");
        //      wr.DefineElement("Item");
        //       wr.DefineValue("PMUIDCode");
        //       wr.DefineValue("StationName");
        //       wr.DefineValue("NominalFrequency");
        //       wr.DefineArray("Measurements");
        //        wr.DefineElement("Item");
        //         wr.DefineValue("MeasurementID");
        //         wr.DefineArray("ChannelName");
        //          wr.DefineValue("Item");
        //         wr.DefineValue("MeasurementValue");
        //         wr.DefineValue("MeasurementIndex");
        //        wr.DefineValue("Units");
        //       wr.EndElement();
        //      wr.EndElement();
        //    wr.EndElement();
        //    return wr.ToSchema();
        //}
    }

    public class C37PMUConfig
    {
        [CommandField()]
        public int PMUIDCode { get; set; }

        [CommandField()]
        public string StationName { get; set; }

        [CommandField()]
        public int NominalFrequency { get; set; }

        [CommandField()]
        public List<C37MeasurementConfig> Measurements;

        public C37PMUConfig()
        {
            Measurements = new List<C37MeasurementConfig>();
        }
    }

    public class C37MeasurementConfig
    {
        /// <summary>
        /// An ID field that will uniquely define this measurement. 
        /// </summary>
        [CommandField()]
        public CtpObject MeasurementID { get; set; }

        [CommandField()]
        public List<string> ChannelName { get; set; }

        [CommandField()]
        public string MeasurementCategory { get; set; }

        [CommandField()]
        public int MeasurementIndex { get; set; }

        [CommandField()]
        public string Units { get; set; }

        public C37MeasurementConfig()
        {
            ChannelName = new List<string>();
        }
    }
}
