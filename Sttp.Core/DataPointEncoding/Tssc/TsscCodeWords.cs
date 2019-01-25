//******************************************************************************************************
//  TsscCodeWords.cs - Gbtc
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

namespace Sttp.DataPointEncoding
{

    /// <summary>
    /// The encoding commands supported by TSSC. This class is used by 
    /// <see cref="TsscDecoder"/> and <see cref="TsscEncoder"/>.
    /// </summary>
    internal static class TsscCodeWordsFloat //For Int64, Float, Double, Time
    {
        public const byte ValueZero = 1;
        public const byte ValueXOR0 = 2;
        public const byte ValueXOR5 = 3;
        public const byte ValueXOR7 = 4;
        public const byte ValueXOR9 = 5;
        public const byte ValueXOR11 = 6;
        public const byte ValueXOR13 = 7;
        public const byte ValueXOR15 = 8;
        public const byte ValueXOR17 = 9;
        public const byte ValueXOR19 = 10;
        public const byte ValueXOR21 = 11;
        public const byte ValueXOR23 = 12;
        public const byte ValueXOR26 = 13;
        public const byte ValueXOR28 = 14;
        public const byte ValueXOR32 = 15;
    }

    /// <summary>
    /// The encoding commands supported by TSSC. This class is used by 
    /// <see cref="TsscDecoder"/> and <see cref="TsscEncoder"/>.
    /// </summary>
    internal static class TsscCodeWordsBool
    {
        public const byte ValueTrue = 21;
        public const byte ValueFalse = 22;
    }

    /// <summary>
    /// The encoding commands supported by TSSC. This class is used by 
    /// <see cref="TsscDecoder"/> and <see cref="TsscEncoder"/>.
    /// </summary>
    internal static class TsscCodeWordsElse //For Guid, String, CtpBuffer, CtpCommand
    {
        public const byte Value1 = 21;
        public const byte Value2 = 22;
        public const byte Value3 = 23;
        public const byte ValueEmpty = 24;
        public const byte ValueRaw = 25;
    }


}
