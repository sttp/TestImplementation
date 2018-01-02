//******************************************************************************************************
//  StreamExtensions.cs - Gbtc
//
//  Copyright © 2012, Grid Protection Alliance.  All Rights Reserved.
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
//  09/19/2008 - J. Ritchie Carroll
//       Generated original version of source code.
//  10/24/2008 - Pinal C. Patel
//       Edited code comments.
//  09/14/2009 - Stephen C. Wills
//       Added new header and license agreement.
//  11/23/2011 - J. Ritchie Carroll
//       Modified copy stream to use buffer pool.
//  12/14/2012 - Starlynn Danyelle Gilliam
//       Modified Header.
//  08/15/2014 - Steven E. Chisholm
//       Added stream encoding functions. 
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Sttp.IO
{
    /// <summary>
    /// Defines extension functions related to <see cref="Stream"/> manipulation.
    /// </summary>
    public static class BigEndianStreamExtensions
    {
       
        #region [ byte array ]

        /// <summary>
        /// Writes the entire buffer to the <paramref name="stream"/>
        /// </summary>
        /// <param name="stream">the stream to write to</param>
        /// <param name="value">the value to write</param>
        public static void Write(this Stream stream, byte[] value)
        {
            if (value.Length > 0)
                stream.Write(value, 0, value.Length);
        }

        /// <summary>
        /// Reads all of the provided bytes. Will not return prematurely, 
        /// but continue to execute a <see cref="Stream.Read"/> command until the entire
        /// <paramref name="length"/> has been read.
        /// </summary>
        /// <param name="stream">The stream to read from</param>
        /// <param name="buffer">The buffer to write to</param>
        /// <param name="position">the start position in the <paramref name="buffer"/></param>
        /// <param name="length">the number of bytes to read</param>
        /// <exception cref="EndOfStreamException">occurs if the end of the stream has been reached.</exception>
        public static void ReadAll(this Stream stream, byte[] buffer, int position, int length)
        {
            buffer.ValidateParameters(position, length);
            while (length > 0)
            {
                int bytesRead = stream.Read(buffer, position, length);
                if (bytesRead == 0)
                    throw new EndOfStreamException();
                length -= bytesRead;
                position += bytesRead;
            }
        }

        #endregion

       
    }
}