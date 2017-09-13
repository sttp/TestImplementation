using System;
using System.IO;

namespace sttp
{
    /// <summary>
    /// When commands are received on the socket layer, this event is raised.
    /// </summary>
    public class CommandReceivedEventArgs : EventArgs
    {
        public byte CommandCode;
        public int Position;
        public int Length;
        public byte[] CommandBytes;
    }

    /// <summary>
    /// When commands are received on the socket layer, this event is raised.
    /// </summary>
    public class CommandResponseReceivedEventArgs : EventArgs
    {
        public byte ResponseCode;
        public byte CommandCode;
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
        public event EventHandler<CommandResponseReceivedEventArgs> CommandResponseReceived;

        public EncodingLayer(Stream stream) { }

        public void SendCommand(bool sendReliably, byte commandCode, byte[] data, int position, int length)
        {
            
        }
        public void SendCommandResponse(bool sendReliably, byte responseCode, byte commandCode, byte[] data, int position, int length)
        {

        }

        public void SendDataPoints(bool sendReliably, DataPoint[] points)
        {
            
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

    public enum StateFlags
    {
        None = 0, // No state is defined
        Timestamp = 1 << 0, // State includes Timestamp
        Quality = 1 << 1, // State includes QualityFlags
        Sequence = 1 << 2, // State includes sequence number as uint16
    }

    public class DataPoint
    {
        public uint RuntimeID;
        public ValueTypeCode Type;
        public StateFlags Flags;
        public byte[] Value;
        public byte[] State;
    }
}
