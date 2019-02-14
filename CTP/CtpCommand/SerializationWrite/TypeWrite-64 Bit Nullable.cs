using System;
using CTP;

namespace CTP.SerializationWrite
{
    internal class TypeWriteUInt64Null
        : TypeWriteMethodBase<ulong?>
    {
        private int m_recordName;
        public TypeWriteUInt64Null(int recordName)
        {
            m_recordName = recordName;
        }
        public override void Save(ulong? obj, CtpCommandWriter writer)
        {
            writer.WriteValue(m_recordName, (CtpObject)obj);
        }
    }

    internal class TypeWriteInt64Null
        : TypeWriteMethodBase<long?>
    {
        private int m_recordName;
        public TypeWriteInt64Null(int recordName)
        {
            m_recordName = recordName;
        }
        public override void Save(long? obj, CtpCommandWriter writer)
        {
            writer.WriteValue(m_recordName, (CtpObject)obj);
        }
    }

    internal class TypeWriteDoubleNull
        : TypeWriteMethodBase<double?>
    {
        private int m_recordName;
        public TypeWriteDoubleNull(int recordName)
        {
            m_recordName = recordName;
        }
        public override void Save(double? obj, CtpCommandWriter writer)
        {
            writer.WriteValue(m_recordName, (CtpObject)obj);
        }
    }
  
    internal class TypeWriteDateTimeNull
        : TypeWriteMethodBase<DateTime?>
    {
        private int m_recordName;
        public TypeWriteDateTimeNull(int recordName)
        {
            m_recordName = recordName;
        }
        public override void Save(DateTime? obj, CtpCommandWriter writer)
        {
            writer.WriteValue(m_recordName, (CtpObject)obj);
        }
    }

    internal class TypeWriteCtpTimeNull
        : TypeWriteMethodBase<CtpTime?>
    {
        private int m_recordName;
        public TypeWriteCtpTimeNull(int recordName)
        {
            m_recordName = recordName;
        }
        public override void Save(CtpTime? obj, CtpCommandWriter writer)
        {
            writer.WriteValue(m_recordName, (CtpObject)obj);
        }
    }
}