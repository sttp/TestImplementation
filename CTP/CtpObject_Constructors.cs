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
            ValueTypeCode = CtpTypeCode.Integer;
            UnsafeInteger = value;
        }

        private CtpObject(float value)
          : this()
        {
            ValueTypeCode = CtpTypeCode.Single;
            UnsafeSingle = value;
        }

        private CtpObject(double value)
          : this()
        {
            ValueTypeCode = CtpTypeCode.Double;
            UnsafeDouble = value;
        }

        private CtpObject(CtpNumeric value)
            : this()
        {
            ValueTypeCode = CtpTypeCode.Numeric;
            UnsafeNumeric = value;
        }

        private CtpObject(CtpTime value)
          : this()
        {
            ValueTypeCode = CtpTypeCode.CtpTime;
            UnsafeCtpTime = value;
        }

        private CtpObject(bool value)
          : this()
        {
            ValueTypeCode = CtpTypeCode.Boolean;
            UnsafeBoolean = value;
        }

        private CtpObject(Guid value)
          : this()
        {
            ValueTypeCode = CtpTypeCode.Guid;
            UnsafeGuid = value;
        }

        private CtpObject(string value)
          : this()
        {
            ValueTypeCode = CtpTypeCode.String;
            m_valueObject = value;
        }

        private CtpObject(CtpBuffer value)
          : this()
        {
            ValueTypeCode = CtpTypeCode.CtpBuffer;
            m_valueObject = value;
        }

        private CtpObject(CtpCommand value)
          : this()
        {
            ValueTypeCode = CtpTypeCode.CtpCommand;
            m_valueObject = value;
        }

    }
}
