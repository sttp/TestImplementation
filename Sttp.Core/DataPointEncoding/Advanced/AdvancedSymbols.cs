//******************************************************************************************************
//  AdvancedSymbols.cs - Gbtc
//
//  Copyright © 2019, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License", you may
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
//  01/30/2019 - Steven E. Chisholm
//       Generated original version of source code.
//
//******************************************************************************************************

namespace Sttp.DataPointEncoding
{
    /// <summary>
    /// These symbols will be used to identify how the data will be encoded. The pattern is, symbol, data, symbol, data.
    /// Symbols must be presented in sequential order. Meaning if the first symbol present is a Value symbol, a default
    /// symbol is automatically assumed for ChannelID, Timestamp, Quality, and Type.
    /// </summary>
    public enum AdvancedSymbols : byte
    {
        DefineChannel = 0,
        ChannelID = 1,
        TimestampDelta1 = 2,
        TimestampDelta2 = 3,
        TimestampDelta3 = 4,
        TimestampElse = 5,
        Quality = 6,
        Type = 7,
        ValueDefault = 8,
        ValueTrue = 9,
        ValueBits128 = 10,
        ValueBuffer = 11,
        ValueLast = 12,
        ValueBits4 = 13,
        ValueBits8 = 14,
        ValueBits12 = 15,
        ValueBits16 = 16,
        ValueBits20 = 17,
        ValueBits24 = 18,
        ValueBits28 = 19,
        ValueBits32 = 20,
        ValueBits36 = 21,
        ValueBits40 = 22,
        ValueBits44 = 23,
        ValueBits48 = 24,
        ValueBits52 = 25,
        ValueBits56 = 26,
        ValueBits60 = 27,
        ValueBits64 = 28,
    }



}
