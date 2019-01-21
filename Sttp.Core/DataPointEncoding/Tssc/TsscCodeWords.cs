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
    internal static class TsscCodeWordsCommon 
    {
        /// <summary>
        /// This code word will be adaptive.
        /// </summary>
        public const byte TimeDelta1Forward = 0;
        public const byte TimeDelta2Forward = 1;
        public const byte TimeDelta3Forward = 2;
        public const byte TimeDelta4Forward = 3;
        public const byte TimeDelta1Reverse = 4;
        public const byte TimeDelta2Reverse = 5;
        public const byte TimeDelta3Reverse = 6;
        public const byte TimeDelta4Reverse = 7;
        public const byte Timestamp2 = 8;
        public const byte TimeXOR7Bit = 9;

        public const byte Quality2 = 10;
        public const byte Quality7Bit32 = 11;

        public const byte ValueTypeChanged = 12;
    }


    /// <summary>
    /// The encoding commands supported by TSSC. This class is used by 
    /// <see cref="TsscDecoder"/> and <see cref="TsscEncoder"/>.
    /// </summary>
    internal static class TsscCodeWordsNumeric //For Int64, Float, Double, Time
    {
        public const byte Value1 = 13;
        public const byte Value2 = 14;
        public const byte Value3 = 15;
        public const byte ValueZero = 16;
        public const byte ValueXOR4 = 17;
        public const byte ValueXOR8 = 18;
        public const byte ValueXOR12 = 19;
        public const byte ValueXOR16 = 20;
        public const byte ValueXOR20 = 21;
        public const byte ValueXOR24 = 22;
        public const byte ValueXOR28 = 23;
        public const byte ValueXOR32 = 24;
        public const byte ValueXOR40 = 25;
        public const byte ValueXOR48 = 26;
        public const byte ValueXOR56 = 27;
        public const byte ValueXOR64 = 28;
    }

    /// <summary>
    /// The encoding commands supported by TSSC. This class is used by 
    /// <see cref="TsscDecoder"/> and <see cref="TsscEncoder"/>.
    /// </summary>
    internal static class TsscCodeWordsBool
    {
        public const byte ValueTrue = 13;
        public const byte ValueFalse = 14;
    }

    /// <summary>
    /// The encoding commands supported by TSSC. This class is used by 
    /// <see cref="TsscDecoder"/> and <see cref="TsscEncoder"/>.
    /// </summary>
    internal static class TsscCodeWordsElse //For Guid, String, CtpBuffer, CtpCommand
    {
        public const byte Value1 = 13;
        public const byte Value2 = 14;
        public const byte Value3 = 15;
        public const byte ValueEmpty = 16;
        public const byte ValueRaw = 17;
    }


}
