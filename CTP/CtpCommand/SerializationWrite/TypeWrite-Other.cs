using System;
using System.Text;
using CTP;

namespace CTP.SerializationWrite
{
    internal class TypeWriteString
        : TypeWriteMethodBase<string>
    {
        private int m_recordName;
        public TypeWriteString(int recordName)
        {
            m_recordName = recordName;
        }
        public override void Save(string obj, CtpCommandWriter writer)
        {
            writer.WriteValue(m_recordName, (CtpObject)obj);
        }
    }

    internal class TypeWriteByteArray
       : TypeWriteMethodBase<byte[]>
    {
        private int m_recordName;
        public TypeWriteByteArray(int recordName)
        {
            m_recordName = recordName;
        }
        public override void Save(byte[] obj, CtpCommandWriter writer)
        {
            writer.WriteValue(m_recordName, (CtpObject)obj);
        }
    }

    internal class TypeWriteCharArray
        : TypeWriteMethodBase<char[]>
    {
        private int m_recordName;
        public TypeWriteCharArray(int recordName)
        {
            m_recordName = recordName;
        }
        public override void Save(char[] obj, CtpCommandWriter writer)
        {
            writer.WriteValue(m_recordName, (CtpObject)obj);
        }
    }

    internal class TypeWriteCommand
        : TypeWriteMethodBase<CtpCommand>
    {
        private int m_recordName;
        public TypeWriteCommand(int recordName)
        {
            m_recordName = recordName;
        }
        public override void Save(CtpCommand obj, CtpCommandWriter writer)
        {
            writer.WriteValue(m_recordName, (CtpObject)obj);
        }
    }

    internal class TypeWriteBuffer
        : TypeWriteMethodBase<CtpBuffer>
    {
        private int m_recordName;
        public TypeWriteBuffer(int recordName)
        {
            m_recordName = recordName;
        }
        public override void Save(CtpBuffer obj, CtpCommandWriter writer)
        {
            writer.WriteValue(m_recordName, (CtpObject)obj);
        }
    }

    internal class TypeWriteNumeric
        : TypeWriteMethodBase<CtpNumeric>
    {
        private int m_recordName;
        public TypeWriteNumeric(int recordName)
        {
            m_recordName = recordName;
        }
        public override void Save(CtpNumeric obj, CtpCommandWriter writer)
        {
            writer.WriteValue(m_recordName, (CtpObject)obj);
        }
    }

    internal class TypeWriteNumericNull
        : TypeWriteMethodBase<CtpNumeric?>
    {
        private int m_recordName;
        public TypeWriteNumericNull(int recordName)
        {
            m_recordName = recordName;
        }
        public override void Save(CtpNumeric? obj, CtpCommandWriter writer)
        {
            writer.WriteValue(m_recordName, (CtpObject)obj);
        }
    }


    internal class TypeWriteCtpObject
        : TypeWriteMethodBase<CtpObject>
    {
        private int m_recordName;
        public TypeWriteCtpObject(int recordName)
        {
            m_recordName = recordName;
        }
        public override void Save(CtpObject obj, CtpCommandWriter writer)
        {
            writer.WriteValue(m_recordName, obj);
        }
    }

    internal class TypeWriteObject
        : TypeWriteMethodBase<object>
    {
        private int m_recordName;
        public TypeWriteObject(int recordName)
        {
            m_recordName = recordName;
        }
        public override void Save(object obj, CtpCommandWriter writer)
        {
            writer.WriteValue(m_recordName, (CtpObject)obj);
        }
    }
}