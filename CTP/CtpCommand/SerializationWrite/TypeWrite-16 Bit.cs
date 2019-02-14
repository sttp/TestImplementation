using System;
using CTP;

namespace CTP.SerializationWrite
{
    internal class TypeWriteUInt16
        : TypeWriteMethodBase<ushort>
    {
        private int m_recordName;
        public TypeWriteUInt16(int recordName)
        {
            m_recordName = recordName;
        }
        public override void Save(ushort obj, CtpCommandWriter writer)
        {
            writer.WriteValue(m_recordName, (CtpObject)obj);
        }
    }

    internal class TypeWriteInt16
        : TypeWriteMethodBase<short>
    {
        private int m_recordName;
        public TypeWriteInt16(int recordName)
        {
            m_recordName = recordName;
        }
        public override void Save(short obj, CtpCommandWriter writer)
        {
            writer.WriteValue(m_recordName, (CtpObject)obj);
        }
    }

    internal class TypeWriteChar
        : TypeWriteMethodBase<char>
    {
        private int m_recordName;
        public TypeWriteChar(int recordName)
        {
            m_recordName = recordName;
        }
        public override void Save(char obj, CtpCommandWriter writer)
        {
            writer.WriteValue(m_recordName, (CtpObject)obj);
        }
    }
}