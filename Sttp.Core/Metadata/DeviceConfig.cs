using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTP;

namespace Sttp.Metadata
{
    [CommandName("DeviceConfig")]
    public class CommandDeviceConfig
        : CommandObject<CommandDeviceConfig>
    {
        //SELECT NodeID, UniqueID, OriginalSource, IsConcentrator, Acronym, Name, AccessID,
        //ParentAcronym, ProtocolName, FramesPerSecond, CompanyAcronym, VendorAcronym, VendorDeviceName,
        //Longitude, Latitude, InterconnectionName, ContactList, Enabled, UpdatedOn FROM DeviceDetail WHERE IsConcentrator = 0
        [CommandField()]
        public Guid NodeID { get; set; }
        [CommandField()]
        public Guid UniqueID { get; set; }
        [CommandField()]
        public string OriginalSource { get; set; }
        [CommandField()]
        public bool IsConcentrator { get; set; }
        [CommandField()]
        public string Acronym { get; set; }
        [CommandField()]
        public string Name { get; set; }
        [CommandField()]
        public int AccessID { get; set; }
        [CommandField()]
        public string ParentAcronym { get; set; }
        [CommandField()]
        public string ProtocolName { get; set; }
        [CommandField()]
        public int FramesPerSecond { get; set; }
        [CommandField()]
        public string CompanyAcronym { get; set; }
        [CommandField()]
        public string VendorAcronym { get; set; }
        [CommandField()]
        public string VendorDeviceName { get; set; }
        [CommandField()]
        public double? Longitude { get; set; }
        [CommandField()]
        public double? Latitude { get; set; }
        [CommandField()]
        public string InterconnectionName { get; set; }
        [CommandField()]
        public string ContactList { get; set; }
        [CommandField()]
        public bool Enabled { get; set; }
        [CommandField()]
        public DateTime UpdatedOn { get; set; }
        [CommandField()]
        public List<MeasurementConfig> Measurements { get; set; }

        public CommandDeviceConfig()
        {
            Measurements = new List<MeasurementConfig>();
        }

        public static explicit operator CommandDeviceConfig(CtpCommand obj)
        {
            return FromCommand(obj);
        }
    }

    public class MeasurementConfig
    {
        //SELECT DeviceAcronym, ID, SignalID, PointTag, SignalReference, SignalAcronym,
        //PhasorSourceIndex, Description, Internal, Enabled, UpdatedOn FROM MeasurementDetail 

        /// <summary>
        /// An ID field that will uniquely define this measurement. 
        /// </summary>
        [CommandField()]
        public string DeviceAcronym { get; set; }
        [CommandField()]
        public string ID { get; set; }
        [CommandField()]
        public Guid SignalID { get; set; }
        [CommandField()]
        public string PointTag { get; set; }
        [CommandField()]
        public string SignalReference { get; set; }
        [CommandField()]
        public string SignalAcronym { get; set; }
        [CommandField()]
        public int? PhasorSourceIndex { get; set; }
        [CommandField()]
        public string Description { get; set; }
        [CommandField()]
        public bool Internal { get; set; }
        [CommandField()]
        public bool Enabled { get; set; }
        [CommandField()]
        public DateTime UpdatedOn { get; set; }
        [CommandField()]
        public PhasorConfig PhasorConfig { get; set; }

        public MeasurementConfig()
        {
        }
    }

    public class PhasorConfig
    {
        //SELECT ID, DeviceAcronym, Label, Type, Phase, DestinationPhasorID, SourceIndex, UpdatedOn FROM PhasorDetail

        [CommandField()]
        public int ID { get; set; }

        [CommandField()]
        public string DeviceAcronym { get; set; }

        [CommandField()]
        public string Label { get; set; }

        [CommandField()]
        public string Type { get; set; }

        [CommandField()]
        public string Phase { get; set; }

        [CommandField()]
        public int? DestinationPhasorID { get; set; }

        [CommandField()]
        public int SourceIndex { get; set; }

        [CommandField()]
        public DateTime UpdatedOn { get; set; }


    }
}
