//******************************************************************************************************
//  Encoding7Bit.cs - Gbtc
//
//  Copyright © 2014, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  03/16/2012 - Steven E. Chisholm
//       Generated original version of source code. 
//
//******************************************************************************************************

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using Sttp.IO;

namespace Sttp
{
    /// <summary>
    /// Contains 7 bit encoding functions
    /// </summary>
    public static class Encoding7Bit
    {
        #region [ 32 bit ]

        #region [ Write ]

        /// <summary>
        /// Writes the 7-bit encoded value to the provided stream.
        /// </summary>
        /// <param name="stream">the stream</param>
        /// <param name="position">a reference parameter to the starting position. 
        /// This field will be updated. when the function returns</param>
        /// <param name="value1">the value to write</param>
        [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "1#")]
        public static void Write(byte[] stream, ref int position, uint value1)
        {
            if (value1 < 128)
            {
                stream[position] = (byte)value1;
                position += 1;
                return;
            }
            stream[position] = (byte)(value1 | 128);
            if (value1 < 128 * 128)
            {
                stream[position + 1] = (byte)(value1 >> 7);
                position += 2;
                return;
            }
            stream[position + 1] = (byte)((value1 >> 7) | 128);
            if (value1 < 128 * 128 * 128)
            {
                stream[position + 2] = (byte)(value1 >> 14);
                position += 3;
                return;
            }
            stream[position + 2] = (byte)((value1 >> 14) | 128);
            if (value1 < 128 * 128 * 128 * 128)
            {
                stream[position + 3] = (byte)(value1 >> 21);
                position += 4;
                return;
            }
            stream[position + 3] = (byte)((value1 >> 21) | 128);
            stream[position + 4] = (byte)(value1 >> 28);
            position += 5;
        }

        #endregion

        #region [ Read ]

        /// <summary>
        /// Reads a 7-bit encoded uint.
        /// </summary>
        /// <param name="stream">the stream</param>
        /// <param name="position">the position in the stream. Position will be updated after reading</param>
        /// <returns>The value</returns>
        [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "1#")]
        public static uint ReadUInt32(byte[] stream, ref int position)
        {
            int pos = position;
            uint value11;
            value11 = stream[pos];
            if (value11 < 128)
            {
                position = pos + 1;
                return value11;
            }
            value11 ^= ((uint)stream[pos + 1] << 7);
            if (value11 < 128 * 128)
            {
                position = pos + 2;
                return value11 ^ 0x80;
            }
            value11 ^= ((uint)stream[pos + 2] << 14);
            if (value11 < 128 * 128 * 128)
            {
                position = pos + 3;
                return value11 ^ 0x4080;
            }
            value11 ^= ((uint)stream[pos + 3] << 21);
            if (value11 < 128 * 128 * 128 * 128)
            {
                position = pos + 4;
                return value11 ^ 0x204080;
            }
            value11 ^= ((uint)stream[pos + 4] << 28) ^ 0x10204080;
            position = pos + 5;
            return value11;
        }

        #endregion

        #endregion

        #region [ 64 bit ]

        #region [ Write ]

        /// <summary>
        /// Writes the 7-bit encoded value to the provided stream.
        /// </summary>
        /// <param name="stream">the stream</param>
        /// <param name="position">a reference parameter to the starting position. 
        /// This field will be updated. when the function returns</param>
        /// <param name="value1">the value to write</param>
        [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "1#")]
        public static void Write(byte[] stream, ref int position, ulong value1)
        {
            if (value1 < 128)
            {
                stream[position] = (byte)value1;
                position += 1;
                return;
            }
            stream[position] = (byte)(value1 | 128);
            if (value1 < 128 * 128)
            {
                stream[position + 1] = (byte)(value1 >> 7);
                position += 2;
                return;
            }
            stream[position + 1] = (byte)((value1 >> 7) | 128);
            if (value1 < 128 * 128 * 128)
            {
                stream[position + 2] = (byte)(value1 >> (7 + 7));
                position += 3;
                return;
            }
            stream[position + 2] = (byte)((value1 >> (7 + 7)) | 128);
            if (value1 < 128 * 128 * 128 * 128)
            {
                stream[position + 3] = (byte)(value1 >> (7 + 7 + 7));
                position += 4;
                return;
            }
            stream[position + 3] = (byte)((value1 >> (7 + 7 + 7)) | 128);
            if (value1 < 128L * 128 * 128 * 128 * 128)
            {
                stream[position + 4] = (byte)(value1 >> (7 + 7 + 7 + 7));
                position += 5;
                return;
            }
            stream[position + 4] = (byte)((value1 >> (7 + 7 + 7 + 7)) | 128);
            if (value1 < 128L * 128 * 128 * 128 * 128 * 128)
            {
                stream[position + 5] = (byte)(value1 >> (7 + 7 + 7 + 7 + 7));
                position += 6;
                return;
            }
            stream[position + 5] = (byte)((value1 >> (7 + 7 + 7 + 7 + 7)) | 128);
            if (value1 < 128L * 128 * 128 * 128 * 128 * 128 * 128)
            {
                stream[position + 6] = (byte)(value1 >> (7 + 7 + 7 + 7 + 7 + 7));
                position += 7;
                return;
            }
            stream[position + 6] = (byte)((value1 >> (7 + 7 + 7 + 7 + 7 + 7)) | 128);
            if (value1 < 128L * 128 * 128 * 128 * 128 * 128 * 128 * 128)
            {
                stream[position + 7] = (byte)(value1 >> (7 + 7 + 7 + 7 + 7 + 7 + 7));
                position += 8;
                return;
            }
            stream[position + 7] = (byte)(value1 >> (7 + 7 + 7 + 7 + 7 + 7 + 7) | 128);
            stream[position + 8] = (byte)(value1 >> (7 + 7 + 7 + 7 + 7 + 7 + 7 + 7));
            position += 9;
        }

        
        #endregion

        #region [ Read ]

        /// <summary>
        /// Reads a 7-bit encoded ulong.
        /// </summary>
        /// <param name="stream">the stream</param>
        /// <param name="position">the position in the stream. Position will be updated after reading</param>
        /// <returns>The value</returns>
        [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "1#")]
        public static ulong ReadUInt64(byte[] stream, ref int position)
        {
            int pos = position;
            ulong value11;
            value11 = stream[pos];
            if (value11 < 128)
            {
                position += 1;
                return value11;
            }
            value11 ^= ((ulong)stream[pos + 1] << (7));
            if (value11 < 128 * 128)
            {
                position += 2;
                return value11 ^ 0x80;
            }
            value11 ^= ((ulong)stream[pos + 2] << (7 + 7));
            if (value11 < 128 * 128 * 128)
            {
                position += 3;
                return value11 ^ 0x4080;
            }
            value11 ^= ((ulong)stream[pos + 3] << (7 + 7 + 7));
            if (value11 < 128 * 128 * 128 * 128)
            {
                position += 4;
                return value11 ^ 0x204080;
            }
            value11 ^= ((ulong)stream[pos + 4] << (7 + 7 + 7 + 7));
            if (value11 < 128L * 128 * 128 * 128 * 128)
            {
                position += 5;
                return value11 ^ 0x10204080L;
            }
            value11 ^= ((ulong)stream[pos + 5] << (7 + 7 + 7 + 7 + 7));
            if (value11 < 128L * 128 * 128 * 128 * 128 * 128)
            {
                position += 6;
                return value11 ^ 0x810204080L;
            }
            value11 ^= ((ulong)stream[pos + 6] << (7 + 7 + 7 + 7 + 7 + 7));
            if (value11 < 128L * 128 * 128 * 128 * 128 * 128 * 128)
            {
                position += 7;
                return value11 ^ 0x40810204080L;
            }
            value11 ^= ((ulong)stream[pos + 7] << (7 + 7 + 7 + 7 + 7 + 7 + 7));
            if (value11 < 128L * 128 * 128 * 128 * 128 * 128 * 128 * 128)
            {
                position += 8;
                return value11 ^ 0x2040810204080L;
            }
            value11 ^= ((ulong)stream[pos + 8] << (7 + 7 + 7 + 7 + 7 + 7 + 7 + 7));
            position += 9;
            return value11 ^ 0x102040810204080L;
        }

        #endregion

        #endregion
    }
}