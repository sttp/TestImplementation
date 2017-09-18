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
        public const int MaxValueSize = 64;

        /// <summary>
        /// Maps to DataPointKey.RuntimeID
        /// </summary>
        public uint ID;

        /// <summary>
        /// Contains the Value.
        /// </summary>
        public readonly byte[] Value = new byte[64];

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

        public SttpTimestamp Time;
        public QualityFlags QualityFlags;

        /// <summary>
        /// Contains the Timestamp and Quality flags. For variable length types, it can also include length and sequence numbers.
        /// </summary>
        public byte[] State;

        public byte[] Encode()
        {
            throw new NotImplementedException();
            //Encoding occurs somewhere else.
        }

        public void SetValue(sbyte value)
        {
            BigEndian.CopyBytes(value, Value, 0);
        }

        public void SetValue(short value)
        {
            BigEndian.CopyBytes(value, Value, 0);
        }

        public void SetValue(int value)
        {
            BigEndian.CopyBytes(value, Value, 0);
        }

        public void SetValue(long value)
        {
            BigEndian.CopyBytes(value, Value, 0);
        }

        public void SetValue(byte value)
        {
            BigEndian.CopyBytes(value, Value, 0);
        }

        public void SetValue(ushort value)
        {
            BigEndian.CopyBytes(value, Value, 0);
        }

        public void SetValue(uint value)
        {
            BigEndian.CopyBytes(value, Value, 0);
        }

        public void SetValue(ulong value)
        {
            BigEndian.CopyBytes(value, Value, 0);
        }

        public void SetValue(decimal value)
        {
            BigEndian.CopyBytes(value, Value, 0);
        }

        public void SetValue(double value)
        {
            BigEndian.CopyBytes(value, Value, 0);
        }

        public void SetValue(float value)
        {
            BigEndian.CopyBytes(value, Value, 0);
        }

        public void SetValue(Guid value)
        {
            throw new NotImplementedException();
            //Guid.ToByteArray does not conform to big endian bytes.
            //todo: Implment this 
            //BigEndian.CopyBytes(value, Value, 0);
        }

        public void SetValue(byte[] value, uint sequence, uint fragment, uint length)
        {
            value.CopyTo(Value, 0);
            Sequence = sequence;
            Fragment = fragment;
            Length = length;
        }

        public void SetState(DataPointState state)
        {
            throw new NotImplementedException();
            //ToDo: Come up with a better way to assign state.
        }


        public static List<DataPoint> GetDataPoints(byte[] value)
        {
            //ToDo: Need to correct this
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

                var point = new DataPoint();
                Array.Copy(value, i * MaxBufferSize,point.Value,0,length);
                point.State = BigEndian.GetBytes((ushort)i);
                dataPoints.Add(point);
            }

            return dataPoints;
        }

        public static List<DataPoint> GetDataPoints(string value, Encoding encoding)
        {
            return GetDataPoints(encoding.GetBytes(value));
        }
    }
}
