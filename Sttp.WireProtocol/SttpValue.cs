using System;
using System.Runtime.InteropServices;
using System.Text;
using Sttp.WireProtocol.SendDataPoints;

namespace Sttp.WireProtocol
{
    /// <summary>
    /// This class contains the fundamental value for STTP.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public class SttpValue : ISttpValue
    {
        #region [ Members ]

        [FieldOffset(0)]
        private ulong m_bytes0to7; //Used for cloning data.

        [FieldOffset(0)]
        private int m_valueInt32;
        [FieldOffset(0)]
        private long m_valueInt64;
        [FieldOffset(0)]
        private float m_valueSingle;
        [FieldOffset(0)]
        private double m_valueDouble;

        [FieldOffset(8)]
        private object m_objectValue;

        [FieldOffset(16)]
        private SttpFundamentalTypeCode m_fundamentalTypeCode;

        #endregion

        #region [ Constructors ]

        public SttpValue()
        {
            IsNull = true;
        }

        /// <summary>
        /// Clones an <see cref="SttpValue"/>. 
        /// </summary>
        /// <param name="value">the value to clone</param>
        public SttpValue(SttpValue value)
        {
            m_bytes0to7 = value.m_bytes0to7;
            m_objectValue = value.m_objectValue;
            m_fundamentalTypeCode = value.m_fundamentalTypeCode;
        }

        #endregion

        #region [ Properties ]

        public int AsInt32
        {
            get
            {
                if (m_fundamentalTypeCode == SttpFundamentalTypeCode.Int32)
                    return m_valueInt32;
                throw new NotSupportedException();
            }
            set
            {
                m_fundamentalTypeCode = SttpFundamentalTypeCode.Int32;
                m_valueInt32 = value;
                m_objectValue = null;
            }
        }

        public long AsInt64
        {
            get
            {
                if (m_fundamentalTypeCode == SttpFundamentalTypeCode.Int64)
                    return m_valueInt64;
                throw new NotSupportedException();
            }
            set
            {
                m_fundamentalTypeCode = SttpFundamentalTypeCode.Int64;
                m_valueInt64 = value;
                m_objectValue = null;
            }
        }

        public float AsSingle
        {
            get
            {
                if (m_fundamentalTypeCode == SttpFundamentalTypeCode.Single)
                    return m_valueSingle;
                throw new NotSupportedException();
            }
            set
            {
                m_fundamentalTypeCode = SttpFundamentalTypeCode.Single;
                m_valueSingle = value;
                m_objectValue = null;
            }
        }

        public double AsDouble
        {
            get
            {
                if (m_fundamentalTypeCode == SttpFundamentalTypeCode.Double)
                    return m_valueDouble;
                throw new NotSupportedException();
            }
            set
            {
                m_fundamentalTypeCode = SttpFundamentalTypeCode.Double;
                m_valueDouble = value;
                m_objectValue = null;
            }
        }

        public byte[] AsBuffer
        {
            get
            {
                if (m_fundamentalTypeCode == SttpFundamentalTypeCode.Buffer)
                    return (byte[])m_objectValue;
                throw new NotSupportedException();
            }
            set
            {
                if (value == null)
                {
                    IsNull = true;
                }
                else
                {
                    m_fundamentalTypeCode = SttpFundamentalTypeCode.Buffer;
                    m_objectValue = value;
                }
            }
        }

        public string AsString
        {
            get
            {
                if (m_fundamentalTypeCode == SttpFundamentalTypeCode.String)
                    return (String)m_objectValue;
                throw new NotSupportedException();
            }
            set
            {
                if (value == null)
                {
                    IsNull = true;
                }
                else
                {
                    m_fundamentalTypeCode = SttpFundamentalTypeCode.String;
                    m_objectValue = value;
                }
            }
        }

        /// <summary>
        /// Gets if this class has a value. Clear this by calling <see cref="SetValueToNull"/>
        /// </summary>
        /// <exception cref="InvalidOperationException">When setting this value to true. Set a value by calling one of the <see cref="SetValue"/> methods.</exception>
        public bool IsNull
        {
            get
            {
                return m_fundamentalTypeCode == SttpFundamentalTypeCode.Null;
            }
            set
            {
                if (!value)
                    throw new InvalidOperationException("Can only set a value to null with this property, to set not null, use one of the other properties to set the value.");
                m_objectValue = null;
            }
        }

        public void SetValue(SttpValue value)
        {
            m_bytes0to7 = value.m_bytes0to7;
            m_objectValue = value.m_objectValue;
            m_fundamentalTypeCode = value.m_fundamentalTypeCode;
        }

        /// <summary>
        /// The type code of the raw value.
        /// </summary>
        public SttpFundamentalTypeCode FundamentalTypeCode
        {
            get
            {
                return m_fundamentalTypeCode;
            }
        }

        #endregion

        #region [ Methods ] 

        /// <summary>
        /// Clones the value
        /// </summary>
        /// <returns></returns>
        public SttpValue Clone()
        {
            return new SttpValue(this);
        }

        public unsafe byte[] Save()
        {
            //Encoding:
            //Bits 0 - 3: Encoding Method
            //Bits 4 - 7: Length
            int code = 0;
            int length = 0;
            byte[] buffer;

            switch (m_fundamentalTypeCode)
            {
                case SttpFundamentalTypeCode.Null:
                    code = 0;
                    buffer = new byte[1];
                    break;
                case SttpFundamentalTypeCode.Int32:
                    int value = m_valueInt32;
                    if (value < 0)
                    {
                        value = ~value;
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    if (value == 0)
                    {
                        length = 0;
                        buffer = new byte[1];
                    }
                    else if (value <= 0xFF)
                    {
                        length = 1;
                        buffer = new byte[2];
                        buffer[1] = (byte)value;
                    }
                    else if (value <= 0xFF)
                    {
                        length = 2;
                        buffer = new byte[3];
                        buffer[1] = (byte)(value >> 8);
                        buffer[2] = (byte)value;
                    }
                    else if (value <= 0xFFFF)
                    {
                        length = 3;
                        buffer = new byte[4];
                        buffer[1] = (byte)(value >> 16);
                        buffer[2] = (byte)(value >> 8);
                        buffer[3] = (byte)value;
                    }
                    else
                    {
                        length = 4;
                        buffer = new byte[5];
                        buffer[1] = (byte)(value >> 24);
                        buffer[2] = (byte)(value >> 16);
                        buffer[3] = (byte)(value >> 8);
                        buffer[4] = (byte)value;
                    }
                    buffer[0] = (byte)(code | (length << 4));
                    break;
                case SttpFundamentalTypeCode.Int64:
                    long valuel = m_valueInt64;
                    if (valuel < 0)
                    {
                        valuel = ~valuel;
                        code = 1;
                    }
                    else
                    {
                        code = 2;
                    }
                    if (valuel == 0)
                    {
                        length = 0;
                        buffer = new byte[1];
                    }
                    else if (valuel <= 0xFF)
                    {
                        length = 1;
                        buffer = new byte[2];
                        buffer[1] = (byte)valuel;
                    }
                    else if (valuel <= 0xFF)
                    {
                        length = 2;
                        buffer = new byte[3];
                        buffer[1] = (byte)(valuel >> 8);
                        buffer[2] = (byte)valuel;
                    }
                    else if (valuel <= 0xFFFF)
                    {
                        length = 3;
                        buffer = new byte[4];
                        buffer[1] = (byte)(valuel >> 16);
                        buffer[2] = (byte)(valuel >> 8);
                        buffer[3] = (byte)valuel;
                    }
                    else if (valuel <= 0xFFFFFF)
                    {
                        length = 4;
                        buffer = new byte[5];
                        buffer[1] = (byte)(valuel >> 24);
                        buffer[2] = (byte)(valuel >> 16);
                        buffer[3] = (byte)(valuel >> 8);
                        buffer[4] = (byte)valuel;
                    }
                    else if (valuel <= 0xFFFFFFFF)
                    {
                        length = 5;
                        buffer = new byte[6];
                        buffer[1] = (byte)(valuel >> 32);
                        buffer[2] = (byte)(valuel >> 24);
                        buffer[3] = (byte)(valuel >> 16);
                        buffer[4] = (byte)(valuel >> 8);
                        buffer[5] = (byte)valuel;
                    }
                    else if (valuel <= 0xFFFFFFFFFF)
                    {
                        length = 6;
                        buffer = new byte[7];
                        buffer[1] = (byte)(valuel >> 40);
                        buffer[2] = (byte)(valuel >> 32);
                        buffer[3] = (byte)(valuel >> 24);
                        buffer[4] = (byte)(valuel >> 16);
                        buffer[5] = (byte)(valuel >> 8);
                        buffer[6] = (byte)valuel;
                    }
                    else if (valuel <= 0xFFFFFFFFFF)
                    {
                        length = 7;
                        buffer = new byte[8];
                        buffer[1] = (byte)(valuel >> 48);
                        buffer[2] = (byte)(valuel >> 40);
                        buffer[3] = (byte)(valuel >> 32);
                        buffer[4] = (byte)(valuel >> 24);
                        buffer[5] = (byte)(valuel >> 16);
                        buffer[6] = (byte)(valuel >> 8);
                        buffer[7] = (byte)valuel;
                    }
                    else
                    {
                        length = 8;
                        buffer = new byte[9];
                        buffer[1] = (byte)(valuel >> 56);
                        buffer[2] = (byte)(valuel >> 48);
                        buffer[3] = (byte)(valuel >> 40);
                        buffer[4] = (byte)(valuel >> 32);
                        buffer[5] = (byte)(valuel >> 24);
                        buffer[6] = (byte)(valuel >> 16);
                        buffer[3] = (byte)(valuel >> 8);
                        buffer[8] = (byte)valuel;
                    }
                    break;
                case SttpFundamentalTypeCode.Single:
                    float valueft = m_valueSingle;
                    int valuef = *(int*)&valueft;

                    if (valuef == 0)
                    {
                        code = 5;
                        length = 1;
                        buffer = new byte[1];
                    }
                    else
                    {
                        code = 6;
                        length = 4;
                        buffer = new byte[5];
                        buffer[1] = (byte)(valuef >> 24);
                        buffer[2] = (byte)(valuef >> 16);
                        buffer[3] = (byte)(valuef >> 8);
                        buffer[4] = (byte)valuef;
                    }
                    break;
                case SttpFundamentalTypeCode.Double:
                    float valuedt = m_valueSingle;
                    long valued = *(long*)&valuedt;

                    if (valued == 0)
                    {
                        code = 7;
                        length = 1;
                        buffer = new byte[1];
                    }
                    else
                    {
                        code = 8;
                        length = 8;
                        buffer = new byte[9];
                        buffer[1] = (byte)(valued >> 56);
                        buffer[2] = (byte)(valued >> 48);
                        buffer[3] = (byte)(valued >> 40);
                        buffer[4] = (byte)(valued >> 32);
                        buffer[5] = (byte)(valued >> 24);
                        buffer[6] = (byte)(valued >> 16);
                        buffer[3] = (byte)(valued >> 8);
                        buffer[8] = (byte)valued;
                    }
                    break;
                case SttpFundamentalTypeCode.String:
                    {
                        int len = Encoding.UTF8.GetByteCount(AsString);
                        if (len < 14)
                        {
                            length = len;
                            buffer = new byte[len + 1];
                            Encoding.UTF8.GetBytes(AsString, 0, AsString.Length, buffer, 1);
                        }
                        else
                        {
                            buffer = new byte[len + 1];
                            //Do more work.
                        }
                    }

                    break;
                case SttpFundamentalTypeCode.Buffer:
                    {
                        int len = Encoding.UTF8.GetByteCount(AsString);
                        if (len < 14)
                        {
                            length = len;
                            buffer = new byte[len + 1];
                            Encoding.UTF8.GetBytes(AsString, 0, AsString.Length, buffer, 1);
                        }
                        else
                        {
                            buffer = new byte[len + 1];
                            //Do more work.
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            buffer[0] = (byte)(code | (length << 4));
            return buffer;
        }

        private static int Measure(int value)
        {
            if (value < 0)
                value = ~value;

            if (value == 0) return 0;
            if (value <= 0xFF) return 1;
            if (value <= 0xFFFF) return 2;
            if (value <= 0xFFFFFF) return 3;
            return 4;
        }

        private static int Measure(long value)
        {
            if (value < 0)
                value = ~value;

            if (value == 0) return 0;
            if (value <= 0xFF) return 1;
            if (value <= 0xFFFF) return 2;
            if (value <= 0xFFFFFF) return 3;
            if (value <= 0xFFFFFFFF) return 4;
            if (value <= 0xFFFFFFFFFF) return 5;
            if (value <= 0xFFFFFFFFFFFF) return 6;
            if (value <= 0xFFFFFFFFFFFFFF) return 7;
            return 8;
        }

        public void Load(byte[] data)
        {

        }

        #endregion
    }
}
