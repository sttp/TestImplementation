using System;
using CTP;

namespace CTP.SerializationWrite
{
    internal class TypeWriteUInt64
        : TypeWriteMethodBase<ulong>
    {
        private int m_recordName;
        public TypeWriteUInt64(int recordName)
        {
            m_recordName = recordName;
        }
        public override void Save(ulong obj, CtpCommandWriter writer)
        {
            writer.WriteValue(m_recordName, (CtpObject)obj);
        }
    }

    internal class TypeWriteInt64
        : TypeWriteMethodBase<long>
    {
        private int m_recordName;
        public TypeWriteInt64(int recordName)
        {
            m_recordName = recordName;
        }
        public override void Save(long obj, CtpCommandWriter writer)
        {
            writer.WriteValue(m_recordName, (CtpObject)obj);
        }
    }

    internal class TypeWriteDouble
        : TypeWriteMethodBase<double>
    {
        private int m_recordName;
        public TypeWriteDouble(int recordName)
        {
            m_recordName = recordName;
        }
        public override void Save(double obj, CtpCommandWriter writer)
        {
            writer.WriteValue(m_recordName, (CtpObject)obj);
        }
    }
  
    internal class TypeWriteDateTime
        : TypeWriteMethodBase<DateTime>
    {
        private int m_recordName;
        public TypeWriteDateTime(int recordName)
        {
            m_recordName = recordName;
        }
        public override void Save(DateTime obj, CtpCommandWriter writer)
        {
            writer.WriteValue(m_recordName, (CtpObject)obj);
        }
    }

    internal class TypeWriteCtpTime
        : TypeWriteMethodBase<CtpTime>
    {
        private int m_recordName;
        public TypeWriteCtpTime(int recordName)
        {
            m_recordName = recordName;
        }
        public override void Save(CtpTime obj, CtpCommandWriter writer)
        {
            writer.WriteValue(m_recordName, (CtpObject)obj);
        }
    }
}