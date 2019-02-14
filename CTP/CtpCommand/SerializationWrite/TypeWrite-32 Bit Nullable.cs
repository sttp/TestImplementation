using System;
using CTP;

namespace CTP.SerializationWrite
{
    internal class TypeWriteUInt32Null
        : TypeWriteMethodBase<uint?>
    {
        private int m_recordName;
        public TypeWriteUInt32Null(int recordName)
        {
            m_recordName = recordName;
        }
        public override void Save(uint? obj, CtpCommandWriter writer)
        {
            writer.WriteValue(m_recordName, (CtpObject)obj);
        }
    }

    internal class TypeWriteInt32Null
        : TypeWriteMethodBase<int?>
    {
        private int m_recordName;
        public TypeWriteInt32Null(int recordName)
        {
            m_recordName = recordName;
        }
        public override void Save(int? obj, CtpCommandWriter writer)
        {
            writer.WriteValue(m_recordName, (CtpObject)obj);
        }
    }

    internal class TypeWriteSingleNull
        : TypeWriteMethodBase<float?>
    {
        private int m_recordName;
        public TypeWriteSingleNull(int recordName)
        {
            m_recordName = recordName;
        }
        public override void Save(float? obj, CtpCommandWriter writer)
        {
            writer.WriteValue(m_recordName, (CtpObject)obj);
        }
    }
}