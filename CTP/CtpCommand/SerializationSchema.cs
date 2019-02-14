using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTP
{
    internal class SerializationSchema
    {
        public int Length { get; private set; }
        /// <summary>
        /// A lookup of all names that have been registered.
        /// </summary>
        private RuntimeMapping m_namesLookup;
        /// <summary>
        /// A list of all names and the state data associated with these names.
        /// </summary>
        private List<CtpCommandKeyword> m_names;

        private CtpCommandKeyword m_rootElement;

        public SerializationSchema(CtpCommandKeyword rootElement)
        {
            //2 byte for NamesCount
            Length = 2;
            m_namesLookup = new RuntimeMapping();
            m_names = new List<CtpCommandKeyword>();
            m_rootElement = rootElement ?? throw new ArgumentNullException(nameof(rootElement));
            Length += m_rootElement.TextWithPrefix.Length;
        }

        public int NamesCount => m_names.Count;


        public int WriteName(CtpCommandKeyword name)
        {
            if (name.RuntimeID < 0)
                throw new ArgumentException();
            if (!m_namesLookup.TryGetValue(name.RuntimeID, out int index))
            {
                m_namesLookup.Add(name.RuntimeID, m_namesLookup.Count);
                m_names.Add(name);
                index = m_namesLookup.Count - 1;
                Length += name.TextWithPrefix.Length;
            }
            return index;
        }

        public void CopyTo(byte[] buffer, int offset)
        {
            Write(buffer, ref offset, m_rootElement);
            WriteSize(buffer, ref offset, (ushort)m_names.Count);
            for (var index = 0; index < m_names.Count; index++)
            {
                Write(buffer, ref offset, m_names[index]);
            }
        }

        private void WriteSize(byte[] buffer, ref int length, ushort value)
        {
            buffer[length] = (byte)(value >> 8);
            length++;
            buffer[length] = (byte)(value);
            length++;
        }
      
        private void Write(byte[] buffer, ref int length, CtpCommandKeyword value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            Array.Copy(value.TextWithPrefix, 0, buffer, length, value.TextWithPrefix.Length);
            length += value.TextWithPrefix.Length;
        }

    }
}
