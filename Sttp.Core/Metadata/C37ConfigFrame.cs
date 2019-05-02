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

        private static string TrimAsciiWhitespace(string value)
        {
            int end = value.Length - 1;
            int start = 0;
            while (start < value.Length && ((int)value[start] <= 32 || (int)value[start] >= 127))
            {
                start++;
            }
            while (end >= start && ((int)value[end] <= 32 || (int)value[end] >= 127))
            {
                end--;
            }
            return value.Substring(start, end - start + 1);
        }

        public static CommandC37ConfigFrame OpenIeeeBytes(byte[] data)
        {
            var rv = new CommandC37ConfigFrame();

            var stream = new ByteReader(data);
            var syncByte1 = stream.ReadByte();
            if (syncByte1 != 0xAA)
                throw new Exception("Unknown Sync Byte");

            var syncByte2 = stream.ReadByte();
            switch ((syncByte2 >> 4) & 7)
            {
                case 3:
                    break;
                default:
                    throw new Exception("Not a configuration frame2");
            }

            switch (syncByte2 & 15)
            {
                case 1:
                    break;
                default:
                    throw new Exception("Not supported protocol version");
            }

            var frameSize = stream.ReadUInt16();
            if (frameSize != data.Length)
                throw new Exception("Wrong frame size");

            rv.PDCIDCode = stream.ReadUInt16();
            var socTime = stream.ReadUInt32();
            var fracSec = stream.ReadUInt32();
            var timeBase = stream.ReadUInt32();

            var numPmUs = stream.ReadUInt16();

            for (int x = 0; x < numPmUs; x++)
            {
                rv.OpenPMU(stream);
            }

            var dataRate = stream.ReadInt16();

            if (dataRate >= 0)
            {
                rv.SamplesPerDay = dataRate * 3600L * 24;
            }
            else
            {
                rv.SamplesPerDay = -3600 * 24 / dataRate;
            }

            var crc = stream.ReadUInt16();

            if (stream.Position != stream.Length)
                throw new Exception("Config Frame not the correct length");

            return rv;
        }

        private void OpenPMU(ByteReader stream)
        {
            var rv = new C37PMUConfig();
            var channelName = new List<string>();
            var conversionFactors = new List<uint>();
            rv.StationName = TrimAsciiWhitespace(stream.ReadString(16, Encoding.ASCII));
            rv.PMUIDCode = stream.ReadUInt16();
            var format = stream.ReadUInt16();

            var phasorsCount = stream.ReadUInt16();
            var analogsCount = stream.ReadUInt16();
            var digitalsCount = stream.ReadUInt16();

            for (int x = 0; x < phasorsCount + analogsCount + 16 * digitalsCount; x++)
            {
                channelName.Add(TrimAsciiWhitespace(stream.ReadString(16, Encoding.ASCII)));
            }

            for (int x = 0; x < phasorsCount + analogsCount + digitalsCount; x++)
            {
                conversionFactors.Add(stream.ReadUInt32());
            }

            var freqAndFlags = stream.ReadUInt16();
            rv.NominalFrequency = ((freqAndFlags & 1) == 0) ? 60 : 50;
            var configChangeCount = stream.ReadUInt16();

            var nameQueue = new Queue<string>(channelName);
            var factors = new Queue<uint>(conversionFactors);

            rv.Measurements.Add(new C37MeasurementConfig("Status", 0, "", "STATUS"));

            for (int x = 0; x < phasorsCount; x++)
            {
                var name = nameQueue.Dequeue();
                var factor = factors.Dequeue();
                rv.Measurements.Add(new C37MeasurementConfig("PhasorMag", x, (factor >> 24) == 0 ? "V" : "A", name));
                rv.Measurements.Add(new C37MeasurementConfig("PhasorAng", x, "DEG", name));
            }

            rv.Measurements.Add(new C37MeasurementConfig("Freq", 0, "Hz", "FREQUENCY"));
            rv.Measurements.Add(new C37MeasurementConfig("DFreq", 0, "Hz/sec", "DFREQUENCY"));

            for (int x = 0; x < analogsCount; x++)
            {
                factors.Dequeue();
                rv.Measurements.Add(new C37MeasurementConfig("Analog", x, "", nameQueue.Dequeue()));
            }

            for (int x = 0; x < digitalsCount; x++)
            {
                List<string> digitalNames = new List<string>();
                while (digitalNames.Count < 16)
                    digitalNames.Add(nameQueue.Dequeue());

                rv.Measurements.Add(new C37MeasurementConfig("Digitals", x, "", digitalNames.ToArray()));
            }
        }
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

        public C37MeasurementConfig(string measurementCategory, int measurementIndex, string units, params string[] channelName)
        {
            MeasurementCategory = measurementCategory;
            MeasurementIndex = measurementIndex;
            Units = units;
            ChannelName = channelName.ToList();
        }




    }
}
