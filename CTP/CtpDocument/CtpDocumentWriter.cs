using System;
using System.Collections.Generic;
using GSF;

namespace CTP
{
    /// <summary>
    /// A user friendly means of writing a <see cref="CtpDocument"/>.
    /// </summary>
    public class CtpDocumentWriter
    {
        //Encoding Scheme: 
        //
        // First, determine the Node Type.
        // 2 bits, CtpDocumentNodeType
        // If EndElement, then exit.
        //
        // Second, Determine the next NameIndex
        // 0: Next NameIndex is same as the last time it was encountered.
        // 1: It's not, Next index is 8 bit encoded number.
        // Third, If NodeType:Value, write the value.

        /// <summary>
        /// A helper class so calls to <see cref="EndElement"/> can be wrapped in a using clause.
        /// Note: this class is a single instance class and does not protect against multiple calls to Dispose. 
        /// Therefore, it's not intended to be used outside of making it easier to section out code.
        /// </summary>
        private class ElementEndElementHelper : IDisposable
        {
            private CtpDocumentWriter m_parent;
            public ElementEndElementHelper(CtpDocumentWriter parent)
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
        private Dictionary<string, int> m_elementNamesLookup;
        private Dictionary<string, int> m_valueNamesLookup;
        /// <summary>
        /// A list of all names and the state data associated with these names.
        /// </summary>
        private List<string> m_elementNames;
        private List<string> m_valueNames;
        /// <summary>
        /// The list of elements so an error can occur when the element tree is invalid..
        /// </summary>
        private Stack<string> m_elementStack;
        /// <summary>
        /// Where to write the data.
        /// </summary>
        private CtpDocumentBitWriter m_stream;
        /// <summary>
        /// A reusable class to ease in calling the <see cref="EndElement"/> method.
        /// </summary>
        private ElementEndElementHelper m_endElementHelper;
        /// <summary>
        /// A temporary value so this class can support setting from an object type.
        /// </summary>
        private CtpObject m_tmpValue;
        private int m_prefixLength;
        private string m_rootElement;

        /// <summary>
        /// Create a new writer with the provided root element.
        /// </summary>
        /// <param name="rootElement"></param>
        public CtpDocumentWriter(string rootElement)
        {
            m_prefixLength = 4;
            m_tmpValue = new CtpObject();
            m_elementNamesLookup = new Dictionary<string, int>();
            m_valueNamesLookup = new Dictionary<string, int>();
            m_elementNames = new List<string>();
            m_valueNames = new List<string>();
            m_elementStack = new Stack<string>();
            m_stream = new CtpDocumentBitWriter();
            m_endElementHelper = new ElementEndElementHelper(this);
            m_rootElement = rootElement;
            m_prefixLength += m_rootElement.Length + 1;
        }

        /// <summary>
        /// The approximate current size of the writer. It's not exact until <see cref="ToCtpDocument"/> has been called.
        /// </summary>
        public int Length => m_stream.Length + m_prefixLength;

        /// <summary>
        /// Resets a document writer so it can be reused.
        /// </summary>
        /// <param name="rootElement"></param>
        public void Reset(string rootElement)
        {
            m_prefixLength = 4;
            m_elementNamesLookup.Clear();
            m_elementNames.Clear();
            m_elementStack.Clear();
            m_stream.Clear();
            m_rootElement = rootElement;
            m_prefixLength += m_rootElement.Length + 1;
        }

        /// <summary>
        /// Starts a new element with the specified name. 
        /// </summary>
        /// <param name="name">The name of the element. This name must conform to 7-bit ASCII and may not exceed 255 characters in length.</param>
        /// <returns>An object that can be used in a using block to make the code cleaner. Disposing this object will call <see cref="EndElement"/></returns>
        public IDisposable StartElement(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            m_elementStack.Push(name);
            m_stream.Write7BitInt((GetElementNameIndex(name) << 4) + (uint)CtpDocumentHeader.StartElement);
            return m_endElementHelper;
        }

        /// <summary>
        /// Ends the current element. This should not be called if <see cref="StartElement"/> is inside a using block, since 
        /// this will automatically be called when exiting the using block.
        /// </summary>
        public void EndElement()
        {
            if (m_elementStack.Count == 0)
                throw new InvalidOperationException("Too many calls to EndElement has occurred. There are no elements to end.");

            m_elementStack.Pop();
            m_stream.Write7BitInt((uint)CtpDocumentHeader.EndElement);
        }

        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="name">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value</param>
        public void WriteValue(string name, CtpObject value)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            if ((object)value == null)
                value = CtpObject.Null;

            m_stream.Write7BitInt((GetValueNameIndex(name) << 4) + (byte)value.ValueTypeCode);
            switch (value.ValueTypeCode)
            {
                case CtpTypeCode.Null:
                    break;
                case CtpTypeCode.Int64:
                    m_stream.Write7BitInt((ulong)PackSign(value.IsInt64));
                    break;
                case CtpTypeCode.Single:
                    m_stream.Write(value.IsSingle);
                    break;
                case CtpTypeCode.Double:
                    m_stream.Write(value.IsDouble);
                    break;
                case CtpTypeCode.CtpTime:
                    m_stream.Write(value.IsCtpTime.Ticks);
                    break;
                case CtpTypeCode.Boolean:
                    m_stream.Write(value.IsBoolean);
                    break;
                case CtpTypeCode.Guid:
                    m_stream.Write(value.IsGuid);
                    break;
                case CtpTypeCode.String:
                    m_stream.Write(value.IsString);
                    break;
                case CtpTypeCode.CtpBuffer:
                    m_stream.Write(value.IsCtpBuffer);
                    break;
                case CtpTypeCode.CtpDocument:
                    m_stream.Write(value.IsCtpDocument);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private uint GetElementNameIndex(string name)
        {
            if (!m_elementNamesLookup.TryGetValue(name, out int index))
            {
                m_elementNamesLookup[name] = m_elementNamesLookup.Count;
                m_elementNames.Add(name);
                index = m_elementNamesLookup.Count - 1;
                m_prefixLength += name.Length + 1;
            }
            return (uint)index;
        }

        private uint GetValueNameIndex(string name)
        {
            if (!m_valueNamesLookup.TryGetValue(name, out int index))
            {
                m_valueNamesLookup[name] = m_valueNamesLookup.Count;
                m_valueNames.Add(name);
                index = m_valueNamesLookup.Count - 1;
                m_prefixLength += name.Length + 1;
            }
            return (uint)index;
        }

        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="name">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value itself.</param>
        public void WriteValue(string name, object value)
        {
            m_tmpValue.SetValue(value);
            WriteValue(name, m_tmpValue);
        }

        /// <summary>
        /// Completes the writing to an <see cref="CtpDocument"/> and returns the completed buffer. This may be called multiple times.
        /// </summary>
        /// <returns></returns>
        public CtpDocument ToCtpDocument()
        {
            if (m_elementStack.Count != 0)
                throw new InvalidOperationException("The element stack does not return to the root. Be sure enough calls to EndElement exist.");

            byte[] rv = new byte[Length];
            CopyTo(rv,0);
            return new CtpDocument(rv);
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

            buffer.ValidateParameters(offset, Length);

            WriteAscii(buffer, ref offset, m_rootElement);
            WriteSize(buffer, ref offset, (ushort)m_elementNames.Count);
            WriteSize(buffer, ref offset, (ushort)m_valueNames.Count);
            foreach (var item in m_elementNames)
            {
                WriteAscii(buffer, ref offset, item);
            }
            foreach (var item in m_valueNames)
            {
                WriteAscii(buffer, ref offset, item);
            }

            m_stream.CopyTo(buffer, offset);
        }

        private static long PackSign(long value)
        {
            //since negative signed values have leading 1's and positive have leading 0's, 
            //it's important to change it into a common format.
            //Basically, we rotate left to move the leading sign bit to bit0, and if bit 0 is set, we invert bits 1-63.
            if (value >= 0)
                return value << 1;
            return (~value << 1) + 1;
        }

        private void WriteSize(byte[] buffer, ref int length, ushort value)
        {
            buffer[length] = (byte)(value >> 8);
            length++;
            buffer[length] = (byte)(value);
            length++;
        }

        private void WriteAscii(byte[] buffer, ref int length, string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (value.Length > 255)
                throw new Exception("Length of string cannot exceed 255 characters");

            buffer[length] = (byte)value.Length;
            length++;
            for (var x = 0; x < value.Length; x++)
            {
                if ((ushort)value[x] > 127) //casting to ushort also takes care of negative numbers if they exist.
                    throw new Exception("Not an ASCII string");
                buffer[length] = (byte)value[x];
                length++;
            }
        }

    }
}