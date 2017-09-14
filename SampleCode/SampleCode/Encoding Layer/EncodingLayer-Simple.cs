using System;
using System.IO;

namespace sttp2
{
    /// <summary>
    /// When commands are received on the socket layer, this event is raised.
    /// </summary>
    public class CommandReceivedEventArgs : EventArgs
    {
        public byte CommandCode;
        public byte ResponseCode; //255 if this is not a response packet.
        public int Position;
        public int Length;
        public byte[] CommandBytes;
    }

    /// <summary>
    /// This layer will encode/decode STTP messages into their wire line protocol, optionally compressing them.
    /// </summary>
    public class EncodingLayer
    {
        /// <summary>
        /// A command has been received from the remote socket.
        /// </summary>
        public event EventHandler<CommandReceivedEventArgs> CommandReceived;

        /// <summary>
        /// Occurs when receiving a signal mapping structure.
        /// </summary>
        public event EventHandler<DataPointKey> NewSignalMapping;

        public EncodingLayer(Stream stream) { }

        public void RegisterSignalMapping(DataPointKey requiredMetadata)
        {
            //Sends this to the client and builds compression state data for it.
        }

        public void SendCommand(bool sendReliably, byte responseCode, byte commandCode, byte[] data, int position, int length)
        {

        }

        public void SendDataPoints(bool sendReliably, DataPointPadded[] points)
        {
            //Serializes these data points, and compresses them if applicable. 
        }
    }

    public enum ValueTypeCode
    {
        Null = 0, // 0-bytes
        SByte = 1, // 1-byte
        Int16 = 2, // 2-bytes
        Int32 = 3, // 4-bytes
        Int64 = 4, // 8-bytes
        Byte = 5, // 1-byte
        UInt16 = 6, // 2-bytes
        UInt32 = 7, // 4-bytes
        UInt64 = 8, // 8-bytes
        Decimal = 9, // 16-bytes
        Double = 10, // 8-bytes
        Single = 11, // 4-bytes
        Ticks = 12, // 8-bytes
        Bool = 13, // 1-byte
        Guid = 14, // 16-bytes
        String = 15, // 64-bytes, max
        Buffer = 16 // 64-bytes, max
    }

    /// <summary>
    /// Reusable structure for data points that are larger than 16 bytes in size.
    /// </summary>
    public class LargeType
    {
        public byte[] Data = new byte[64];
        /// <summary>
        /// Increments every time that a new value larger than 64 bytes is created. 0 if the data doesn't need to be fragmented.
        /// </summary>
        public uint Sequence;
        /// <summary>
        /// The fragment index of data that is being sent.
        /// </summary>
        public uint Fragment;
        /// <summary>
        /// The total size of all fragments.
        /// </summary>
        public uint Length;
    }

    public class DataPointPadded
    {
        public uint RuntimeID;
        public ulong Value1; //The lower 8 bytes of the value. 
        public ulong Value2; //The upper 8 bytes of the value. Only valid for types that require more than 8 bytes;
        public LargeType ExtendedValueBuffer; //Required if MaxValueLength > 16. Otherwise this value will be null.
        public ulong Timestamp1; //Lower 8 bytes of the time.
        public ulong Timestamp2; //Upper 8 bytes of the time.
        public uint TimestampFlags;
        public uint QualityFlags;
    }

    public class DataPointKey
    {
        public uint RuntimeID;
        public string Identifier; //Can include any mix of the following: GUID:{2238...23}; DeviceID = 2038; DeviceName='PMU123'; Type=PA:3
        public ValueTypeCode Type;

        /// <summary>
        /// Information for the encoding algorithm.
        /// </summary>
        public int MaxValueLength; //The maximum number of bytes required to store the length
        public int MaxTimestampLength; //The maximum number of bytes to store the timestamp
        public int MaxTimestampFlagsLength; //The maximum number of bytes to store the timestamp flags
        public int MaxQualityFlagsLength; //The maximum number of bytes to store the quality flags
        public int SequenceLength; //The maximum number of bytes to store the sequence length
    }
}
