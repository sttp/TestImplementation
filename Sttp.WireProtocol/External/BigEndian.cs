//******************************************************************************************************
//  BigEndian.cs - Gbtc
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
//  05/06/2014 - Steven E. Chisholm
//       Generated original version of source code based on EndianOrder.cs
//  08/20/2014 - Steven E. Chisholm
//       Added encoding for decimal numbers and support for pointer methods.
//
//******************************************************************************************************

#region [ Contributor License Agreements ]

/**************************************************************************\
   Copyright © 2009 - J. Ritchie Carroll
   All rights reserved.
  
   Redistribution and use in source and binary forms, with or without
   modification, are permitted provided that the following conditions
   are met:
  
      * Redistributions of source code must retain the above copyright
        notice, this list of conditions and the following disclaimer.
       
      * Redistributions in binary form must reproduce the above
        copyright notice, this list of conditions and the following
        disclaimer in the documentation and/or other materials provided
        with the distribution.
  
   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDER "AS IS" AND ANY
   EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
   IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR
   PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
   CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
   EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
   PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
   PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY
   OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
   (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
   OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
  
\**************************************************************************/

#endregion

using System;

namespace Sttp
{
    /// <summary>
    /// Defines a set of big-endian byte order interoperability functions.
    /// </summary>
    /// <remarks>
    /// This class is setup to support aggressive in-lining of big endian conversions. Bounds
    /// will not be checked as part of this function call, if bounds are violated, the exception
    /// will be thrown at the <see cref="Array"/> level.
    /// </remarks>
    public static unsafe class BigEndian
    {
        #region [ ToValue Array ]

       
        /// <summary>
        /// Returns a 16-bit signed integer converted from two bytes, accounting for target endian-order, at a specified position in a byte array.
        /// </summary>
        /// <param name="buffer">An array of bytes (i.e., buffer containing binary image of value).</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <returns>A 16-bit signed integer formed by two bytes beginning at startIndex.</returns>
        /// <exception cref="ArgumentNullException">value is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">startIndex is less than zero or greater than the length of value minus 1.</exception>
        public static short ToInt16(byte[] buffer, int startIndex)
        {
            return (short)((int)buffer[startIndex] << 8 | (int)buffer[startIndex + 1]);
        }

       
        /// <summary>
        /// Returns a 32-bit signed integer converted from four bytes, accounting for target endian-order, at a specified position in a byte array.
        /// </summary>
        /// <param name="buffer">An array of bytes (i.e., buffer containing binary image of value).</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <returns>A 32-bit signed integer formed by four bytes beginning at startIndex.</returns>
        /// <exception cref="ArgumentNullException">value is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">startIndex is less than zero or greater than the length of value minus 1.</exception>
        public static int ToInt32(byte[] buffer, int startIndex)
        {
            return (int)buffer[startIndex + 0] << 24 |
                   (int)buffer[startIndex + 1] << 16 |
                   (int)buffer[startIndex + 2] << 8 |
                   (int)buffer[startIndex + 3];
        }

        /// <summary>
        /// Returns a 128-bit decimal converted from 16 bytes, accounting for target endian-order, at a specified position in a byte array.
        /// </summary>
        /// <param name="buffer">An array of bytes (i.e., buffer containing binary image of value).</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <returns>A 128-bit decimal formed by 16 bytes beginning at startIndex.</returns>
        /// <exception cref="ArgumentNullException">value is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">startIndex is less than zero or greater than the length of value minus 1.</exception>
        public static unsafe decimal ToDecimal(byte[] buffer, int startIndex)
        {
            buffer.ValidateParameters(startIndex, 16);

            fixed (byte* ptr = &buffer[startIndex])
            {
                if (!BitConverter.IsLittleEndian)
                    return *(decimal*)(ptr);

                decimal returnValue;
                byte* destination = (byte*)&returnValue;
                //int flags
                destination[0] = ptr[3];
                destination[1] = ptr[2];
                destination[2] = ptr[1];
                destination[3] = ptr[0];
                //int high
                destination[4] = ptr[7];
                destination[5] = ptr[6];
                destination[6] = ptr[5];
                destination[7] = ptr[4];
                //int low
                destination[8] = ptr[11];
                destination[9] = ptr[10];
                destination[10] = ptr[9];
                destination[11] = ptr[8];
                //int mid
                destination[12] = ptr[15];
                destination[13] = ptr[14];
                destination[14] = ptr[13];
                destination[15] = ptr[12];
                return returnValue;
            }
        }

        #endregion

        #region [ GetBytes ]

        /// <summary>
        /// Returns the specified 32-bit signed integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>An array of bytes with length 4.</returns>
        public static byte[] GetBytes(int value)
        {
            return new[]
            {
                (byte)(value >> 24),
                (byte)(value >> 16),
                (byte)(value >> 8),
                (byte)(value)
            };
        }

        
        #endregion

        #region [ Copy Bytes Array ]
     
        /// <summary>
        /// Copies the specified 128-bit decimal value as an array of 16 bytes in the target endian-order to the destination array.
        /// </summary>
        /// <param name="value">The number to convert and copy.</param>
        /// <param name="destinationArray">The destination buffer.</param>
        /// <param name="destinationIndex">The byte offset into <paramref name="destinationArray"/>.</param>
        /// <returns>Length of bytes copied into array based on size of <paramref name="value"/>.</returns>
        public static unsafe int CopyBytes(decimal value, byte[] destinationArray, int destinationIndex)
        {
            destinationArray.ValidateParameters(destinationIndex, 16);
            fixed (byte* destination = &destinationArray[destinationIndex])
            {
                if (!BitConverter.IsLittleEndian)
                {
                    *(decimal*)(destination) = value;
                }
                else
                {
                    byte* ptr = (byte*)&value;
                    //int flags
                    destination[0] = ptr[3];
                    destination[1] = ptr[2];
                    destination[2] = ptr[1];
                    destination[3] = ptr[0];
                    //int high
                    destination[4] = ptr[7];
                    destination[5] = ptr[6];
                    destination[6] = ptr[5];
                    destination[7] = ptr[4];
                    //int low
                    destination[8] = ptr[11];
                    destination[9] = ptr[10];
                    destination[10] = ptr[9];
                    destination[11] = ptr[8];
                    //int mid
                    destination[12] = ptr[15];
                    destination[13] = ptr[14];
                    destination[14] = ptr[13];
                    destination[15] = ptr[12];
                }
            }
            return 16;
        }

        #endregion

       
    }
}