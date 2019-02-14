using System;
using CTP;

namespace CTP.SerializationWrite
{
    internal class TypeWriteUInt8
        : TypeWriteMethodBase<byte>
    {
        private int m_recordName;
        public TypeWriteUInt8(int recordName)
        {
            m_recordName = recordName;
        }
        public override void Save(byte obj, CtpCommandWriter writer)
        {
            writer.WriteValue(m_recordName, (CtpObject)obj);
        }
    }

    internal class TypeWriteInt8
        : TypeWriteMethodBase<sbyte>
    {
        private int m_recordName;
        public TypeWriteInt8(int recordName)
        {
            m_recordName = recordName;
        }
        public override void Save(sbyte obj, CtpCommandWriter writer)
        {
            writer.WriteValue(m_recordName, (CtpObject)obj);
        }
    }

    internal class TypeWriteBool
        : TypeWriteMethodBase<bool>
    {
        private int m_recordName;
        public TypeWriteBool(int recordName)
        {
            m_recordName = recordName;
        }
        public override void Save(bool obj, CtpCommandWriter writer)
        {
            writer.WriteValue(m_recordName, (CtpObject)obj);
        }
    }
}