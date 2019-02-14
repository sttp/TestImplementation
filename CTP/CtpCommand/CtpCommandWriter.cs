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

        private bool m_isArray;

        private int m_bitsPerName;

        private SerializationSchema m_writeSchema;

        /// <summary>
        /// Create a new writer with the provided root element.
        /// </summary>
        public CtpCommandWriter(SerializationSchema writeSchema)
        {
            m_writeSchema = writeSchema;
            m_bitsPerName = 32 - BitMath.CountLeadingZeros((uint)writeSchema.NamesCount-1);
            m_elementStack = new Stack<bool>();
            m_stream = new ByteWriter();
            m_endElementHelper = new ElementEndElementHelper(this);
            m_isArray = false;
        }

        /// <summary>
        /// The size of the writer if all elements were closed and the data was serialized.
        /// </summary>
        public int Length
        {
            get
            {
                var innerLength = 2 + m_stream.ActualSize + m_writeSchema.Length;
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
            if (m_elementStack.Count > 30)
                throw new Exception("Element depth cannot exceed 30 nested elements.");
            m_stream.WriteBits1(0);
            m_stream.WriteBits1(0);
            m_stream.WriteBits1(isArray);
            if (!m_isArray)
            {
                if (nameID < 0)
                    throw new ArgumentException();
                m_stream.WriteBits(m_bitsPerName, (uint)nameID);
            }
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
            {
                if (nameID < 0)
                    throw new ArgumentException();
                m_stream.WriteBits(m_bitsPerName, (uint)nameID);
            }
            m_stream.WriteObject(value);
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

            m_writeSchema.CopyTo(buffer, offset);
            offset += m_writeSchema.Length;
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

    }
}