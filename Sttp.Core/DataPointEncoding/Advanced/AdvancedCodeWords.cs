//******************************************************************************************************
//  AdvancedCodeWords.cs - Gbtc
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
    internal enum AdvancedCodeWords : byte
    {
        PointID = 0,
        Timestamp1 = 1,
        Timestamp2 = 2,
        Timestamp3 = 3,
        Timestamp4 = 4,
        Timestamp5 = 29,
        NewPoint = 30,
        TimestampElse = 31,
        Quality = 5,
        Type = 6,
        ValuePrev = 7,
        ValueDefault = 8,
        ValueRaw = 9,
        ValueXOR4 = 10,
        ValueXOR8 = 11,
        ValueXOR10 = 12,
        ValueXOR12 = 13,
        ValueXOR14 = 14,
        ValueXOR16 = 15,
        ValueXOR18 = 16,
        ValueXOR20 = 17,
        ValueXOR24 = 18,
        ValueXOR28 = 19,
        ValueXOR32 = 20,
        ValueXOR36 = 21,
        ValueXOR40 = 22,
        ValueXOR44 = 23,
        ValueXOR48 = 24,
        ValueXOR52 = 25,
        ValueXOR56 = 26,
        ValueXOR60 = 27,
        ValueTrue = 28,
    }

}
