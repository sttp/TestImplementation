using System;
using CTP;

namespace CTP.SerializationWrite
{
    internal class TypeWriteUInt32
        : TypeWriteMethodBase<uint>
    {
        private int m_recordName;
        public TypeWriteUInt32(int recordName)
        {
            m_recordName = recordName;
        }
        public override void Save(uint obj, CtpCommandWriter writer)
        {
            writer.WriteValue(m_recordName, (CtpObject)obj);
        }
    }

    internal class TypeWriteInt32
        : TypeWriteMethodBase<int>
    {
        private int m_recordName;
        public TypeWriteInt32(int recordName)
        {
            m_recordName = recordName;
        }
        public override void Save(int obj, CtpCommandWriter writer)
        {
            writer.WriteValue(m_recordName, (CtpObject)obj);
        }
    }

    internal class TypeWriteSingle
        : TypeWriteMethodBase<float>
    {
        private int m_recordName;
        public TypeWriteSingle(int recordName)
        {
            m_recordName = recordName;
        }
        public override void Save(float obj, CtpCommandWriter writer)
        {
            writer.WriteValue(m_recordName, (CtpObject)obj);
        }
    }
}