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
        private RuntimeMapping m_elementNamesLookup;
        private RuntimeMapping m_valueNamesLookup;
        /// <summary>
        /// A list of all names and the state data associated with these names.
        /// </summary>
        private List<CtpCommandKeyword> m_elementNames;
        private List<CtpCommandKeyword> m_valueNames;
        /// <summary>
        /// The list of elements so an error can occur when the element tree is invalid..
        /// </summary>
        private Stack<string> m_elementStack;
        /// <summary>
        /// Where to write the data.
        /// </summary>
        private CtpCommandBitWriter m_stream;
        /// <summary>
        /// A reusable class to ease in calling the <see cref="EndElement"/> method.
        /// </summary>
        private ElementEndElementHelper m_endElementHelper;
        /// <summary>
        /// A temporary value so this class can support setting from an object type.
        /// </summary>
        private CtpObject m_tmpValue;
        private int m_prefixLength;
        private CtpCommandKeyword m_rootElement;

        /// <summary>
        /// Create a new writer with the provided root element.
        /// </summary>
        public CtpCommandWriter()
        {
            m_prefixLength = 6;
            m_tmpValue = new CtpObject();
            m_elementNamesLookup = new RuntimeMapping();
            m_valueNamesLookup = new RuntimeMapping();
            m_elementNames = new List<CtpCommandKeyword>();
            m_valueNames = new List<CtpCommandKeyword>();
            m_elementStack = new Stack<string>();
            m_stream = new CtpCommandBitWriter();
            m_endElementHelper = new ElementEndElementHelper(this);
        }

        public void Initialize(CtpCommandKeyword rootElement)
        {
            m_prefixLength = 6;
            m_elementNamesLookup.Clear();
            m_valueNamesLookup.Clear();
            m_elementNames.Clear();
            m_valueNames.Clear();
            m_elementStack.Clear();
            m_stream.Clear();
            m_rootElement = rootElement ?? throw new ArgumentNullException(nameof(rootElement));
            m_prefixLength += m_rootElement.TextWithPrefix.Length;
        }

        /// <summary>
        /// The size of the writer if all elements were closed and the data was serialized.
        /// </summary>
        public int Length
        {
            get
            {
                var innerLength = m_stream.Length + m_prefixLength + m_elementStack.Count;
                if (innerLength > 4093)
                    return innerLength + 2;
                return innerLength;
            }
        }

        /// <summary>
        /// Starts a new element with the specified name. 
        /// </summary>
        /// <param name="name">The name of the element. This name must conform to 7-bit ASCII and may not exceed 255 characters in length.</param>
        /// <returns>An object that can be used in a using block to make the code cleaner. Disposing this object will call <see cref="EndElement"/></returns>
        public IDisposable StartElement(CtpCommandKeyword name)
        {
            m_stream.Write7BitInt((GetElementNameIndex(name) << 4) + (uint)CtpCommandHeader.StartElement);
            m_elementStack.Push(name.Value);
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
            m_stream.Write7BitInt((uint)CtpCommandHeader.EndElement);
        }

        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="name">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value</param>
        public void WriteValue(CtpCommandKeyword name, CtpObject value)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if ((object)value == null)
                value = CtpObject.Null;

            switch (value.ValueTypeCode)
            {
                case CtpTypeCode.Null:
                    m_stream.Write7BitInt((GetValueNameIndex(name) << 4) + (byte)CtpCommandHeader.ValueNull);
                    break;
                case CtpTypeCode.Int64:
                    if (value.IsInt64 < 0)
                    {
                        m_stream.Write7BitInt((GetValueNameIndex(name) << 4) + (byte)CtpCommandHeader.ValueInvertedInt64);
                        m_stream.Write7BitInt(~(ulong)value.IsInt64);
                    }
                    else
                    {
                        m_stream.Write7BitInt((GetValueNameIndex(name) << 4) + (byte)CtpCommandHeader.ValueInt64);
                        m_stream.Write7BitInt((ulong)value.IsInt64);
                    }
                    break;
                case CtpTypeCode.Single:
                    m_stream.Write7BitInt((GetValueNameIndex(name) << 4) + (byte)CtpCommandHeader.ValueSingle);
                    m_stream.Write(value.IsSingle);
                    break;
                case CtpTypeCode.Double:
                    m_stream.Write7BitInt((GetValueNameIndex(name) << 4) + (byte)CtpCommandHeader.ValueDouble);
                    m_stream.Write(value.IsDouble);
                    break;
                case CtpTypeCode.CtpTime:
                    m_stream.Write7BitInt((GetValueNameIndex(name) << 4) + (byte)CtpCommandHeader.ValueCtpTime);
                    m_stream.Write(value.IsCtpTime);
                    break;
                case CtpTypeCode.Boolean:
                    if (value.IsBoolean)
                    {
                        m_stream.Write7BitInt((GetValueNameIndex(name) << 4) + (byte)CtpCommandHeader.ValueBooleanTrue);
                    }
                    else
                    {
                        m_stream.Write7BitInt((GetValueNameIndex(name) << 4) + (byte)CtpCommandHeader.ValueBooleanFalse);
                    }
                    break;
                case CtpTypeCode.Guid:
                    m_stream.Write7BitInt((GetValueNameIndex(name) << 4) + (byte)CtpCommandHeader.ValueGuid);
                    m_stream.Write(value.IsGuid);
                    break;
                case CtpTypeCode.String:
                    m_stream.Write7BitInt((GetValueNameIndex(name) << 4) + (byte)CtpCommandHeader.ValueString);
                    m_stream.Write(value.IsString);
                    break;
                case CtpTypeCode.CtpBuffer:
                    m_stream.Write7BitInt((GetValueNameIndex(name) << 4) + (byte)CtpCommandHeader.ValueCtpBuffer);
                    m_stream.Write(value.IsCtpBuffer);
                    break;
                case CtpTypeCode.CtpDocument:
                    m_stream.Write7BitInt((GetValueNameIndex(name) << 4) + (byte)CtpCommandHeader.ValueCtpDocument);
                    m_stream.Write(value.IsCtpCommand);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private uint GetElementNameIndex(CtpCommandKeyword name)
        {
            if (name.RuntimeID < 0)
                throw new ArgumentException();
            if (!m_elementNamesLookup.TryGetValue(name.RuntimeID, out int index))
            {
                m_elementNamesLookup.Add(name.RuntimeID, m_elementNamesLookup.Count);
                m_elementNames.Add(name);
                index = m_elementNamesLookup.Count - 1;
                m_prefixLength += name.TextWithPrefix.Length;
            }
            return (uint)index;
        }

        private uint GetValueNameIndex(CtpCommandKeyword name)
        {
            if (name.RuntimeID < 0)
                throw new ArgumentException();
            if (!m_valueNamesLookup.TryGetValue(name.RuntimeID, out int index))
            {
                m_valueNamesLookup.Add(name.RuntimeID, m_valueNamesLookup.Count);
                m_valueNames.Add(name);
                index = m_valueNamesLookup.Count - 1;
                m_prefixLength += name.TextWithPrefix.Length;
            }
            return (uint)index;
        }

        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="name">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value itself.</param>
        public void WriteValue(CtpCommandKeyword name, object value)
        {
            m_tmpValue.SetValue(value);
            WriteValue(name, m_tmpValue);
        }

        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="name">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value itself.</param>
        public void WriteValue(CtpCommandKeyword name, DBNull value)
        {
            m_tmpValue.SetValue(value);
            WriteValue(name, m_tmpValue);
        }

        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="name">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value itself.</param>
        public void WriteValue(CtpCommandKeyword name, sbyte value)
        {
            m_tmpValue.SetValue(value);
            WriteValue(name, m_tmpValue);
        }

        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="name">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value itself.</param>
        public void WriteValue(CtpCommandKeyword name, short value)
        {
            m_tmpValue.SetValue(value);
            WriteValue(name, m_tmpValue);
        }

        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="name">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value itself.</param>
        public void WriteValue(CtpCommandKeyword name, int value)
        {
            m_tmpValue.SetValue(value);
            WriteValue(name, m_tmpValue);
        }

        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="name">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value itself.</param>
        public void WriteValue(CtpCommandKeyword name, long value)
        {
            m_tmpValue.SetValue(value);
            WriteValue(name, m_tmpValue);
        }

        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="name">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value itself.</param>
        public void WriteValue(CtpCommandKeyword name, byte value)
        {
            m_tmpValue.SetValue(value);
            WriteValue(name, m_tmpValue);
        }

        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="name">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value itself.</param>
        public void WriteValue(CtpCommandKeyword name, ushort value)
        {
            m_tmpValue.SetValue(value);
            WriteValue(name, m_tmpValue);
        }

        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="name">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value itself.</param>
        public void WriteValue(CtpCommandKeyword name, uint value)
        {
            m_tmpValue.SetValue(value);
            WriteValue(name, m_tmpValue);
        }

        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="name">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value itself.</param>
        public void WriteValue(CtpCommandKeyword name, ulong value)
        {
            m_tmpValue.SetValue(value);
            WriteValue(name, m_tmpValue);
        }

        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="name">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value itself.</param>
        public void WriteValue(CtpCommandKeyword name, float value)
        {
            m_tmpValue.SetValue(value);
            WriteValue(name, m_tmpValue);
        }

        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="name">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value itself.</param>
        public void WriteValue(CtpCommandKeyword name, double value)
        {
            m_tmpValue.SetValue(value);
            WriteValue(name, m_tmpValue);
        }

        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="name">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value itself.</param>
        public void WriteValue(CtpCommandKeyword name, decimal value)
        {
            m_tmpValue.SetValue(value);
            WriteValue(name, m_tmpValue);
        }

        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="name">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value itself.</param>
        public void WriteValue(CtpCommandKeyword name, DateTime value)
        {
            m_tmpValue.SetValue(value);
            WriteValue(name, m_tmpValue);
        }

        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="name">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value itself.</param>
        public void WriteValue(CtpCommandKeyword name, CtpTime value)
        {
            m_tmpValue.SetValue(value);
            WriteValue(name, m_tmpValue);
        }

        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="name">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value itself.</param>
        public void WriteValue(CtpCommandKeyword name, bool value)
        {
            m_tmpValue.SetValue(value);
            WriteValue(name, m_tmpValue);
        }

        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="name">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value itself.</param>
        public void WriteValue(CtpCommandKeyword name, Guid value)
        {
            m_tmpValue.SetValue(value);
            WriteValue(name, m_tmpValue);
        }

        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="name">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value itself.</param>
        public void WriteValue(CtpCommandKeyword name, char value)
        {
            m_tmpValue.SetValue(value);
            WriteValue(name, m_tmpValue);
        }

        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="name">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value itself.</param>
        public void WriteValue(CtpCommandKeyword name, char[] value)
        {
            m_tmpValue.SetValue(value);
            WriteValue(name, m_tmpValue);
        }

        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="name">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value itself.</param>
        public void WriteValue(CtpCommandKeyword name, string value)
        {
            m_tmpValue.SetValue(value);
            WriteValue(name, m_tmpValue);
        }

        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="name">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value itself.</param>
        public void WriteValue(CtpCommandKeyword name, CtpBuffer value)
        {
            m_tmpValue.SetValue(value);
            WriteValue(name, m_tmpValue);
        }

        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="name">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value itself.</param>
        public void WriteValue(CtpCommandKeyword name, CtpCommand value)
        {
            m_tmpValue.SetValue(value);
            WriteValue(name, m_tmpValue);
        }

        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="name">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value itself.</param>
        public void WriteValue(CtpCommandKeyword name, byte[] value)
        {
            m_tmpValue.SetValue(value);
            WriteValue(name, m_tmpValue);
        }

        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="name">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value itself.</param>
        public void WriteValue(CtpCommandKeyword name, sbyte? value)
        {
            m_tmpValue.SetValue(value);
            WriteValue(name, m_tmpValue);
        }

        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="name">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value itself.</param>
        public void WriteValue(CtpCommandKeyword name, short? value)
        {
            m_tmpValue.SetValue(value);
            WriteValue(name, m_tmpValue);
        }

        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="name">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value itself.</param>
        public void WriteValue(CtpCommandKeyword name, int? value)
        {
            m_tmpValue.SetValue(value);
            WriteValue(name, m_tmpValue);
        }

        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="name">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value itself.</param>
        public void WriteValue(CtpCommandKeyword name, long? value)
        {
            m_tmpValue.SetValue(value);
            WriteValue(name, m_tmpValue);
        }

        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="name">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value itself.</param>
        public void WriteValue(CtpCommandKeyword name, byte? value)
        {
            m_tmpValue.SetValue(value);
            WriteValue(name, m_tmpValue);
        }

        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="name">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value itself.</param>
        public void WriteValue(CtpCommandKeyword name, ushort? value)
        {
            m_tmpValue.SetValue(value);
            WriteValue(name, m_tmpValue);
        }

        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="name">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value itself.</param>
        public void WriteValue(CtpCommandKeyword name, uint? value)
        {
            m_tmpValue.SetValue(value);
            WriteValue(name, m_tmpValue);
        }

        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="name">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value itself.</param>
        public void WriteValue(CtpCommandKeyword name, ulong? value)
        {
            m_tmpValue.SetValue(value);
            WriteValue(name, m_tmpValue);
        }

        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="name">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value itself.</param>
        public void WriteValue(CtpCommandKeyword name, float? value)
        {
            m_tmpValue.SetValue(value);
            WriteValue(name, m_tmpValue);
        }

        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="name">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value itself.</param>
        public void WriteValue(CtpCommandKeyword name, double? value)
        {
            m_tmpValue.SetValue(value);
            WriteValue(name, m_tmpValue);
        }

        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="name">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value itself.</param>
        public void WriteValue(CtpCommandKeyword name, decimal? value)
        {
            m_tmpValue.SetValue(value);
            WriteValue(name, m_tmpValue);
        }

        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="name">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value itself.</param>
        public void WriteValue(CtpCommandKeyword name, DateTime? value)
        {
            m_tmpValue.SetValue(value);
            WriteValue(name, m_tmpValue);
        }

        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="name">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value itself.</param>
        public void WriteValue(CtpCommandKeyword name, CtpTime? value)
        {
            m_tmpValue.SetValue(value);
            WriteValue(name, m_tmpValue);
        }
        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="name">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value itself.</param>
        public void WriteValue(CtpCommandKeyword name, bool? value)
        {
            m_tmpValue.SetValue(value);
            WriteValue(name, m_tmpValue);
        }
        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="name">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value itself.</param>
        public void WriteValue(CtpCommandKeyword name, Guid? value)
        {
            m_tmpValue.SetValue(value);
            WriteValue(name, m_tmpValue);
        }
        /// <summary>
        /// Writes the provided value.
        /// </summary>
        /// <param name="name">the name of the value. This name must conform to 7-bit ascii and may not exceed 255 characters in length.</param>
        /// <param name="value">the value itself.</param>
        public void WriteValue(CtpCommandKeyword name, char? value)
        {
            m_tmpValue.SetValue(value);
            WriteValue(name, m_tmpValue);
        }


        /// <summary>
        /// Completes the writing to an <see cref="CtpCommand"/> and returns the completed buffer. This may be called multiple times.
        /// </summary>
        /// <returns></returns>
        public CtpCommand ToCtpDocument()
        {
            if (m_elementStack.Count != 0)
                throw new InvalidOperationException("The element stack does not return to the root. Be sure enough calls to EndElement exist.");

            byte[] rv = new byte[Length];
            CopyTo(rv, 0);
            return new CtpCommand(rv, false);
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

            WriteAscii(buffer, ref offset, m_rootElement);
            WriteSize(buffer, ref offset, (ushort)m_elementNames.Count);
            WriteSize(buffer, ref offset, (ushort)m_valueNames.Count);
            for (var index = 0; index < m_elementNames.Count; index++)
            {
                WriteAscii(buffer, ref offset, m_elementNames[index]);
            }

            for (var index = 0; index < m_valueNames.Count; index++)
            {
                WriteAscii(buffer, ref offset, m_valueNames[index]);
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

        private void WriteAscii(byte[] buffer, ref int length, CtpCommandKeyword value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            Array.Copy(value.TextWithPrefix, 0, buffer, length, value.TextWithPrefix.Length);
            length += value.TextWithPrefix.Length;
        }

    }
}