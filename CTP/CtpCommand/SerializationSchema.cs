using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTP
{
    internal class SerializationSchema
    {
        private int m_length;
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
            //2 byte header, 2 byte for NamesCount
            m_length = 4;
            m_namesLookup = new RuntimeMapping();
            m_names = new List<CtpCommandKeyword>();
            m_rootElement = rootElement ?? throw new ArgumentNullException(nameof(rootElement));
            m_length += m_rootElement.TextWithPrefix.Length;
        }

        public int WriteName(CtpCommandKeyword name)
        {
            if (name.RuntimeID < 0)
                throw new ArgumentException();
            if (!m_namesLookup.TryGetValue(name.RuntimeID, out int index))
            {
                m_namesLookup.Add(name.RuntimeID, m_namesLookup.Count);
                m_names.Add(name);
                index = m_namesLookup.Count - 1;
                m_length += name.TextWithPrefix.Length;
            }
            return index;
        }

    }
}
