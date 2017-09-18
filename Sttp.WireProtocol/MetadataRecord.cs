using System;
using System.Collections.Generic;

namespace Sttp.WireProtocol
{
    public class MetadataRecord
    {
        public string Table;
        public Guid DataPointID;
        public List<KeyValuePair<string, string>> Attributes;

        //For non-wrapped protocols
        // TABLE=DATAPOINTS
        // Attributes: 
        //   PointTag=
        //   SignalReference=
        //   SignalTypeID=
        //   Adder=
        //   Multiplier=
        //   Description=
        //   DeviceID=

        //For non-wrapped protocols
        // TABLE=DEVICES
        // Attributes:
        //   NodeID=
        //   Acronym=
        //   Name=
        //   CompanyID=
        //   ProtocolID=
        //   Latitude=
        //   Longitude=
        //   TimeZone=
        //   FrameRate=

        //For non-wrapped protocols
        // TABLE=NODES
        // Attributes:
        //   Name=
        //   Latitude=
        //   Longitude=
        //   Description=

        //For C37 wrapped protocols
        // Table=C37MAPPING
        // Attributes:
        //   IDCODE=Data source ID number identifies source of each data block
        //   STN=Station Name―16 bytes in ASCII format. 
        //   CHNAME=The name of the channel
        //   CHNAME[0..15]=The 16 names of the digital flags
        //   FNOM=Nominal line frequency code and flags. 
        //   DATA_RATE=Rate of data transmissions.
        //   POSITION=(STAT|FREQ|DFREQ|PM:1|PA:1|PR:1|PI:1|ANALOG:1|DIGITAL:1


    }
}
