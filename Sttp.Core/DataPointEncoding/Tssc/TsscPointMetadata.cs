//******************************************************************************************************
//  TsscPointMetadata.cs - Gbtc
//
//  Copyright © 2016, Grid Protection Alliance.  All Rights Reserved.
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
//  12/02/2016 - Steven E. Chisholm
//       Generated original version of source code.
//
//******************************************************************************************************


using System;
using CTP;

namespace Sttp.DataPointEncoding
{
    /// <summary>
    /// The metadata kept for each pointID.
    /// </summary>
    internal class TsscPointMetadata
    {
        public int PrevNextPointId1;
        public int PrevNextPointId2;

        public ulong PrevQuality1;
        public ulong PrevQuality2;

        public CtpTypeCode PrevTypeCode;
        public ulong PrevValue1;
        public ulong PrevValue2;
        public ulong PrevValue3;

        public CtpObject Prev1;
        public CtpObject Prev2;
        public CtpObject Prev3;

        public TsscPointIDWordEncoding PointIDEncoding;
        public TsscWordEncoding ValueEncoding;

        public TsscPointMetadata(ByteWriter writer, ByteReader reader)
        {
            PointIDEncoding = new TsscPointIDWordEncoding(writer, reader);
            ValueEncoding = new TsscWordEncoding(writer, reader);
        }
    }
}
