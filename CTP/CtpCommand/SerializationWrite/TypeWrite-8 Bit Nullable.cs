using System;
using CTP;

namespace CTP.SerializationWrite
{
    internal class TypeWriteUInt8Null
        : TypeWriteMethodBase<byte?>
    {
        private int m_recordName;
        public TypeWriteUInt8Null(int recordName)
        {
            m_recordName = recordName;
        }
        public override void Save(byte? obj, CtpCommandWriter writer)
        {
            writer.WriteValue(m_recordName, (CtpObject)obj);
        }
    }

    internal class TypeWriteInt8Null
        : TypeWriteMethodBase<sbyte?>
    {
        private int m_recordName;
        public TypeWriteInt8Null(int recordName)
        {
            m_recordName = recordName;
        }
        public override void Save(sbyte? obj, CtpCommandWriter writer)
        {
            writer.WriteValue(m_recordName, (CtpObject)obj);
        }
    }

    internal class TypeWriteBoolNull
        : TypeWriteMethodBase<bool?>
    {
        private int m_recordName;
        public TypeWriteBoolNull(int recordName)
        {
            m_recordName = recordName;
        }
        public override void Save(bool? obj, CtpCommandWriter writer)
        {
            writer.WriteValue(m_recordName, (CtpObject)obj);
        }
    }
}