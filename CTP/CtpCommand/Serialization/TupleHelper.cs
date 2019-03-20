using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSF.IO;

namespace CTP.Serialization
{
    internal class TupleHelper<T1, T2>
    {
        private string m_recordName;
        private string m_item1Name;
        private string m_item2Name;
        private TypeIOMethodBase<T1> m_serializeT1;
        private TypeIOMethodBase<T2> m_serializeT2;

        public TupleHelper(string recordName, string item1Name, string item2Name)
        {
            m_recordName = recordName;
            m_item1Name = item1Name;
            m_item2Name = item2Name;
            m_serializeT1 = TypeIO.Create<T1>(item1Name);
            m_serializeT2 = TypeIO.Create<T2>(item2Name);
        }

        public void WriteSchema(CommandSchemaWriter schema)
        {
            schema.StartElement(m_recordName);
            m_serializeT1.WriteSchema(schema);
            m_serializeT2.WriteSchema(schema);
            schema.EndElement();
        }

        public void Load(CtpCommandReader reader, out bool isNull, out T1 v1, out T2 v2)
        {
            isNull = reader.IsElementOrArrayNull;
            v1 = default(T1);
            v2 = default(T2);

            if (reader.IsElementOrArrayNull)
            {
                reader.Read();
                return;
            }

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case CommandSchemaSymbol.StartElement:
                    case CommandSchemaSymbol.StartArray:
                        if (reader.ElementName == m_item1Name)
                        {
                            v1 = m_serializeT1.Load(reader);
                        }
                        else if (reader.ElementName == m_item2Name)
                        {
                            v2 = m_serializeT2.Load(reader);
                        }
                        else
                        {
                            throw new Exception("Parsing Error");
                        }
                        break;
                    case CommandSchemaSymbol.Value:
                        if (reader.ValueName == m_item1Name)
                        {
                            v1 = m_serializeT1.Load(reader);
                        }
                        else if (reader.ValueName == m_item2Name)
                        {
                            v2 = m_serializeT2.Load(reader);
                        }
                        else
                        {
                            throw new Exception("Parsing Error");
                        }
                        break;
                    case CommandSchemaSymbol.EndElement:
                        return;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            throw new Exception("Parsing Error");

        }

        public void Save(bool isNull, T1 item1, T2 item2, CtpObjectWriter writer)
        {
            if (isNull)
            {
                writer.Write(false);
            }
            else
            {
                writer.Write(true);
                m_serializeT1.Save(item1, writer);
                m_serializeT2.Save(item2, writer);
            }
        }



    }
}
