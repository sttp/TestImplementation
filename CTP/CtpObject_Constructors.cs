using System;
using System.Runtime.InteropServices;

namespace CTP
{
    /// <summary>
    /// This class contains the fundamental value for STTP.
    /// </summary>
    public partial struct CtpObject
    {
        private CtpObject(long value)
          : this()
        {
            m_valueTypeCode = CtpTypeCode.Int64;
            m_valueInt64 = value;
        }

        private CtpObject(float value)
          : this()
        {
            m_valueTypeCode = CtpTypeCode.Single;
            m_valueSingle = value;
        }

        private CtpObject(double value)
          : this()
        {
            m_valueTypeCode = CtpTypeCode.Double;
            m_valueDouble = value;
        }

        private CtpObject(CtpTime value)
          : this()
        {
            m_valueTypeCode = CtpTypeCode.CtpTime;
            m_valueCtpTime = value;
        }

        private CtpObject(bool value)
          : this()
        {
            m_valueTypeCode = CtpTypeCode.Boolean;
            m_valueBoolean = value;
        }

        private CtpObject(Guid value)
          : this()
        {
            m_valueTypeCode = CtpTypeCode.Guid;
            m_valueGuid = value;
        }

        private CtpObject(string value)
          : this()
        {
            m_valueTypeCode = CtpTypeCode.String;
            m_valueObject = value;
        }

        private CtpObject(CtpBuffer value)
          : this()
        {
            m_valueTypeCode = CtpTypeCode.CtpBuffer;
            m_valueObject = value;
        }

        private CtpObject(CtpCommand value)
          : this()
        {
            m_valueTypeCode = CtpTypeCode.CtpCommand;
            m_valueObject = value;
        }

    }
}
