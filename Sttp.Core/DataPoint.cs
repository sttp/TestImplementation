using System;
using System.Text;
using Sttp.WireProtocol.SendDataPoints;

namespace Sttp.WireProtocol
{
    /// <summary>
    /// Represents a single point of data.
    /// </summary>
    public class DataPoint
    {
        public DataPointKey Key;
        public byte[] Value;
        public int ValueLength;
        public DateTime Time;
        public TimeQualityFlags Flags;
        public DataQualityFlags QualityFlags;

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

        public void SetValue(byte[] value, int length)
        {
            value.CopyTo(Value, 0);
            ValueLength = length;
        }

        public void SetValue(string value)
        {
            Value = Encoding.UTF8.GetBytes(value);
            ValueLength = Value.Length;
        }

    }
}
