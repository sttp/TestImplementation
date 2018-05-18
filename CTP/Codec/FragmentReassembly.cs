using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GSF.IO.Checksums;

namespace CTP
{
    /// <summary>
    /// This class assists in the reassembly of fragmented data.
    /// </summary>
    internal class FragmentReassembly
    {
        /// <summary>
        /// A buffer for building fragmented packets.
        /// </summary>
        public byte[] Buffer;
        /// <summary>
        /// The compressed size of a fragmented packet.
        /// </summary>
        public int TotalSize;

        /// <summary>
        /// The number of bytes of the fragment that have been received thus far.
        /// </summary>
        public int ReceivedSize;

        private uint m_checksum;

        private int m_totalFragments;
        private int m_prevFragment;

        public int FragmentID;

        /// <summary>
        /// The header that was included in this packet.
        /// </summary>
        public CtpHeader Header;

        public FragmentReassembly()
        {
            Buffer = new byte[0];
        }

        public bool IsComplete => TotalSize == ReceivedSize;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="header"></param>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        public void ProcessFragment(CtpHeader header, byte[] buffer, int offset, int length)
        {
            int fragmentID = ToInt32(buffer, offset);
            int currentFragment = (int)(ushort)ToInt16(buffer, offset + 4); //Current fragment
            int totalFragments = (int)(ushort)ToInt16(buffer, offset + 6);

            if (currentFragment == 0)
            {
                Header = header;
                FragmentID = fragmentID;
                m_prevFragment = currentFragment;
                m_totalFragments = totalFragments;
                TotalSize = ToInt32(buffer, offset + 8);
                m_checksum = (uint)ToInt32(buffer, offset + 12);

                if (Buffer.Length < TotalSize)
                {
                    Buffer = new byte[TotalSize];
                }
                if (length - 16 > TotalSize)
                    throw new Exception("Fragment overflow. Too many bytes were received for a fragment.");
                ReceivedSize = length - 16;
                Array.Copy(buffer, offset + 16, Buffer, 0, length - 16);
                if (IsComplete)
                {
                    ValidateChecksum();
                }
            }
            else
            {
                if (IsComplete)
                    throw new Exception("Fragment has already been assembled");

                if (fragmentID != FragmentID)
                    throw new Exception("Fragment IDs don't match");

                if (totalFragments != m_totalFragments)
                    throw new Exception("Total fragment mismatched");

                if (m_prevFragment + 1 != currentFragment)
                    throw new Exception("Wrong Current Fragment Number");

                m_prevFragment++;

                if (ReceivedSize + length - 8 > TotalSize)
                    throw new Exception("Fragment overflow. Too many bytes were received by the packet.");

                Array.Copy(buffer, offset + 8, Buffer, ReceivedSize, length - 8);
                ReceivedSize += length - 8;

                if (IsComplete)
                {
                    ValidateChecksum();
                }
            }
        }

        private void ValidateChecksum()
        {
            if (m_checksum != Crc32.Compute(Buffer, 0, TotalSize))
            {
                throw new InvalidOperationException("Fragment Checksum is not valid.");
            }
        }

        private static short ToInt16(byte[] buffer, int startIndex)
        {
            return (short)((int)buffer[startIndex] << 8 | (int)buffer[startIndex + 1]);
        }

        private static int ToInt32(byte[] buffer, int startIndex)
        {
            return (int)buffer[startIndex + 0] << 24 |
                   (int)buffer[startIndex + 1] << 16 |
                   (int)buffer[startIndex + 2] << 8 |
                   (int)buffer[startIndex + 3];
        }

    }
}
