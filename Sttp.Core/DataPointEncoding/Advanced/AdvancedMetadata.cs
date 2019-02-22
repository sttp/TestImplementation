//******************************************************************************************************
//  AdvancedMetadata.cs - Gbtc
//
//  Copyright © 2019, Grid Protection Alliance.  All Rights Reserved.
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
//  01/30/2019 - Steven E. Chisholm
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
    internal class AdvancedMetadata
    {
        public readonly SttpDataPointMetadata Metadata;

        public readonly int ChannelID;

        public long PrevQuality;
        public long PrevTime;
        public CtpObject PrevValue;
        public long TimeDelta1 = 0;
        public long TimeDelta2 = 0;
        public long TimeDelta3 = 0;
        public int NeighborChannelId;

        /// <summary>
        /// When reading the next point
        /// </summary>
        public AdvancedSymbolEncoding NextSymbolEncoding;

        public AdvancedMetadata(int channelID, SttpDataPointMetadata metadata, BitStreamWriter streamWriter, BitStreamReader streamReader)
        {
            Metadata = metadata;
            ChannelID = channelID;
            NextSymbolEncoding = new AdvancedSymbolEncoding(streamWriter, streamReader);
        }

        public void Assign(SttpDataPoint point)
        {
            PrevValue = point.Value;
            PrevQuality = point.Quality;
            PrevTime = point.Time.Ticks;
        }
    }
}
