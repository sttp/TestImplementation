using System;
using System.Threading;
using CTP;
using GSF;

namespace CTP.SerializationWrite
{
    internal class TypeWriteDecimalNull
        : TypeWriteMethodBase<decimal?>
    {
        private int m_recordName;
        public TypeWriteDecimalNull(int recordName)
        {
            m_recordName = recordName;
        }
        public override void Save(decimal? obj, CtpCommandWriter writer)
        {
            writer.WriteValue(m_recordName, (CtpObject)obj);
        }
    }

    internal class TypeWriteGuidNull
        : TypeWriteMethodBase<Guid?>
    {
        private int m_recordName;
        public TypeWriteGuidNull(int recordName)
        {
            m_recordName = recordName;
        }
        public override void Save(Guid? obj, CtpCommandWriter writer)
        {
            writer.WriteValue(m_recordName, (CtpObject)obj);
        }
    }
}