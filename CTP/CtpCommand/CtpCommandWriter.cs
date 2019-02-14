using System;
using System.Collections.Generic;
using GSF;

namespace CTP
{
    /// <summary>
    /// A user friendly means of writing a <see cref="CtpCommand"/>.
    /// </summary>
    internal class CtpCommandWriter
    {
        /// <summary>
        /// A helper class so calls to <see cref="EndElement"/> can be wrapped in a using clause.
        /// Note: this class is a single instance class and does not protect against multiple calls to Dispose. 
        /// Therefore, it's not intended to be used outside of making it easier to section out code.
        /// </summary>
        private class ElementEndElementHelper : IDisposable
        {
            private CtpCommandWriter m_parent;
            public ElementEndElementHelper(CtpCommandWriter parent)
            {
                m_parent = parent;
            }
            public void Dispose()
            {
                m_parent.EndElement();
            }
        }

        /// <summary>
        /// A lookup of all names that have been registered.
        /// </summary>
        private RuntimeMapping m_namesLookup;
        /// <summary>
        /// A list of all names and the state data associated with these names.
        /// </summary>
        private List<CtpCommandKeyword> m_names;
        /// <summary>
        /// The list of elements so an error can occur when the element tree is invalid.
        /// </summary>
        private Stack<bool> m_elementStack;
        /// <summary>
        /// Where to write the data.
        /// </summary>
        private ByteWriter m_stream;
        /// <summary>
        /// A reusable class to ease in calling the <see cref="EndElement"/> method.
        /// </summary>
        private ElementEndElementHelper m_endElementHelper;
        /// <summary>
        /// A temporary value so this class can support setting from an object type.
        /// </summary>
        private int m_prefixLength;

        private bool m_isArray;

        private CtpCommandKeyword m_rootElement;

        private SerializationSchema m_writeSchema;

        /// <summary>
        /// Create a new writer with the provided root element.
        /// </summary>
        public CtpCommandWriter()
        {
            //2 byte header, 2 byte for NamesCount
            m_prefixLength = 4;
            m_namesLookup = new RuntimeMapping();
            m_names = new List<CtpCommandKeyword>();
            m_elementStack = new Stack<bool>();
            m_stream = new ByteWriter();
            m_endElementHelper = new ElementEndElementHelper(this);
            m_isArray = false;
        }

        public void Initialize(SerializationSchema writeSchema, CtpCommandKeyword rootElement)
        {
            //2 byte header, 2 byte for NamesCount
            m_prefixLength = 4;
            m_namesLookup.Clear();
            m_names.Clear();
            m_elementStack.Clear();
            m_stream.Clear();
            m_rootElement = rootElement ?? throw new ArgumentNullException(nameof(rootElement));
            m_prefixLength += m_rootElement.TextWithPrefix.Length;
            m_isArray = false;
            m_writeSchema = writeSchema;
        }

        /// <summary>
        /// The size of the writer if all elements were closed and the data was serialized.
        /// </summary>
        public int Length
        {
            get
            {
                var innerLength = m_stream.ActualSize + m_prefixLength;
                if (innerLength > 4093)
                    return innerLength + 2;
                return innerLength;
            }
        }

        /// <summary>
        /// Starts a new element with the specified name. 
        /// </summary>
        /// <param name="nameID">The name of the element. This name must conform to 7-bit ASCII and may not exceed 255 characters in length.</param>
        /// <param name="isArray"></param>
        /// <returns>An object that can be used in a using block to make the code cleaner. Disposing this object will call <see cref="EndElement"/></returns>
        public IDisposable StartElement(int nameID, bool isArray)
        {
            m_stream.WriteBits1(0);
            m_stream.WriteBits1(0);
            m_stream.WriteBits1(isArray);
            if (!m_isArray)
                m_stream.Write4BitSegments((uint)nameID);
            m_isArray = isArray;
            m_elementStack.Push(isArray);
            return m_endElementHelper;
        }

        /// <summary>
        /// Ends the current element. This should not be called if <see cref="StartElement"/> is inside a using block, since 
        /// this will automatically be called when exiting the using block.
        /// </summary>
        private void EndElement()
        {
            if (m_elementStack.Count == 0)
                throw new InvalidOperationException("Too many calls to EndElement has occurred. There are no elements to end.");
            m_elementStack.Pop();
            m_isArray = m_elementStack.Count != 0 && m_elementStack.Peek();
            m_stream.WriteBits1(0);
            m_stream.WriteBits1(1);
        }

        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="nameID">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value</param>
        public void WriteValue(int nameID, CtpObject value)
        {
            m_stream.WriteBits1(1);
            if (!m_isArray)
                m_stream.Write4BitSegments((uint)nameID);
            m_stream.WriteObject(value);
        }

        private void WriteName(CtpCommandKeyword name)
        {
            if (name.RuntimeID < 0)
                throw new ArgumentException();
            if (!m_namesLookup.TryGetValue(name.RuntimeID, out int index))
            {
                m_namesLookup.Add(name.RuntimeID, m_namesLookup.Count);
                m_names.Add(name);
                index = m_namesLookup.Count - 1;
                m_prefixLength += name.TextWithPrefix.Length;
            }
            m_stream.Write4BitSegments((uint)index);
        }

        /// <summary>
        /// Completes the writing to an <see cref="CtpCommand"/> and returns the completed buffer. This may be called multiple times.
        /// </summary>
        /// <returns></returns>
        public CtpCommand ToCtpCommand()
        {
            if (m_elementStack.Count != 0)
                throw new InvalidOperationException("The element stack does not return to the root. Be sure enough calls to EndElement exist.");

            byte[] rv = new byte[Length];
            CopyTo(rv, 0);
            return CtpCommand.Load(rv, false);
        }

        /// <summary>
        /// Copies the contest of the current document writer to the specified stream.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        public void CopyTo(byte[] buffer, int offset)
        {
            if (m_elementStack.Count != 0)
                throw new InvalidOperationException("The element stack does not return to the root. Be sure enough calls to EndElement exist.");

            var length = Length;
            buffer.ValidateParameters(offset, length);

            if (length <= 4095)
            {
                //This is a 2 byte header;
                WriteSize(buffer, ref offset, (ushort)length);
            }
            else
            {
                //This is a 4 byte header;
                WriteSize(buffer, ref offset, (uint)(length + (1 << 28)));
            }

            Write(buffer, ref offset, m_rootElement);
            WriteSize(buffer, ref offset, (ushort)m_names.Count);
            for (var index = 0; index < m_names.Count; index++)
            {
                Write(buffer, ref offset, m_names[index]);
            }

            m_stream.CopyTo(buffer, offset);
        }

        private void WriteSize(byte[] buffer, ref int length, ushort value)
        {
            buffer[length] = (byte)(value >> 8);
            length++;
            buffer[length] = (byte)(value);
            length++;
        }

        private void WriteSize(byte[] buffer, ref int length, uint value)
        {
            buffer[length] = (byte)(value >> 24);
            length++;
            buffer[length] = (byte)(value >> 16);
            length++;
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