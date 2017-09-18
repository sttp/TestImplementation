using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sttp.WireProtocol
{
    // Note: this should be turned into a reusable class and therefore the initial size of 
    // Value and State should be the maximum supported size. It's not necessary to clear
    // the unused bytes on Value and State.
    
    /// <summary>
    /// Represents a single point of data.
    /// </summary>
    public class DataPoint : IEncode
    {
        public const int MaxValueSize = 16;
        public const int MaxStateSize = 20;

        /// <summary>
        /// Maps to DataPointKey.RuntimeID
        /// </summary>
        public uint ID;

        /// <summary>
        /// Contains the Value.
        /// </summary>
        public byte[] Value;

        /// <summary>
        /// Contains the Timestamp and Quality flags. For variable length types, it can also include length and sequence numbers.
        /// </summary>
        public byte[] State;

        public byte[] Encode()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                stream.Write(BigEndian.GetBytes(ID), 0, 4);

                if ((object)Value != null && Value.Length > 0)
                    stream.Write(Value, 0, Value.Length);

                if ((object)State != null && State.Length > 0)
                    stream.Write(State, 0, State.Length);

                return stream.ToArray();
            }
        }

        public void SetValue(sbyte value)
        {
            Value = BigEndian.GetBytes(value);
        }

        public void SetValue(short value)
        {
            Value = BigEndian.GetBytes(value);
        }

        public void SetValue(int value)
        {
            Value = BigEndian.GetBytes(value);
        }

        public void SetValue(long value)
        {
            Value = BigEndian.GetBytes(value);
        }

        public void SetValue(byte value)
        {
            Value = BigEndian.GetBytes(value);
        }

        public void SetValue(ushort value)
        {
            Value = BigEndian.GetBytes(value);
        }

        public void SetValue(uint value)
        {
            Value = BigEndian.GetBytes(value);
        }

        public void SetValue(ulong value)
        {
            Value = BigEndian.GetBytes(value);
        }

        public void SetValue(decimal value)
        {
            Value = BigEndian.GetBytes(value);
        }

        public void SetValue(double value)
        {
            Value = BigEndian.GetBytes(value);
        }

        public void SetValue(float value)
        {
            Value = BigEndian.GetBytes(value);
        }

        public void SetValue(Guid value)
        {
            Value = value.ToByteArray();
        }

        public void SetValue(byte[] value)
        {
            BufferValue buffer = new BufferValue { Data = Value };
            Value = buffer.Encode();
        }

        public void SetState(DataPointState state)
        {
            State = state?.Encode();
        }

        public static List<DataPoint> GetDataPoints(byte[] value)
        {
            const int MaxBufferSize = MaxValueSize - 1;

            List<DataPoint> dataPoints = new List<DataPoint>();

            // Fragment value into 15-byte chunks with sequence number
            int remainder = value.Length % MaxBufferSize;
            int fragments = value.Length / MaxBufferSize + remainder > 0 ? 1 : 0;

            for (int i = 0; i < fragments; i++)
            {
                int length = MaxBufferSize;

                if (i == fragments - 1 && remainder > 0)
                    length = remainder;

                BufferValue bufferValue = new BufferValue
                {
                    Data = value.BlockCopy(i * MaxBufferSize, length)
                };

                dataPoints.Add(new DataPoint
                {
                    Value = bufferValue.Encode(),
                    State = BigEndian.GetBytes((ushort)i)
                });
            }

            return dataPoints;
        }

        public static List<DataPoint> GetDataPoints(string value, Encoding encoding)
        {
            const int MaxBufferSize = MaxValueSize - 1;

            List<DataPoint> dataPoints = new List<DataPoint>();

            byte[] data = encoding.GetBytes(value);

            // Fragment value into 15-byte chunks with sequence number
            int remainder = data.Length % MaxBufferSize;
            int fragments = data.Length / MaxBufferSize + remainder > 0 ? 1 : 0;

            for (int i = 0; i < fragments; i++)
            {
                int length = MaxBufferSize;

                if (i == fragments - 1 && remainder > 0)
                    length = remainder;

                StringValue stringValue = new StringValue
                {
                    Data = data.BlockCopy(i * MaxBufferSize, length)
                };

                dataPoints.Add(new DataPoint
                {
                    Value = stringValue.Encode(),
                    State = BigEndian.GetBytes((ushort)i)
                });
            }

            return dataPoints;
        }
    }
}
