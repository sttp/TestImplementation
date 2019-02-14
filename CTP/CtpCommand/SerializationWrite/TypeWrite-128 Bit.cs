using System;
using CTP;
using GSF;

namespace CTP.SerializationWrite
{
    internal class TypeWriteDecimal
        : TypeWriteMethodBase<decimal>
    {
        private int m_recordName;
        public TypeWriteDecimal(int recordName)
        {
            m_recordName = recordName;
        }
        public override void Save(decimal obj, CtpCommandWriter writer)
        {
            writer.WriteValue(m_recordName, (CtpObject)obj);
        }
    }

    internal class TypeWriteGuid
        : TypeWriteMethodBase<Guid>
    {
        private int m_recordName;
        public TypeWriteGuid(int recordName)
        {
            m_recordName = recordName;
        }
        public override void Save(Guid obj, CtpCommandWriter writer)
        {
            writer.WriteValue(m_recordName, (CtpObject)obj);
        }
    }
}