using System;
using CTP;

namespace CTP.SerializationWrite
{
    internal class TypeWriteUInt16Null
        : TypeWriteMethodBase<ushort?>
    {
        private int m_recordName;
        public TypeWriteUInt16Null(int recordName)
        {
            m_recordName = recordName;
        }
        public override void Save(ushort? obj, CtpCommandWriter writer)
        {
            writer.WriteValue(m_recordName, (CtpObject)obj);
        }
    }

    internal class TypeWriteInt16Null
        : TypeWriteMethodBase<short?>
    {
        private int m_recordName;
        public TypeWriteInt16Null(int recordName)
        {
            m_recordName = recordName;
        }
        public override void Save(short? obj, CtpCommandWriter writer)
        {
            writer.WriteValue(m_recordName, (CtpObject)obj);
        }
    }

    internal class TypeWriteCharNull
        : TypeWriteMethodBase<char?>
    {
        private int m_recordName;
        public TypeWriteCharNull(int recordName)
        {
            m_recordName = recordName;
        }
        public override void Save(char? obj, CtpCommandWriter writer)
        {
            writer.WriteValue(m_recordName, (CtpObject)obj);
        }
    }
}